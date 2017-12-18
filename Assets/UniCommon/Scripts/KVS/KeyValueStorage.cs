namespace UniCommon {
    public interface IKeyValueStorage {
        void Upsert(string key, int value);
        void Upsert(string key, float value);
        void Upsert(string key, bool value);
        void Upsert(string key, string value);
        bool HasKey(string key);
        string GetString(string key, string defaultValue = "");
        float GetFloat(string key, float defaultValue = 0);
        int GetInt(string key, int defaultValue = 0);
        bool GetBool(string key);
        void Delete(string key);
    }

    public static class KeyValueStorage {
        public static readonly IKeyValueStorage DefaultStore = new KeyValueStoragePlayerPrefs("Default");

        public static IKeyValueStorage Prefixed(string prefix) {
            return new KeyValueStoragePlayerPrefs(prefix);
        }

        public static IKeyValueStorage Secure(
            string prefix,
            string version,
            VersionedCrypterProvider crypterProvider,
            IKeyValueStorage kvs = null) {
            if (kvs == null) kvs = Prefixed(prefix);
            return new KeyValueStorageSecure(prefix, version, crypterProvider, kvs);
        }
    }
}