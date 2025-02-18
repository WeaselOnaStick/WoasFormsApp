using WoasFormsApp.Data;

namespace WoasFormsApp.Utils
{
    public static class WoasFormsUtils
    {
        private static string TemplateFallbackTitle = "Unnamed Template (ID {0})";

        public static string TemplateTitleOrUnnamed(Template template, string? fallback = null)
        {
            fallback ??= TemplateFallbackTitle;
            return string.IsNullOrWhiteSpace(template.Title) ? string.Format(fallback, template.Id) : $"\"{template.Title}\"";
        }
    }
}
