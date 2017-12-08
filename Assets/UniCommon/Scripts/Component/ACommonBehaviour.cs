using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniCommon {
    public abstract class ACommonBehaviour : AManagedBehaviour {
        protected ILogger Logger {
            get { return Loggers.Get(GetType().Name); }
        }

        public float posX {
            get { return transform.position.x; }
            set { transform.position = new Vector3(value, posY, posZ); }
        }

        public float posY {
            get { return transform.position.y; }
            set { transform.position = new Vector3(posX, value, posZ); }
        }

        public float posZ {
            get { return transform.position.z; }
            set { transform.position = new Vector3(posX, posY, value); }
        }

        public float lscaleX {
            get { return transform.localScale.x; }
            set { transform.localScale = new Vector3(value, lscaleY, lscaleZ); }
        }

        public float lscaleY {
            get { return transform.localScale.y; }
            set { transform.localScale = new Vector3(lscaleX, value, lscaleZ); }
        }

        public float lscaleZ {
            get { return transform.localScale.z; }
            set { transform.localScale = new Vector3(lscaleX, lscaleY, value); }
        }

        public float rotX {
            get { return transform.rotation.eulerAngles.x; }
            set { transform.rotation = Quaternion.Euler(value, rotY, rotZ); }
        }

        public float rotY {
            get { return transform.rotation.eulerAngles.y; }
            set { transform.rotation = Quaternion.Euler(rotX, value, rotZ); }
        }

        public float rotZ {
            get { return transform.rotation.eulerAngles.z; }
            set { transform.rotation = Quaternion.Euler(rotX, rotY, value); }
        }

        private readonly Dictionary<int, int> _coroutineIds = new Dictionary<int, int>();
        private int _coroutineId;

        public IEnumerator WaitForAllCoroutines(params IEnumerator[] coroutines) {
            yield return WaitForCoroutines(coroutines.Length, coroutines);
        }

        public IEnumerator WaitForAnyCoroutines(params Coroutine[] coroutines) {
            if (coroutines.Length == 0) yield break;
            var id = _coroutineId++;
            _coroutineIds[id] = 1;
            var list = coroutines.Select(c => ExecCoroutine(c, id)).ToList();
            while (0 < _coroutineIds[id]) {
                yield return 0;
            }
            list.ForEach(StopCoroutine);
            _coroutineIds.Remove(id);
        }

        private IEnumerator ExecCoroutine(Coroutine co, int id) {
            yield return co;
            _coroutineIds[id] -= 1;
        }

        public IEnumerator WaitForCoroutines(int count, params IEnumerator[] coroutines) {
            if (coroutines.Length == 0) yield break;
            var id = _coroutineId++;
            _coroutineIds[id] = count;
            var list = coroutines.Select(c => StartCoroutine(ExecCoroutine(c, id))).ToList();
            while (0 < _coroutineIds[id]) {
                yield return 0;
            }
            list.ForEach(StopCoroutine);
            _coroutineIds.Remove(id);
        }

        private IEnumerator ExecCoroutine(IEnumerator instruction, int id) {
            yield return instruction;
            _coroutineIds[id] -= 1;
        }


        protected Coroutine DoEndOfFrame(Action action) {
            return StartCoroutine(CoDoEndOfFrame(action));
        }

        private IEnumerator CoDoEndOfFrame(Action action) {
            yield return new WaitForEndOfFrame();
            action();
        }

        public Coroutine DoAfter(float delay, Action action) {
            return StartCoroutine(CoDoAfter(delay, action));
        }

        private IEnumerator CoDoAfter(float delay, Action action) {
            yield return Yields.WaitForSeconds(delay);
            action();
        }
    }
}