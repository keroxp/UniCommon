using UnityEngine;

namespace UniCommon {
    internal class KeyValueStoragePlayerPrefs : AKeyValueStorage {
        private readonly string prefix;

        public KeyValueStoragePlayerPrefs(string prefix) : base(prefix) {
            this.prefix = prefix;
        }

        protected override string Key(string key) {
            return prefix + "." + key;
        }

        public sealed override void Upsert(string key, int value) {
            PlayerPrefs.SetInt(Key(key), value);
        }

        public sealed override void Upsert(string key, float value) {
            PlayerPrefs.SetFloat(Key(key), value);
        }

        public sealed override void Upsert(string key, bool value) {
            PlayerPrefs.SetInt(Key(key), value ? 1 : 0);
        }

        public sealed override void Upsert(string key, string value) {
            PlayerPrefs.SetString(Key(key), value);
        }

        public sealed override bool HasKey(string key) {
            return PlayerPrefs.HasKey(Key(key));
        }

        public sealed override string GetString(string key, string defaultValue = "") {
            return PlayerPrefs.GetString(Key(key), defaultValue);
        }

        public sealed override float GetFloat(string key, float defaultValue = 0) {
            return PlayerPrefs.GetFloat(Key(key), defaultValue);
        }

        public sealed override int GetInt(string key, int defaultValue = 0) {
            return PlayerPrefs.GetInt(Key(key), defaultValue);
        }

        public sealed override bool GetBool(string key) {
            return PlayerPrefs.GetInt(Key(key)) == 1;
        }

        public sealed override void Delete(string key) {
            PlayerPrefs.DeleteKey(Key(key));
        }
    }
}