using UnityEngine;

namespace UniCommon {
    internal abstract class AKeyValueStorage : IKeyValueStorage {
        private readonly string prefix;

        protected AKeyValueStorage(string prefix) {
            this.prefix = prefix;
        }

        protected virtual string Key(string key) {
            return prefix + "." + key;
        }

        public abstract void Upsert(string key, int value);
        public abstract void Upsert(string key, float value);
        public abstract void Upsert(string key, bool value);
        public abstract void Upsert(string key, string value);
        public abstract bool HasKey(string key);
        public abstract string GetString(string key, string defaultValue = "");
        public abstract float GetFloat(string key, float defaultValue = 0);
        public abstract int GetInt(string key, int defaultValue = 0);
        public abstract bool GetBool(string key);
        public abstract void Delete(string key);
    }

    internal abstract class ADelegatedKeyValueStorage : AKeyValueStorage {
        private readonly IKeyValueStorage delegator;

        protected ADelegatedKeyValueStorage(string prefix, IKeyValueStorage delegator) : base(prefix) {
            this.delegator = delegator;
        }

        protected override string Key(string key) {
            return base.Key(key);
        }

        public override void Upsert(string key, int value) {
            delegator.Upsert(key, value);
        }

        public override void Upsert(string key, float value) {
            delegator.Upsert(key, value);
        }

        public override void Upsert(string key, bool value) {
            delegator.Upsert(key, value);
        }

        public override void Upsert(string key, string value) {
            delegator.Upsert(key, value);
        }

        public override bool HasKey(string key) {
            return delegator.HasKey(key);
        }

        public override string GetString(string key, string defaultValue = "") {
            return delegator.GetString(key, defaultValue);
        }

        public override float GetFloat(string key, float defaultValue = 0) {
            return delegator.GetFloat(key, defaultValue);
        }

        public override int GetInt(string key, int defaultValue = 0) {
            return delegator.GetInt(key, defaultValue);
        }

        public override bool GetBool(string key) {
            return delegator.GetBool(key);
        }

        public override void Delete(string key) {
            delegator.Delete(key);
        }
    }
}