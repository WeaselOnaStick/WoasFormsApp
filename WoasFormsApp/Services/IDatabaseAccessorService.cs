using WoasFormsApp.Data;

namespace WoasFormsApp.Services
{
    public interface IDatabaseAccessorService
    {
        Task<List<TemplateTopic>> GetTopics();
        Task<List<TemplateTag>> GetTags();
        Task<List<TemplateFieldType>> GetTemplateFieldTypes();

        Task<Template?> CreateTemplate(Template template);
        Task<Template?> GetTemplate(int templateId);
        Task<List<Template>> GetAvailableTemplates();
        Task<List<Template>> GetUsersTemplates(string userName);
        Task<Template?> UpdateTemplate(Template newTemplate, int? templateId = null);
        Task DeleteTemplate(int templateId);
        Task LikeTemplate(int templateId);
        Task UnLikeTemplate(int templateId);
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
