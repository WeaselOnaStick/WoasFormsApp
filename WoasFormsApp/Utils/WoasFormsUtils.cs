using Microsoft.Extensions.Localization;
using WoasFormsApp.Data;

namespace WoasFormsApp.Utils
{
    public static class WoasFormsUtils
    {
        public static string TemplateTitleOrUnnamed(Template template, IStringLocalizer loc)
        {
            var fallback = loc["UNNAMED_TEMPLATE_ID", template.Id];
            return string.IsNullOrWhiteSpace(template.Title) ? fallback : $"\"{template.Title}\"";
        }
    }
}
