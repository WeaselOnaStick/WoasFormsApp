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
        private HttpClient GetClient()
        {
            var client = factory.CreateClient("jira");
            return client;
        }

        public class CustomerCreateResponse
        {
            public string accountId { get; set; } = "";
        }

        public async Task<JiraCustomerView?> CreateCustomerFromCurrentUser()
        {
            var client = GetClient();
            var curUser = await dba.GetCurrentUser();
            if (curUser == null) return null;

            var request = new HttpRequestMessage(HttpMethod.Post, "https://woasforms.atlassian.net/rest/servicedeskapi/customer");
            request.Content = JsonContent.Create(
                new
                {
                    displayName = curUser.UserName,
                    email = curUser.Email,
                }
                );
            var response = await client.SendAsync(request);
            try { response.EnsureSuccessStatusCode(); } catch (Exception) { return null; }

            var responseRead = await response.Content.ReadFromJsonAsync<CustomerCreateResponse>();
            curUser.JiraAccountId = responseRead.accountId;
            await ctx.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public class SearchIssueResponseItem
        {
            public string id { get; set; }
        }

        public class SearchIssuesByUserResponse
        {
            public List<SearchIssueResponseItem> issues { get; set; } = new List<SearchIssueResponseItem>();
        }

        public record IssueGetResponse
        {
            public string id { get; init; }
            public IssueGetResponseFields fields { get; init; }
        }

        public record IssueGetResponseFields
        {
            public DateTime created { get; init; }
            public string summary { get; init; }
            public IssueGetResponseStatus status { get; init; }
            public IssueGetResponseReporter reporter { get; init; }
        }

        public record IssueGetResponseStatus { public string name { get; init; } }
        public record IssueGetResponseReporter { public string accountId { get; init; } }

        private async Task<List<JiraTicketView>> GetTicketsByUser(string userId)
        {
            var res = new List<JiraTicketView>();
            var client = GetClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"search/jql?jql=reporter={userId}");
            var response = await client.SendAsync(request);
            try { response.EnsureSuccessStatusCode(); } catch (Exception) { return null; }

            var responseRead = await response.Content.ReadFromJsonAsync<SearchIssuesByUserResponse>();

            foreach (var ticketId in responseRead.issues)
            {
                var ticketRequest = new HttpRequestMessage(HttpMethod.Get, $"issue/{ticketId.id}");
                var ticketResponse = await client.SendAsync(ticketRequest);

                try { ticketResponse.EnsureSuccessStatusCode(); } catch (Exception) { continue; }

                var ticketResponseRead = await ticketResponse.Content.ReadFromJsonAsync<IssueGetResponse>();
                if (ticketResponseRead == null) continue;
                res.Append(new JiraTicketView
                {
                    Id = ticketId.id,
                    Status = ticketResponseRead.fields.status.name,
                    Summary = ticketResponseRead.fields.summary,
                    FiledOn = ticketResponseRead.fields.created,
                });
            }

            return res;
        }

        public class JiraCustomerGetResponse
        {
            public string accountId { get; set; }
            public string displayName { get; set; }
            public string emailAddress { get; set; }
        }

        private async Task<JiraCustomerView?> GetCustomerById(string userId)
        {
            var client = GetClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"user/?accountId={userId}");
            var response = await client.SendAsync(request);

            try { response.EnsureSuccessStatusCode(); } catch (Exception) { return null; }

            var responseRead = await response.Content.ReadFromJsonAsync<JiraCustomerGetResponse>();
            if (responseRead == null) return null;

            return new JiraCustomerView
            {
                AccountId = responseRead.accountId,
                DisplayName = responseRead.displayName,
                Email = responseRead.emailAddress,
                Tickets = await GetTicketsByUser(responseRead.accountId),
            };
        }

        public async Task<JiraCustomerView?> GetCustomerFromCurrentUser()
        {
            var curUser = await dba.GetCurrentUser();
            if (curUser == null || string.IsNullOrWhiteSpace(curUser.JiraAccountId)) return null;

            return await GetCustomerById(curUser.JiraAccountId);
        }

        public async Task<JiraTicketView?> CreateTicket(JiraTicketView model)
        {
            throw new NotImplementedException();
        }
    }
}
