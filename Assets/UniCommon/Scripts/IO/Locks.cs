using System.Collections.Generic;

namespace UniCommon {
    public static class Locks {
        private static readonly Dictionary<object, DisposableLock> locks = new Dictionary<object, DisposableLock>();

        public static DisposableLock Get(object key) {
            lock (locks) {
                if (locks.ContainsKey(key)) return locks[key];
                return locks[key] = new DisposableLock();
            }
        }

        public static void Release(object key) {
            lock (locks) {
                locks.Remove(key);
            }
        }
    }
}