using System.Collections;
using System.Collections.Generic;

namespace UniCommon {
    internal class PickEveryAtRandomEnumerator<T> : IEnumerator<T> {
        private readonly IList<T> _list;
        private readonly List<int> indices = new List<int>();

        public PickEveryAtRandomEnumerator(IList<T> list) {
            _list = list;
            ShuffleIndices();
        }

        public void Dispose() {
        }

        private int index;

        private void ShuffleIndices() {
            indices.Clear();
            for (var i = 0; i < _list.Count; i++)
                indices.Add(i);
            indices.Shuffle();
        }

        public bool MoveNext() {
            if (++index >= _list.Count) {
                index = 0;
                ShuffleIndices();
            }
            return true;
        }

        public void Reset() {
            index = 0;
        }

        object IEnumerator.Current {
            get { return Current; }
        }

        public T Current {
            get {
                if (_list.Count == 0) return default(T);
                return _list[indices[index]];
            }
        }
    }
}