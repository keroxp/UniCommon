using System;
using System.Collections.Generic;

namespace UniCommon {
    public static class Enumerators {
        public static List<T> ToList<T>(this IEnumerator<T> self) {
            var ret = new List<T>();
            var it = self;
            while (it.MoveNext()) ret.Add(it.Current);
            return ret;
        }

        public static T Next<T>(this IEnumerator<T> self) {
            if (self.MoveNext()) return self.Current;
            throw new Exception("Enumerator could not move next.");
        }

        public static void ForEach<T>(this IEnumerator<T> self, Action<T> func) {
            while (self.MoveNext()) func(self.Current);
        }

        public static IEnumerator<T> Cycle<T>(IList<T> list) {
            return new CycleEnumerator<T>(list);
        }

        public static IEnumerator<T> PickEveryAtRandom<T>(IList<T> src) {
            return new PickEveryAtRandomEnumerator<T>(src);
        }
    }
}