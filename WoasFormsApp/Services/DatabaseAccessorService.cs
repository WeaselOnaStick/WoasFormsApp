﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using WoasFormsApp.Data;

namespace WoasFormsApp.Services
{
    public class DatabaseAccessorService : IDatabaseAccessorService
    {
        WoasFormsDbContext _ctx;
        AuthenticationStateProvider _asp;
        UserManager<WoasFormsAppUser> _users;
        SignInManager<WoasFormsAppUser> _sign;

        public DatabaseAccessorService(
            WoasFormsDbContext context,
            AuthenticationStateProvider authenticationStateProvider,
            UserManager<WoasFormsAppUser> userManager,
            SignInManager<WoasFormsAppUser> signInManager)
        {
            _ctx = context;
            _asp = authenticationStateProvider;
            _users = userManager;
            _sign = signInManager;
        }

        private WoasFormsAppUser? _curUserCache = null;
        private bool _curUserIsCached = false;

        private bool _curUserHasAdminCache;
        private bool _curUserHasAdminIsCached = false;

        public async Task<WoasFormsAppUser?> GetCurrentUser()
        {
            if (!_curUserIsCached)
            {
                var user = (await _asp.GetAuthenticationStateAsync()).User;
                string userName = user.Identity.Name;
                if (userName == null) return null;
                var resUser = await _users.FindByNameAsync(userName);
                _curUserCache = resUser;
                _curUserIsCached = true;
            }
            return _curUserCache;
        }

        private async Task<bool> CurrentUserHasAdmin()
        {
            if (!_curUserHasAdminIsCached)
            {
                var user = await GetCurrentUser();
                if (user == null) return false;
                _curUserHasAdminCache = await _users.IsInRoleAsync(user, "Admin");
                _curUserHasAdminIsCached = true;
            }
            return _curUserHasAdminCache;
        }

        private async Task<bool> CurrentUserHasPowerOverTarget(WoasFormsAppUser targetUser) 
            => (await CurrentUserHasAdmin()) || (await GetCurrentUser() == targetUser);

        private async Task<bool> CurrentUserHasPowerOverTarget(string userId) 
            => await CurrentUserHasPowerOverTarget(await GetUserById(userId));
        

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

        private static IQueryable<Template> GetTemplateDetails(IQueryable<Template> query)
        {
            return query
                .Include(t => t.Owner)
                .Include(t => t.Responses)
                .Include(t => t.Fields).ThenInclude(f => f.Type)
                .Include(t => t.Topic)
                .Include(t => t.Tags)
                .Include(t => t.Responses)
                .Include(t => t.UsersWhoLiked)
                .Include(t => t.Comments)
                .Include(t => t.AllowedUsers);
        }

        private async Task<IQueryable<Template>> TemplateAuthVisibility(IQueryable<Template> query)
        {
            var curUserHasAdmin = await CurrentUserHasAdmin();
            if (curUserHasAdmin) return query;

            var curUser = await GetCurrentUser();
            return query.Where(t => t.Public || t.Owner == curUser || t.AllowedUsers.Contains(curUser));
        }

        private async Task<bool> TemplateAuthManage(Template template)
        {
            var curUserHasAdmin = await CurrentUserHasAdmin();
            if (curUserHasAdmin) return true;

            var curUser = await GetCurrentUser();
            return template.Owner == curUser;
        }

        public record TemplateOrderModeData
        {
            required public string DisplayName { get; init; }
            required public Func<Template, object> Selector { get; init; }
            required public SortDirection Direction { get; init; }
        }

        public static Dictionary<TemplateOrderMode, TemplateOrderModeData> TemplateOrdersData = new Dictionary<TemplateOrderMode, TemplateOrderModeData>
        {
            {TemplateOrderMode.Oldest,              new TemplateOrderModeData{ DisplayName="Oldest",            Direction = SortDirection.Descending,   Selector = x => x.CreatedAt } },
            {TemplateOrderMode.Newest,              new TemplateOrderModeData{ DisplayName="Newest",            Direction = SortDirection.Ascending,    Selector = x => x.CreatedAt } },
            {TemplateOrderMode.MostLiked,           new TemplateOrderModeData{ DisplayName="Most Likes",        Direction = SortDirection.Descending,   Selector = x => x.UsersWhoLiked.Count } },
            {TemplateOrderMode.LeastLiked,          new TemplateOrderModeData{ DisplayName="Least Likes",       Direction = SortDirection.Ascending,    Selector = x => x.UsersWhoLiked.Count } },
            {TemplateOrderMode.MostCommented,       new TemplateOrderModeData{ DisplayName="Most Comments",     Direction = SortDirection.Descending,   Selector = x => x.Comments.Count } },
            {TemplateOrderMode.LeastCommented,      new TemplateOrderModeData{ DisplayName="Least Comments",    Direction = SortDirection.Ascending,    Selector = x => x.Comments.Count } },
            {TemplateOrderMode.MostResponded,       new TemplateOrderModeData{ DisplayName="Most Forms",        Direction = SortDirection.Descending,   Selector = x => x.Responses.Count } },
            {TemplateOrderMode.LeastResponded,      new TemplateOrderModeData{ DisplayName="Least Forms",       Direction = SortDirection.Ascending,    Selector = x => x.Responses.Count } },

        };

        public async Task<IList<Template>> SearchTemplates(TemplateOrderMode order = TemplateOrderMode.Newest, string? query = default, string? username = default, string? tag = default)
        {
            var res = _ctx.Templates.Where(t =>
                t.Title.Contains(query) ||
                t.Description.Contains(query) ||
                t.Fields.Select(f => f.Title).Contains(query) ||
                t.Fields.Select(f => f.Description).Contains(query));
            
            if (username != null)
                res = res.Where(t => t.Owner.UserName == username);

            if (tag != null)
                res = res.Where(t => t.Tags.Select(t => t.Title).Contains(tag));

            res = await TemplateAuthVisibility(res);
            var resList = await res.ToListAsync();
            var curOrderModeData = TemplateOrdersData[order];
            return resList.OrderByDirection(curOrderModeData.Direction, curOrderModeData.Selector).ToList();
        }

        public async Task<bool> GetCurrentUserOwnsTemplate(int templateId)
        {
            if (await CurrentUserHasAdmin()) return true;
            var curUser = await GetCurrentUser();
            return curUser == (await GetTemplate(templateId))?.Owner;
        }

        public async Task<Template?> GetTemplate(int templateId)
        {
            var res = _ctx.Templates.Where(t => t.Id == templateId);
            res = await TemplateAuthVisibility(res);
            res = GetTemplateDetails(res);
            return await res.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Template>> GetAvailableTemplates()
        {
            var res = _ctx.Templates.AsQueryable();
            res = await TemplateAuthVisibility(res);
            res = GetTemplateDetails(res);
            return res;
        }

        public async Task<IEnumerable<Template>> GetTemplatesByOwner(string userName)
        {
            var res = _ctx.Templates.Where(t => t.Owner.UserName == userName);
            res = await TemplateAuthVisibility(res);
            res = GetTemplateDetails(res);
            return res;
        }

        public async Task<IEnumerable<Template>> GetTemplatesByCurrentUser()
        {
            var curUser = await GetCurrentUser();
            var res = _ctx.Templates.Where(t => t.Owner == curUser);
            //res = await TemplateAuthVisibility(res);
            res = GetTemplateDetails(res);
            return res;
        }

        public async Task<Template?> UpdateTemplate(Template newTemplate, int? templateId = null)
        {
            var realTemplateId = templateId ?? newTemplate.Id;
            Template? template = await GetTemplate(realTemplateId);
            if (template == null) return null;
            if (!await TemplateAuthManage(template)) return null;
            // Complex function, comparing new fields to old+hidden, recycling data
            throw new NotImplementedException();
        }

        public async Task DeleteTemplate(int templateId)
        {
            Template? template = await GetTemplate(templateId);
            if (template == null) return;
            if (!await TemplateAuthManage(template)) return;
            _ctx.Templates.Remove(template);
            await _ctx.SaveChangesAsync();

            // Soft delete not stated in requirements
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
            var targetUser = await GetUserById(targetUserId);
            await _users.UpdateSecurityStampAsync(targetUser);
            if (await GetCurrentUser() == targetUser) await _sign.SignOutAsync();
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
            var targetUser = await GetUserById(userId);
            await _users.RemoveFromRoleAsync(targetUser, roleName);
            await _users.UpdateSecurityStampAsync(targetUser);
            if (roleName == "Admin" && await GetCurrentUser() == targetUser) await _sign.SignOutAsync();
            await _ctx.SaveChangesAsync();
        }

        public async Task BlockUser(string userId, bool blocked)
        {
            if (!await CurrentUserHasAdmin()) return;
            var targetUser = await GetUserById(userId);
            targetUser.IsBlocked = blocked;
            await _users.UpdateSecurityStampAsync(targetUser);
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

        private static IQueryable<Response> GetResponseDetails(IQueryable<Response> query)
        {
            return query
                 .Include(r => r.Respondent)
                 .Include(r => r.Template)
                    .ThenInclude(t => t.Owner)
                 .Include(r => r.Template)
                 .Include(r => r.Answers)
                    .ThenInclude(a => a.Field)
                        .ThenInclude(f => f.Type);
        }

        private async Task<IQueryable<Response>> ResponseAuthVisibility(IQueryable<Response> query)
        {
            var curUserHasAdmin = await CurrentUserHasAdmin();
            if (curUserHasAdmin) return query;

            var curUser = await GetCurrentUser();
            return query.Where(r => r.Respondent == curUser);
        }

        public async Task<bool> GetCurrentUserOwnsResponse(int responseId)
        {
            if (await CurrentUserHasAdmin()) return true;
            var curUser = await GetCurrentUser();
            return curUser == (await GetResponse(responseId))?.Respondent;
        }

        public async Task<IEnumerable<Response>> GetResponsesByTemplate(int templateId)
        {
            var res = _ctx.Responses.Where(r => r.Template.Id == templateId);
            res = await ResponseAuthVisibility(res);
            res = GetResponseDetails(res);
            return res;
        }

        public async Task<IEnumerable<Response>> GetResponsesByRespondent(string userId)
        {
            var res = _ctx.Responses.Where(r => r.Respondent.Id == userId);
            res = await ResponseAuthVisibility(res);
            res = GetResponseDetails(res);
            return res;
        }

        public async Task<IEnumerable<Response>> GetResponsesByCurrentUser()
        {
            var curUser = await GetCurrentUser();
            var res = _ctx.Responses.Where(r => r.Respondent.Id == curUser.Id);
            //res = await ResponseAuthVisibility(res);
            res = GetResponseDetails(res);
            return res;
        }

        public async Task<Response?> GetResponse(int responseId)
        {
            var res = _ctx.Responses.Where(r => r.Id == responseId);
            res = await ResponseAuthVisibility(res);
            res = GetResponseDetails(res);
            return await res.FirstAsync();
        }

        public async Task<IEnumerable<Response>> GetAllResponses()
        {
            var res = _ctx.Responses.AsQueryable();
            res = await ResponseAuthVisibility(res);
            res = GetResponseDetails(res);
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

        public async Task<WoasFormsAppUser?> GetUser(string userId) 
            => await GetUserById(userId);

        public async IAsyncEnumerable<WoasFormsAppUser> GetUsers(IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
            {
                var user = await GetUserById(userId);
                if (user != null) yield return user;
            }
        }

    }
}
