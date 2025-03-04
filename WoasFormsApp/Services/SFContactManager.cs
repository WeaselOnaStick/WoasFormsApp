using Duende.AccessTokenManagement;
using Duende.IdentityModel.Client;
using System.Text.Json;
using System.Text.Json.Nodes;
using WoasFormsApp.Data;    

namespace WoasFormsApp.Services
{
    public class SFContactManager(
        IHttpClientFactory factory,
        IConfiguration config,
        WoasFormsDbContext ctx,
        IClientCredentialsTokenManagementService tokenManager,
        IDatabaseAccessorService dba) : ISFContactManager
    {
        private string _ver = "v63.0";

        private async Task<HttpClient> GetClient()
        {
            var client = factory.CreateClient("salesforce");
            var tokenReq = await tokenManager.GetAccessTokenAsync("salesforce");
            var token = tokenReq.AccessToken;
            client.SetBearerToken(token ?? "");
            return client;
        }

        public class CreateResponse
        {
            public class ResponseItem
            {
                public class Body
                {
                    public string id { get; set; }
                    public bool success { get; set; }
                }
                public Body body { get; set; }
                public required int httpStatusCode { get; set; }
                public required string referenceId { get; set; }
            }

            public List<ResponseItem> compositeResponse { get; set; }
        }

        public async Task<sfUserDataView?> CreateCurrentUserSFContact()
        {
            var curUser = await dba.GetCurrentUser();
            if (curUser == null) return null;
            var request = new HttpRequestMessage(HttpMethod.Post, $"/services/data/{_ver}/composite");

            var actions = new List<dynamic>();
            actions.Add(new
            {
                method = "POST",
                url = $"/services/data/{_ver}/sobjects/Account",
                referenceId = "NewAccount",
                body = new
                {
                    Name = curUser.UserName
                }
            });
            actions.Add(new
            {
                method = "POST",
                url = $"/services/data/{_ver}/sobjects/Contact",
                referenceId = "NewContact",
                body = new
                {
                    AccountId = "@{NewAccount.id}",
                    LastName = curUser.UserName ?? "",
                    Email = curUser.Email ?? "",
                }
            });

            var contentJson = new
            {
                allOrNone = true,
                compositeRequest = actions.ToArray(),
            };

            request.Content = JsonContent.Create(contentJson);
            var Client = await GetClient();
            var response = await Client.SendAsync(request);

            try {response.EnsureSuccessStatusCode();} catch (Exception) { return null;}
            var responseRead = await response.Content.ReadFromJsonAsync<CreateResponse>();

            curUser.SalesForceAccountId = responseRead.compositeResponse.First(x => x.referenceId == "NewAccount").body.id;
            curUser.SalesForceContactId = responseRead.compositeResponse.First(x => x.referenceId == "NewContact").body.id;
            await ctx.SaveChangesAsync();
            return await GetUserSFContact(curUser.SalesForceContactId);
        }

        public class GetContactResponse
        {
            public string? Email { get; set; }
            public string Name { get; set; }
            public string? FirstName { get; set; }
            public string LastName { get; set; }
            public string? Description { get; set; }
            public DateOnly? Birthdate { get; set; }
        }

        private async Task<sfUserDataView?> GetUserSFContact(string id)
        {
            var Client = await GetClient();
            var response = await Client.GetFromJsonAsync<GetContactResponse>($"/services/data/{_ver}/sobjects/Contact/{id}");
            if (response == null) return null;
            sfUserDataView res = new sfUserDataView
            {
                SalesForceContactID = id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Email = response.Email ?? "",
                About = response.Description ?? "",
                BirthDay = response.Birthdate,
            };
            return res;
        }

        public async Task<sfUserDataView?> GetCurrentUserSFContact()
        {
            var curUser = await dba.GetCurrentUser();
            if (curUser == null || string.IsNullOrWhiteSpace(curUser.SalesForceContactId)) return null;
            return await GetUserSFContact(curUser.SalesForceContactId);
        }

        public async Task<bool> UpdateCurrentUserSFContact(sfUserDataView view)
        {
            var Client = await GetClient();
            var request = new HttpRequestMessage(HttpMethod.Patch, $"/services/data/{_ver}/sobjects/Contact/{view.SalesForceContactID}");
            var contentDict = new
            {
                view.Email,
                Description = view.About,
                Birthdate = view.BirthDay,
                view.FirstName,
                view.LastName,
            };
            request.Content = JsonContent.Create(contentDict);
            
            var response = await Client.SendAsync(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
