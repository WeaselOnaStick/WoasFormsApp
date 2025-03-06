using Azure.Core;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using WoasFormsApp.Data;

namespace WoasFormsApp.Services
{
    public class JiraTicketsService(
        IHttpClientFactory factory,
        IDatabaseAccessorService dba,
        WoasFormsDbContext ctx) : IJiraTicketsService
    {
        private HttpClient GetClient() => factory.CreateClient("jira");    
        private HttpClient GetClientDesk() => factory.CreateClient("jira_servicedesk");

        public record CustomerCreateResponse (string accountId);

        public async Task<JiraCustomerView?> CreateCustomerFromCurrentUser(string? overrideEmail = null)
        {
            var client = GetClientDesk();
            var curUser = await dba.GetCurrentUser();
            if (curUser == null) return null;
            var request = new HttpRequestMessage(HttpMethod.Post,"customer/");
            request.Content = JsonContent.Create(
                new
                {
                    displayName = curUser.UserName,
                    email = overrideEmail ?? curUser.Email,
                }
                );
            var response = await client.SendAsync(request);
            try { response.EnsureSuccessStatusCode(); } catch (Exception) { return null; }

            var responseRead = await response.Content.ReadFromJsonAsync<CustomerCreateResponse>();
            curUser.JiraAccountId = responseRead.accountId;
            await ctx.SaveChangesAsync();
            return await GetCustomerById(responseRead.accountId);
        }

        public record IssueGetResponse(
            string id,
            IssueGetResponseFields fields
        );

        public record IssueGetResponseFields(
            string summary,
            IssueGetResponseStatus status,
            IssueGetResponsePriority priority,
            IssueGetResponseReporter reporter
        );

        public record IssueGetResponseStatus(string name);
        public record IssueGetResponsePriority(string name);
        public record IssueGetResponseReporter(string accountId);

        public async Task<JiraTicketView?> GetTicketById(string ticketId)
        {
            var client = GetClient();
            var ticketRequest = new HttpRequestMessage(HttpMethod.Get, $"issue/{ticketId}");
            var ticketResponse = await client.SendAsync(ticketRequest);

            try { ticketResponse.EnsureSuccessStatusCode(); } catch (Exception) { return null; }

            var ticketResponseRead = await ticketResponse.Content.ReadFromJsonAsync<IssueGetResponse>();
            if (ticketResponseRead == null) return null;
            return new JiraTicketView
            {
                Id = ticketId,
                Status = ticketResponseRead.fields.status.name,
                Priority = ticketResponseRead.fields.priority.name,
                Summary = ticketResponseRead.fields.summary,
            };
        }

        public record SearchIssueResponseItem(string id);
        public record SearchIssuesByUserResponse(List<SearchIssueResponseItem> issues);

        public record FetchIssuesResponse(IssueGetResponse[] issues);

        private async Task<List<JiraTicketView>> GetTicketsByUser(string userId)
        {
            var res = new List<JiraTicketView>();
            var client = GetClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"search/jql?jql=reporter={userId}");
            var response = await client.SendAsync(request);
            try { response.EnsureSuccessStatusCode(); } catch (Exception) { return null; }

            var responseRead = await response.Content.ReadFromJsonAsync<SearchIssuesByUserResponse>();
            responseRead ??= new SearchIssuesByUserResponse(new());
            var ticketIds = responseRead.issues.Select(i => i.id);

            var requestFetch = new HttpRequestMessage(HttpMethod.Post, $"issue/bulkfetch");
            requestFetch.Content = JsonContent.Create(new { 
                expand = new[] {"names"},
                fields = new[] {"summary", "status", "priority"},
                issueIdsOrKeys = ticketIds.ToArray(),
            });

            var responseFetch = await client.SendAsync(requestFetch);
            try { responseFetch.EnsureSuccessStatusCode(); } catch (Exception) { return null; }
            var responseFetchRead = await responseFetch.Content.ReadFromJsonAsync<FetchIssuesResponse>();

            res.AddRange(responseFetchRead.issues.Select(fetched => new JiraTicketView
            {
                Id = fetched.id,
                Status = fetched.fields.status.name,
                Priority = fetched.fields.priority.name,
                Summary = fetched.fields.summary,
            }));
            return res;
        }

        public record JiraCustomerGetResponse(  
            string accountId,
            string displayName,
            string emailAddress
        );


        private async Task<JiraCustomerView?> GetCustomerByEmail(string email)
        {
            var client = GetClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"user/search/?query={email}");
            var response = await client.SendAsync(request);

            try { response.EnsureSuccessStatusCode(); } catch (Exception) { return null; }

            var responseRead = await response.Content.ReadFromJsonAsync<JiraCustomerGetResponse[]>();
            if (responseRead == null || responseRead.Count() == 0) return null;

            var foundUser = responseRead.First();
            var tickets = await GetTicketsByUser(foundUser.accountId);
            var result = new JiraCustomerView
            {
                AccountId = foundUser.accountId,
                DisplayName = foundUser.displayName,
                Email = foundUser.emailAddress,
                Tickets = tickets,
            };

            return result;

        }

        private async Task<JiraCustomerView?> GetCustomerById(string userId)
        {
            var client = GetClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"user/?accountId={userId}");
            var response = await client.SendAsync(request);

            try { response.EnsureSuccessStatusCode(); } catch (Exception) { return null; }

            var responseRead = await response.Content.ReadFromJsonAsync<JiraCustomerGetResponse>();
            if (responseRead == null) return null;

            var tickets = await GetTicketsByUser(responseRead.accountId);
            var result = new JiraCustomerView
            {
                AccountId = responseRead.accountId,
                DisplayName = responseRead.displayName,
                Email = responseRead.emailAddress,
                Tickets = tickets,
            };

            return result;
        }

        public async Task<JiraCustomerView?> GetCustomerFromCurrentUser()
        {
            var curUser = await dba.GetCurrentUser();
            if (curUser == null) return null;
            if (!await CreateCustomerIfNeeded()) return null;
            ctx.Update(curUser);

            return await GetCustomerById(curUser.JiraAccountId);
        }

        private async Task<bool> CreateCustomerIfNeeded(string? overrideEmail = null)
        {
            var curUser = await dba.GetCurrentUser();
            if (curUser == null) return false;

            if (string.IsNullOrWhiteSpace(curUser.JiraAccountId))
            {
                var foundCustomer = await GetCustomerByEmail(curUser.Email??"");
                if (foundCustomer != null)
                {
                    curUser.JiraAccountId = foundCustomer.AccountId;
                    await ctx.SaveChangesAsync();
                }
                else
                {
                    var newSupportCustomer = await CreateCustomerFromCurrentUser(overrideEmail);
                    if (newSupportCustomer == null) return false;
                }
            }
            return true;
        }


        public record IssueCreateResponse(string id);

        public async Task<JiraTicketView?> CreateTicketByCurrentUser(NewTicketModel model)
        {
            await CreateCustomerIfNeeded(overrideEmail: model.Email);
            var curUser = await dba.GetCurrentUser();
            if (curUser == null) return null;
            var reporterId = curUser.JiraAccountId;

            var payload = new
            {
                fields = new
                {
                    project = new { key = "SUP" },
                    issuetype = new { id = "10006" },
                    summary = model.Summary,
                    reporter = new { id = reporterId}
                }
            };

            var client = GetClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "issue");
            request.Content = JsonContent.Create(payload);

            var response = await client.SendAsync(request);
            try { response.EnsureSuccessStatusCode(); } catch (Exception) { return null; }
            var newTicketId = (await response.Content.ReadFromJsonAsync<IssueCreateResponse>())!.id;

            return await GetTicketById(newTicketId);
        }

    }
}
