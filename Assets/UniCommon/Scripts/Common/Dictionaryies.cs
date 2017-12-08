using System.Collections.Generic;

namespace UniCommon {
    public static class DictionaryExt {
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key,
            TValue defaultValue = default(TValue)) {
            TValue value;
            return self.TryGetValue(key, out value) ? value : defaultValue;
        }
    }

    public static class Dictionaries {
        public static IDictionary<TKey, TValue> New<TKey, TValue>(TKey key, TValue value) {
            return new Dictionary<TKey, TValue> {{key, value}};
        }

        public static IDictionary<TKey, TValue> New<TKey, TValue>(TKey key, TValue value, TKey key2, TValue value2) {
            return new Dictionary<TKey, TValue> {{key, value}, {key2, value2}};
        }

        public static IDictionary<TKey, TValue> New<TKey, TValue>(TKey key, TValue value, TKey key2, TValue value2,
            TKey key3, TValue value3) {
            return new Dictionary<TKey, TValue> {{key, value}, {key2, value2}, {key3, value3}};
        }
    }
}