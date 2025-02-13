using Microsoft.AspNetCore.Identity;
using WoasFormsApp.Data;

namespace MudBlazorWebApp2.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<WoasFormsAppUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<WoasFormsAppUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
