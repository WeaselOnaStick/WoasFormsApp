namespace WoasFormsApp.Services
{
    public class ThemeService
    {
        private bool _isDarkMode { get; set; } = true;

        public bool GetDarkMode()
        {
            return _isDarkMode;
        }

        public void SetDarkMode(bool darkMode)
        {
            _isDarkMode = darkMode;
        }
    }
}
