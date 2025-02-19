using WoasFormsApp.Data;
using static WoasFormsApp.Services.DatabaseAccessorService;

namespace WoasFormsApp.Services
{

    public enum TemplateOrderMode
    {
        Newest,
        Oldest,
        MostLiked,
        LeastLiked,
        MostCommented,
        LeastCommented,
        MostResponded,
        LeastResponded,
    }

    public interface IDatabaseAccessorService
    {
        Task<HashSet<TemplateTopic>> GetTopics();
        Task<HashSet<TemplateTag>> GetTags();
        Task<HashSet<TemplateFieldType>> GetTemplateFieldTypes();

        public static Dictionary<TemplateOrderMode, TemplateOrderModeData> TemplateOrdersData;
        Task<IList<Template>> SearchTemplates(TemplateOrderMode order = TemplateOrderMode.Newest, string? query = default, string? username = default, string? tag = default);
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

        Task<WoasFormsAppUser?> GetUser(string userId);
        Task<List<WoasFormsAppUser>> GetAllUsers();
        Task<WoasFormsAppUser?> GetCurrentUser();
        Task DeleteUser(string userId);
        Task GiveUserRole(string userId, string roleName);
        Task RevokeUserRole(string userId, string roleName);
        Task BlockUser(string userId);
        Task UnblockUser(string userId);
        Task<List<string>> GetUserRoles(string userId);
    }
}
