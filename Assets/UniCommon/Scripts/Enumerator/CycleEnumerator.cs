using System.Collections;
using System.Collections.Generic;

namespace UniCommon {
    class CycleEnumerator<T> : IEnumerator<T> {
        private readonly IList<T> _list;
        private int _index;

        public CycleEnumerator(IList<T> list) {
            _list = list;
        }

        public void Dispose() {
        }

        public bool MoveNext() {
            if (++_index >= _list.Count) {
                _index = 0;
            }
            return true;
        }

        public void Reset() {
            _index = 0;
        }

        public T Current {
            get { return _list[_index]; }
        }

        object IEnumerator.Current {
            get { return Current; }
        }
    }
}