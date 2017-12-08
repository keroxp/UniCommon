using System;

namespace UniCommon {
    public class DisposableLock : IDisposable {
        public void Use(Action action) {
            lock (this) {
                using (this) {
                    action();
                }
            }
        }

        public void Dispose() {
            Locks.Release(this);
        }
    }
}