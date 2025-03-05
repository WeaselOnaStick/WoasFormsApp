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
            IssueGetResponseReporter reporter
        );

        public record IssueGetResponseStatus(string name);
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
                Summary = ticketResponseRead.fields.summary,
            };
        }

        public record SearchIssueResponseItem(string id);
        public record SearchIssuesByUserResponse(List<SearchIssueResponseItem> issues);

        private async Task<List<JiraTicketView>> GetTicketsByUser(string userId)
        {
            var res = new List<JiraTicketView>();
            var client = GetClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"search/jql?jql=reporter={userId}");
            var response = await client.SendAsync(request);
            try { response.EnsureSuccessStatusCode(); } catch (Exception) { return null; }

            var responseRead = await response.Content.ReadFromJsonAsync<SearchIssuesByUserResponse>();

            foreach (var ticketId in responseRead!.issues)
            {
                var foundTicket = await GetTicketById(ticketId.id);
                if (foundTicket != null)
                    res.Add(foundTicket);
            }

            return res;
        }

        public record JiraCustomerGetResponse(  
            string accountId,
            string displayName,
            string emailAddress
        );

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
            if (curUser == null || string.IsNullOrWhiteSpace(curUser.JiraAccountId)) return null;

            return await GetCustomerById(curUser.JiraAccountId);
        }

        public record IssueCreateResponse(string id);

        public async Task<JiraTicketView?> CreateTicketByCurrentUser(NewTicketModel model)
        {
            var curUser = await dba.GetCurrentUser();
            if (curUser == null) return null;
            var reporterId = curUser.JiraAccountId;

            if (string.IsNullOrWhiteSpace(reporterId))
            {
                var newSupportCustomer = await CreateCustomerFromCurrentUser(overrideEmail: model.Email);
                if (newSupportCustomer == null) return null;
                reporterId = newSupportCustomer.AccountId; 
            }

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
