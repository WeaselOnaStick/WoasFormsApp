using Blazored.LocalStorage;

namespace WoasFormsApp.Services
{
    public interface IUserPrefService<T>
    {
        T? CachedVar { get; }
        public bool Initialized { get; }
        Task InitializeAsync();
        Task SetCachedValueAsync(T value);
        Task<T> RefreshValueAsync();
    }

    public class UserPrefService<T> : IUserPrefService<T>
    {
        public T? CachedVar { get; private set; }
        private bool _initialized;
        public bool Initialized 
        {
            get
            {
                _initialized |= (CachedVar != null);
                return _initialized;
            }
            private set => _initialized = value;
        }
        private string StorageKey;
        private T DefaultValue;

        private ILocalStorageService _storage;

        public UserPrefService(ILocalStorageService storage, string storageKey, T? defaultValue = default)
        {
            DefaultValue = defaultValue;
            _storage = storage;
            StorageKey = storageKey;
            Initialized = false;
        }

        public async Task InitializeAsync()
        {
            if (Initialized) return;
            var storedVal = await _storage.GetItemAsync<T?>(StorageKey);
            if (storedVal == null)
            {
                storedVal = DefaultValue;
                await _storage.SetItemAsync(StorageKey, DefaultValue);
            }
            CachedVar = (T)DefaultValue;
            Initialized = true;
        }

        public async Task SetCachedValueAsync(T value)
        {
            CachedVar = value;
            await _storage.SetItemAsync(StorageKey, value);
            Initialized = true;
        }

        public async Task<T> RefreshValueAsync()
        {
            var storedVal = await _storage.GetItemAsync<T?>(StorageKey);
            if (storedVal == null)
            {
                await InitializeAsync();
                return CachedVar!;
            }
            else
                return storedVal;
        }
    }

    public class ThemePrefsService : UserPrefService<bool>
    {
        public ThemePrefsService(ILocalStorageService storage, string storageKey = "DarkMode", bool defaultValue = true) : base(storage, storageKey, defaultValue) { }
    }

    public class LocalePrefsService : UserPrefService<string>
    {
        public LocalePrefsService(ILocalStorageService storage, string storageKey = "Locale", string? defaultValue = "en-US") : base(storage, storageKey, defaultValue) { }
    }
}
