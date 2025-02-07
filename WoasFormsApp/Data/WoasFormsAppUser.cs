using Microsoft.AspNetCore.Identity;

namespace WoasFormsApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class WoasFormsAppUser : IdentityUser
    {
        public bool IsBlocked { get; set; } = false;

        public ICollection<Template>? OwnedTemplates { get; set; }
    }

}
