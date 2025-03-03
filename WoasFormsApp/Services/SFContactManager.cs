using Duende.AccessTokenManagement;
using Duende.IdentityModel.Client;

namespace WoasFormsApp.Services
{
    public class SFContactManager(
        IHttpClientFactory factory, 
        IClientCredentialsTokenManagementService tokenManager,
        IConfiguration config) : ISFContactManager
    {
        private HttpClient? _client;

        public async Task<sfUserDataView?> GetCurrentUserSFContact()
        {
            _client = factory.CreateClient("salesforce");
          
            _client.SetBasicAuthenticationOAuth(
                userName: config["SalesForce:USERNAME"]!,
                password: config["SalesForce:PASSWORD"]!
            );

            var token = await tokenManager.GetAccessTokenAsync("salesforce");
            _client.SetBearerToken(token.AccessToken!);
            var res = await _client.GetAsync("/services/data/v63.0/sobjects/account/quickActions");
            throw new NotImplementedException();
        }
    }
}
