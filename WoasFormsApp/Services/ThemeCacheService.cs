using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace WoasFormsApp.Services
{
    public interface IThemeCacheService
    {
        bool CachedMode { get; }
        public bool Initialized { get; }
        Task InitializeAsync();
        Task SetCachedValueAsync(bool value);
    }

    public class ThemeCacheService : IThemeCacheService 
    {
        public bool CachedMode { get; private set; }
        public static bool DefaultMode = true;
        public string StorageKey { get; private set; } = "DarkMode";
        public bool Initialized { get; private set; }
        

        private ILocalStorageService _storage;

        public ThemeCacheService(ILocalStorageService storage)
        {
            _storage = storage;
            Initialized = false;
        }

        public async Task InitializeAsync()
        {
            if (Initialized) return;
            var storedBool = await _storage.GetItemAsync<bool?>(StorageKey);
            if (storedBool == null)
            {
                storedBool = DefaultMode;
                await _storage.SetItemAsync(StorageKey, DefaultMode);
            }
            CachedMode = (bool)storedBool;
            Initialized = true;
        }

        public async Task SetCachedValueAsync(bool value)
        {
            CachedMode = value;
            await _storage.SetItemAsync(StorageKey, value);
            Initialized = true;
        }
    }

}
