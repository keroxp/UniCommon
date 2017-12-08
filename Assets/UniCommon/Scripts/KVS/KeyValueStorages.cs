using System;

namespace UniCommon {
    public static class KeyValueStorages {
        public static void Upsert(this IKeyValueStorage self, string key, DateTime date) {
            var v = date.ToBinary().ToString();
            self.Upsert(key, v);
        }

        public static DateTime GetDate(this IKeyValueStorage self, string key) {
            bool result;
            return GetDate(self, key, out result);
        }

        public static DateTime GetDate(this IKeyValueStorage self, string key, out bool ok) {
            var v = self.GetString(key);
            long bin;
            ok = long.TryParse(v, out bin);
            return DateTime.FromBinary(bin);
        }
    }
}