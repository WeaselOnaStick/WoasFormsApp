using System.Globalization;

namespace WoasFormsApp.Utils
{
    public static class Localization
    {
        public record SupportedLocale
        {
            required public CultureInfo CultureInfo;
            required public string FlagUrl;
        }

        public static HashSet<SupportedLocale> SupportedLocales = new()
        {
            new SupportedLocale
            {
                CultureInfo = new CultureInfo("en-US"),
                FlagUrl = "https://upload.wikimedia.org/wikipedia/en/a/a4/Flag_of_the_United_States.svg"
            },
            new SupportedLocale
            {
                CultureInfo = new CultureInfo("ru-RU"),
                FlagUrl = "https://upload.wikimedia.org/wikipedia/en/f/f3/Flag_of_Russia.svg"
            }
        };
    }
}
