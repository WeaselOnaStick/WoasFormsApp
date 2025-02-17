using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
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

        public async Task<WoasFormsAppUser> GetCurrentUser()
        {
            var user = (await _asp.GetAuthenticationStateAsync()).User;
            string userName = user.Identity.Name;
            if (userName == null) return null;
            return await _users.FindByNameAsync(userName);
        }

        private async Task<bool> CurrentUserHasAdmin()
        {
            var user = await GetCurrentUser();
            if (user == null) return false;
            return await _users.IsInRoleAsync(user, "Admin");
        }

        private async Task<bool> CurrentUserHasPowerOverTarget(string userId) 
            => await CurrentUserHasPowerOverTarget(await GetUserById(userId));
        

        private async Task<bool> CurrentUserHasPowerOverTarget(WoasFormsAppUser targetUser) 
            => (await CurrentUserHasAdmin()) || (await GetCurrentUser() == targetUser);
        

        private async Task<WoasFormsAppUser> GetUserById(string userId) => await _users.FindByIdAsync(userId);
        private async Task<WoasFormsAppUser> GetUserByName(string userName) => await _users.FindByNameAsync(userName);

        public async Task<HashSet<TemplateTopic>> GetTopics() =>
            await _ctx.TemplateTopics.ToHashSetAsync();

        public async Task<HashSet<TemplateTag>> GetTags() =>
            await _ctx.TemplateTags.ToHashSetAsync();

        public async Task<HashSet<TemplateFieldType>> GetTemplateFieldTypes() =>
            await _ctx.FieldTypes.ToHashSetAsync();

        public bool ValidateTemplate(Template template)
        {
            if (string.IsNullOrWhiteSpace(template.Title)) return false;
            if (template.Fields == null || template.Fields.Count == 0) return false;
            return true;
        }

        public async Task<Template?> CreateTemplate(Template template)
        {
            template.Owner ??= await GetCurrentUser();

            // replacing submitted tags with existing ones if found, creating new if not found
            HashSet<TemplateTag> syncedTagList = new HashSet<TemplateTag>();
            HashSet<TemplateTag> newTags = new HashSet<TemplateTag>();
            foreach (var tagName in template.Tags.Select(t=>t.Title))
            {
                TemplateTag? foundTag = await _ctx.TemplateTags.FirstOrDefaultAsync(t => t.Title == tagName);
                if (foundTag != null)
                    syncedTagList.Add(foundTag);
                else
                    newTags.Add(new TemplateTag { Title = tagName });
            }

            if (newTags.Any())
            {
                await _ctx.TemplateTags.AddRangeAsync(newTags);
                await _ctx.SaveChangesAsync();
                syncedTagList.UnionWith(newTags);
            }

            template.Tags = syncedTagList;

            template.CreatedAt = DateTime.UtcNow;

            if (!ValidateTemplate(template)) return null;

            var freshTemplate = await _ctx.Templates.AddAsync(template);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return freshTemplate.Entity;
        }

        public async Task<Template?> GetTemplate(int templateId)
        {
            var res = await _ctx.Templates
                .Include(t => t.Owner)
                .Include(t => t.Responses)
                .Include(t => t.Fields).ThenInclude(f => f.Type)
                .Include(t => t.Topic)
                .Include(t => t.Tags)
                .Include(t => t.Responses)
                .Include(t => t.UsersWhoLiked)
                .Include(t => t.Comments)
                .Include(t => t.AllowedUsers)
                .FirstAsync(t => t.Id == templateId);
            if (res == null) return null;
            res.Fields.Sort((a,b) => a.Position.CompareTo(b.Position));
            if (await CurrentUserHasAdmin()) return res;
            if (res.Owner == null && await CurrentUserHasAdmin()) return res;
            if (res.Owner == null) return null;
            if (res.Public) return res;
            if (res.Owner == await GetCurrentUser()) return res;
            if (res.AllowedUsers.Contains(await GetCurrentUser())) return res;
            return null;
        }

        public async Task<List<Template>> GetAvailableTemplates()
        {
            var templateIds = await _ctx.Templates.Select(t => t.Id).ToListAsync();
            var res = new List<Template>();
            foreach (var id in templateIds)
            {
                var foundTemplate = await GetTemplate(id);
                if (foundTemplate != null) res.Add(foundTemplate);
            }     
            
            return res;
        }

        public async Task<List<Template>> GetUsersTemplates(string userName)
        {
            var curUser = await GetCurrentUser();
            var targetUser = await GetUserByName(userName);
            bool curUserHasFullAccess = (await CurrentUserHasAdmin()) || (curUser == targetUser);

            if (targetUser.OwnedTemplates == null) return new List<Template>();
            var res = targetUser.OwnedTemplates.ToList();
            if (!curUserHasFullAccess)
                res = res.Where(t => t.Public || t.AllowedUsers.Contains(curUser)).ToList();
            return res;
        }

        public async Task<Template?> UpdateTemplate(Template newTemplate, int? templateId = null)
        {
            if (templateId == null) templateId = newTemplate.Id;
            // Complex function, comparing new fields to old+hidden, recycling data
            throw new NotImplementedException();
        }

        public async Task DeleteTemplate(int templateId)
        {
            var template = await GetTemplate(templateId);
            if (template == null) return;
            _ctx.Remove(template);
            await _ctx.SaveChangesAsync();
            
            //Soft delete. Enabling would require migration (currently owner can't be null)
            //var owner = template.Owner;
            //if (owner == null) return;
            //template.Owner = null;
        }

        public async Task LikeTemplate(int templateId, bool liked)
        {
            var appUser = await GetCurrentUser();
            if (appUser == null) return;
            var template = await GetTemplate(templateId);
            if (template == null) return;
            var curUserAllowedToInteract = template.Public || await CurrentUserHasAdmin() || (!template.Public && template.AllowedUsers.Contains(appUser));
            if (!curUserAllowedToInteract) return;

            if (liked)
                template.UsersWhoLiked.Add(appUser);
            else
                template.UsersWhoLiked.Remove(appUser);

            await _ctx.SaveChangesAsync();
        }


        public async Task CommentOnTemplate(int templateId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText)) return;
            var appUser = await GetCurrentUser();
            if (appUser == null) return;
            var template = await GetTemplate(templateId);
            if (template == null) return;
            var curUserAllowedToInteract = template.Public || (!template.Public && template.AllowedUsers.Contains(appUser));
            if (!curUserAllowedToInteract) return;

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
            return await _users.Users
                .Include(u => u.OwnedTemplates)
                .ToListAsync();
        }

        public async Task DeleteUser(string targetUserId)
        {
            if (!await CurrentUserHasPowerOverTarget(targetUserId)) return;
            _ctx.Users.Remove(await GetUserById(targetUserId));
            await _ctx.SaveChangesAsync();
        }

        public async Task GiveUserRole(string userId, string roleName)
        {
            if (!await CurrentUserHasAdmin()) return;
            await _users.AddToRoleAsync(await GetUserById(userId), roleName);
            await _ctx.SaveChangesAsync();
        }

        public async Task RevokeUserRole(string userId, string roleName)
        {
            if (!await CurrentUserHasAdmin()) return;
            await _users.RemoveFromRoleAsync(await GetUserById(userId), roleName);
            await _ctx.SaveChangesAsync();
        }

        public async Task BlockUser(string userId, bool blocked)
        {
            if (!await CurrentUserHasAdmin()) return;
            (await GetUserById(userId)).IsBlocked = blocked;
            await _ctx.SaveChangesAsync();
        }

        public async Task BlockUser(string userId) => await BlockUser(userId, true);

        public async Task UnblockUser(string userId) => await BlockUser(userId, false);

        public async Task<List<string>> GetUserRoles(string userId)
        {
            List<string> res = new List<string>();
            if (!await CurrentUserHasAdmin()) return res;
            res = (await _users.GetRolesAsync(await GetUserById(userId))).ToList();
            return res;
        }

        public async Task<Response?> CreateResponse(Response response)
        {
            var curUser = await GetCurrentUser();
            response.Respondent = curUser;
            response.CreationDate = DateTime.UtcNow;
            // Response validation? Nothing to validate for now
            var freshResponse = await _ctx.Responses.AddAsync(response);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }
            return freshResponse.Entity;
        }
    }
}
