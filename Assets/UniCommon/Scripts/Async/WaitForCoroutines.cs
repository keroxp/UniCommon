using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniCommon {
    public class WaitForCoroutines : CustomYieldInstruction {
        public static CustomYieldInstruction All(params IEnumerator[] list) {
            return new WaitForCoroutines(list, list.Length);
        }

        public static CustomYieldInstruction Any(params IEnumerator[] list) {
            return new WaitForCoroutines(list, 1);
        }

        private IEnumerable<IEnumerator> _coroutines;
        private int _count;
        private int _done;

        private WaitForCoroutines(IEnumerator[] coroutines, int count) {
            _coroutines = coroutines;
            _count = count;
        }

        public override bool keepWaiting {
            get {
                foreach (var coroutine in _coroutines) {
                    if (!coroutine.MoveNext()) _done++;
                }
                return _done < _count;
            }
        }
    }
}