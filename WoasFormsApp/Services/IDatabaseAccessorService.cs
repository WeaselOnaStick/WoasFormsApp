﻿using WoasFormsApp.Data;

namespace WoasFormsApp.Services
{
    public interface IDatabaseAccessorService
    {
        Task<HashSet<TemplateTopic>> GetTopics();
        Task<HashSet<TemplateTag>> GetTags();
        Task<HashSet<TemplateFieldType>> GetTemplateFieldTypes();

        Task<Template?> CreateTemplate(Template template);
        Task<Template?> GetTemplate(int templateId);
        Task<List<Template>> GetAvailableTemplates();
        Task<List<Template>> GetUsersTemplates(string userName);
        Task<Template?> UpdateTemplate(Template newTemplate, int? templateId = null);
        Task<Response?> CreateResponse(Response response);
        Task DeleteTemplate(int templateId);
        Task LikeTemplate(int templateId, bool like);
        Task CommentOnTemplate(int templateId, string commentText);

        Task<List<WoasFormsAppUser>> GetAllUsers();
        Task<WoasFormsAppUser> GetCurrentUser();
        Task DeleteUser(string userId);
        Task GiveUserRole(string userId, string roleName);
        Task RevokeUserRole(string userId, string roleName);
        Task BlockUser(string userId);
        Task UnblockUser(string userId);
        Task<List<string>> GetUserRoles(string userId);
    }
}
