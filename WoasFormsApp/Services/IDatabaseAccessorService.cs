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
    }
}
