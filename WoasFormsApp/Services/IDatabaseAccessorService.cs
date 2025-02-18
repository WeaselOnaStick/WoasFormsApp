using WoasFormsApp.Data;

namespace WoasFormsApp.Services
{
    public interface IDatabaseAccessorService
    {
        Task<HashSet<TemplateTopic>> GetTopics();
        Task<HashSet<TemplateTag>> GetTags();
        Task<HashSet<TemplateFieldType>> GetTemplateFieldTypes();

        Task<bool> GetCurrentUserOwnsTemplate(int templateId);
        Task<Template?> GetTemplate(int templateId);
        Task<IEnumerable<Template>> GetAvailableTemplates();
        Task<IEnumerable<Template>> GetTemplatesByOwner(string userName);
        Task<IEnumerable<Template>> GetTemplatesByCurrentUser();
        Task<Template?> CreateTemplate(Template template);
        Task<Template?> UpdateTemplate(Template newTemplate, int? templateId = null);
        Task DeleteTemplate(int templateId);
        Task LikeTemplate(int templateId, bool like);
        Task CommentOnTemplate(int templateId, string commentText);

        Task<bool> GetCurrentUserOwnsResponse(int responseId);
        Task<Response?> CreateResponse(Response response);
        Task<Response?> GetResponse(int responseId);
        Task<IEnumerable<Response>> GetResponsesByTemplate(int templateId);
        Task<IEnumerable<Response>> GetResponsesByRespondent(string userId);
        Task<IEnumerable<Response>> GetResponsesByCurrentUser();

        Task<List<WoasFormsAppUser>> GetAllUsers();
        Task<WoasFormsAppUser?> GetCurrentUser();
        Task UserDelete(string userId);
        Task UserGiveRole(string userId, string roleName);
        Task UserRevokeRole(string userId, string roleName);
        Task UserBlock(string userId);
        Task UserUnblock(string userId);
        Task<List<string>> UserGetRoles(string userId);
    }
}
