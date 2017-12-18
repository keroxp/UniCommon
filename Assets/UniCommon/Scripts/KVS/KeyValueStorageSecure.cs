using System;
using System.Text;

namespace UniCommon {
    public delegate ICrypter VersionedCrypterProvider(string version);

    internal sealed class KeyValueStorageSecure : ADelegatedKeyValueStorage {
        private readonly VersionedCrypterProvider _crypterProvider;
        private readonly string _version;

        public KeyValueStorageSecure(
            string prefix,
            string version,
            VersionedCrypterProvider crypterProvider,
            IKeyValueStorage kvs) : base(prefix, kvs) {
            _crypterProvider = crypterProvider;
            _version = version;
        }

        private ICrypter CurrentCrypter() {
            return _crypterProvider(_version);
        }

        protected override string Key(string key) {
            return "Secure." + base.Key(key);
        }

        private void _Upsert(string key, byte[] value) {
            var vsn = Versioned.TryWrap(_version, value);
            base.Upsert(key, Convert.ToBase64String(vsn));
        }

        public override void Upsert(string key, int value) {
            _Upsert(key, CurrentCrypter().TryEncrypt(BitConverter.GetBytes(value)));
        }

        public override void Upsert(string key, float value) {
            _Upsert(key, CurrentCrypter().TryEncrypt(BitConverter.GetBytes(value)));
        }

        public override void Upsert(string key, bool value) {
            Upsert(key, value ? 1 : 0);
        }

        public override void Upsert(string key, string value) {
            _Upsert(key, CurrentCrypter().TryEncrypt(Encoding.UTF8.GetBytes(value)));
        }

        private byte[] GetBytes(string key) {
            if (!HasKey(key)) return null;
            var base64 = base.GetString(key);
            var bytes = Convert.FromBase64String(base64);
            var vsn = Versioned.TryUnwrap(bytes);
            return _crypterProvider(vsn.Version).TryDecrypt(vsn.Data);
        }

        public override string GetString(string key, string defaultValue = "") {
            var s = GetBytes(key);
            return s == null ? defaultValue : Encoding.UTF8.GetString(s);
        }

        public override float GetFloat(string key, float defaultValue = 0) {
            var s = GetBytes(key);
            return s == null ? defaultValue : BitConverter.ToSingle(s, 0);
        }

        public override int GetInt(string key, int defaultValue = 0) {
            var s = GetBytes(key);
            return s == null ? defaultValue : BitConverter.ToInt32(s, 0);
        }

        public override bool GetBool(string key) {
            return GetInt(key) != 0;
        }
    }
}