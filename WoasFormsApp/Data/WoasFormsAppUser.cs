using Microsoft.AspNetCore.Identity;

namespace WoasFormsApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class WoasFormsAppUser : IdentityUser
    {
        public DateTime? RegisteredAt { get; set; }
        public bool IsBlocked { get; set; } = false;

        public ICollection<Response> Responses { get; set; }

        public ICollection<Template> OwnedTemplates { get; set; }
    }
}