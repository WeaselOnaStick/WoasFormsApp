using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using WoasFormsApp.Data;

namespace WoasFormsApp.Services
{
    public class DatabaseAccessorService : IDatabaseAccessorService
    {
        WoasFormsDbContext _ctx;
        AuthenticationStateProvider _asp;

        public DatabaseAccessorService(WoasFormsDbContext context, AuthenticationStateProvider authenticationStateProvider)
        {
            _ctx = context;
            _asp = authenticationStateProvider;
        }


        public Task CreateTemplate(Template template)
        {
            throw new NotImplementedException();
        }

        public Task<List<Template>> GetAvailableTemplates()
        {
            throw new NotImplementedException();
        }

        public Task UpdateTemplate(int templateId, Template newTemplate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTemplate(int templateId)
        {
            throw new NotImplementedException();
        }

        public Task LikeTemplate(int templateId)
        {
            throw new NotImplementedException();
        }

        public Task UnLikeTemplate(int templateId)
        {
            throw new NotImplementedException();
        }

        public Task CommentOnTemplate(int templateId, string commentText)
        {
            throw new NotImplementedException();
        }
    }
}
