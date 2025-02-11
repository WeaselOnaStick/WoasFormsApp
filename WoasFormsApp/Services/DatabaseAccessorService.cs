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

        private async Task<WoasFormsAppUser> GetCurrentUserAsync()
        {
            var user = (await _asp.GetAuthenticationStateAsync()).User;
            string userName = user.Identity.Name;
            return await _users.FindByNameAsync(userName);
        }

        private async Task<WoasFormsAppUser> GetUserById(string userId) => await _users.FindByIdAsync(userId);

        private async Task<Template> GetTemplateById(int templateId) => await _ctx.Templates.FindAsync(templateId);

        public async Task CreateTemplate(Template template)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Template>> GetAvailableTemplates()
        {
            List<Template> res = await _ctx.Templates.Where(t => t.Public).ToListAsync();
            var appUser = await GetCurrentUserAsync();
            if (appUser != null)
            {
                Console.WriteLine($"Found user by id {appUser.UserName}. Adding allowed private templates.");
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
            var template = GetTemplateById(templateId);
            if (template == null) return;
            _ctx.Remove(template);
            await _ctx.SaveChangesAsync();
            
            //Soft delete. Enabling would require migration (currently owner can't be null)
            //var owner = template.Owner;
            //if (owner == null) return;
            //template.Owner = null;
        }

        private async Task ToggleTemplateLike(int templateId, bool liked)
        {
            var appUser = await GetCurrentUserAsync();
            if (appUser == null) return;
            var template = await GetTemplateById(templateId);
            if (template == null) return;
            
            if (liked)
                template.UsersWhoLiked.Add(appUser);
            else
                template.UsersWhoLiked.Remove(appUser);

            await _ctx.SaveChangesAsync();
        }

        public async Task LikeTemplate(int templateId)
        {
            ToggleTemplateLike(templateId, true);
        }

        public async Task UnLikeTemplate(int templateId)
        {
            ToggleTemplateLike(templateId, false);
        }

        public async Task CommentOnTemplate(int templateId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText)) return;
            var appUser = await GetCurrentUserAsync();
            if (appUser == null) return;
            var template = await GetTemplateById(templateId);
            if (template == null) return;

            template.Comments.Add(new TemplateComment
            {
                PostedAt = DateTime.UtcNow,
                User = appUser,
                Template = template,
                Text = commentText,
            });
            await _ctx.SaveChangesAsync();
        }

        public async Task<List<WoasFormsAppUser>> GetAllUsers()
        {
            return await _users.Users.ToListAsync();
        }

        public async Task DeleteUser(string userId)
        {
            _ctx.Users.Remove(await GetUserById(userId));
            await _ctx.SaveChangesAsync();
        }

        public async Task GiveUserRole(string userId, string roleName)
        {
            await _users.AddToRoleAsync(await GetUserById(userId), roleName);
            await _ctx.SaveChangesAsync();
        }

        public async Task RevokeUserRole(string userId, string roleName)
        {
            await _users.RemoveFromRoleAsync(await GetUserById(userId), roleName);
            await _ctx.SaveChangesAsync();
        }

        public async Task BlockUser(string userId, bool blocked)
        {
            (await GetUserById(userId)).IsBlocked = blocked;
            await _ctx.SaveChangesAsync();
        }

        public async Task BlockUser(string userId) => await BlockUser(userId, true);

        public async Task UnblockUser(string userId) => await BlockUser(userId, false);
    }
}
