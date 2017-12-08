using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniCommon {
    public static class Lists {
        public static IList<T> Of<T>(params T[] objs) {
            return objs;
        }

        public static List<T> Initialized<T>(int capacity, Func<int, T> initializer) {
            var ret = new List<T>(capacity);
            for (var i = 0; i < capacity; i++) {
                ret[i] = initializer(i);
            }
            return ret;
        }

        public static T AnyValue<T>(this IEnumerable<T> self) {
            var enumerable = self as IList<T> ?? self.ToList();
            var cnt = enumerable.Count;
            return cnt == 0 ? default(T) : enumerable[Random.Range(0, cnt)];
        }

        public static void Shuffle<T>(this IList<T> list) {
            for (var i = list.Count - 1; i > 0; i--) {
                var j = (int) Mathf.Floor(Random.value * (i + 1));
                var tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
        }

        public static string ToConcatenatedString<T>(this IEnumerable<T> list, int maxLen = 100) {
            var ret = new StringBuilder(maxLen);
            ret.Append("[");
            foreach (var i in list) {
                ret.Append(i);
                ret.Append(",");
            }
            ret.Append("]");
            return ret.ToString();
        }
    }
}