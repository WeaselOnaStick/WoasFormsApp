using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WoasFormsApp.Data;

namespace WoasFormsApp.Services
{
    public class DatabaseAccessorService : IDatabaseAccessorService
    {
        WoasFormsDbContext _ctx;
        AuthenticationStateProvider _asp;
        UserManager<WoasFormsAppUser> _users;

        public DatabaseAccessorService(
            WoasFormsDbContext context, 
            AuthenticationStateProvider authenticationStateProvider,
            UserManager<WoasFormsAppUser> userManager)
        {
            _ctx = context;
            _asp = authenticationStateProvider;
            _users = userManager;
        }


        public async Task CreateTemplate(Template template)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Template>> GetAvailableTemplates()
        {
            
            List<Template> res = await _ctx.Templates.Where(t => t.Public).ToListAsync();
            var user = (await _asp.GetAuthenticationStateAsync()).User;
            if (user.Identity.IsAuthenticated)
            {
                string userName = user.Identity.Name;
                WoasFormsAppUser appUser = await _users.FindByNameAsync(userName);
                Console.WriteLine($"Found user by id {appUser.UserName}");
                res.Concat(_ctx.Templates.Where(t => !t.Public && t.AllowedUsers.Contains(appUser)));
            }

            return res;
        }

        public async Task UpdateTemplate(int templateId, Template newTemplate)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteTemplate(int templateId)
        {
            throw new NotImplementedException();
        }

        public async Task LikeTemplate(int templateId)
        {
            throw new NotImplementedException();
        }

        public async Task UnLikeTemplate(int templateId)
        {
            throw new NotImplementedException();
        }

        public async Task CommentOnTemplate(int templateId, string commentText)
        {
            throw new NotImplementedException();
        }
    }
}
