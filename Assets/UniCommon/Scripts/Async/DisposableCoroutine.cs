using System;
using System.Collections;

namespace UniCommon {
    public class DisposableCoroutine : IEnumerator, IDisposable {
        private bool _disposed;
        private IEnumerator _coroutine;

        public DisposableCoroutine(IEnumerator coroutine) {
            _coroutine = coroutine;
        }

        public void Dispose() {
            _disposed = true;
        }

        public bool MoveNext() {
            if (_disposed) return false;
            return _coroutine.MoveNext();
        }

        public void Reset() {
            _coroutine.Reset();
        }

        public object Current {
            get { return _coroutine.Current; }
        }
    }
}