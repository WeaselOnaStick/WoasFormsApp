using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace WoasFormsApp.Controllers
{
    public class CultureController : Controller
    {
        [Route("[controller]/[action]")]
        public IActionResult Set(string culture, string redirectURI)
        {
            if (culture != null)
            {
                HttpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(
                        new RequestCulture(culture, culture)
                        )
                    );
            }
            return LocalRedirect(redirectURI);
        }
    }
}
