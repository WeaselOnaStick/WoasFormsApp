﻿using WoasFormsApp.Data;

namespace WoasFormsApp.Services
{
    public interface IDatabaseAccessorService
    {
        Task<HashSet<TemplateTopic>> GetTopics();
        Task<HashSet<TemplateTag>> GetTags();
        Task<HashSet<TemplateFieldType>> GetTemplateFieldTypes();

        Task<Template?> GetTemplate(int templateId);
        Task<IEnumerable<Template>> GetAvailableTemplates();
        Task<IEnumerable<Template>> GetTemplatesByOwner(string userName);
        Task<IEnumerable<Template>> GetTemplatesByCurrentUser();
        Task<Template?> CreateTemplate(Template template);
        Task<Template?> UpdateTemplate(Template newTemplate, int? templateId = null);
        Task DeleteTemplate(int templateId);
        Task LikeTemplate(int templateId, bool like);
        Task CommentOnTemplate(int templateId, string commentText);

        Task<Response?> CreateResponse(Response response);
        Task<Response?> GetResponse(int responseId);
        Task<IEnumerable<Response>> GetResponsesByTemplate(int templateId);
        Task<IEnumerable<Response>> GetResponsesByRespondent(string userId);
        Task<IEnumerable<Response>> GetResponsesByCurrentUser();

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
