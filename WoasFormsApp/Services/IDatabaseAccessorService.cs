using WoasFormsApp.Data;

namespace WoasFormsApp.Services
{
    public interface IDatabaseAccessorService
    {
        Task CreateTemplate(Template template);
        Task<List<Template>?> GetAvailableTemplates();
        Task UpdateTemplate(int templateId, Template newTemplate);
        Task DeleteTemplate(int templateId);
        Task LikeTemplate(int templateId);
        Task UnLikeTemplate(int templateId);
        Task CommentOnTemplate(int templateId, string commentText);

        Task<List<WoasFormsAppUser>> GetAllUsers();
        Task DeleteUser(string userId);
        Task GiveUserRole(string userId, string roleName);
        Task RevokeUserRole(string userId, string roleName);
        Task BlockUser(string userId);
        Task UnblockUser(string userId);
    }
}
