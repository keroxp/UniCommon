using System;
using System.Collections;
using UnityEngine;

namespace UniCommon {
    public abstract class AManagedBehaviourInternal : MonoBehaviour {
        protected abstract void Awake();
        protected abstract void Start();
        protected abstract void Update();
        protected abstract void LateUpdate();
        protected abstract void OnDestroy();
    }

    public abstract class AManagedBehaviour : AManagedBehaviourInternal {
        private Vector3 prevpos;

        public Vector3 deltaPosition {
            get { return transform.position - prevpos; }
        }

        public float actualElapsedTime { get; private set; }
        public float elapsedTime { get; private set; }
        private string _localTimeKey = Times.DefaultKey;

        public string localTimeKey {
            get { return _localTimeKey; }
            set { _localTimeKey = value; }
        }

        protected float localDeltaTime {
            get { return Times.GetLocalDeltaTime(localTimeKey); }
        }

        protected float localSmoothDeltaTime {
            get { return Times.GetLocalSmoothDeltaTime(localTimeKey); }
        }

        protected sealed override void Awake() {
#if !DEVELOPMENT_BUILD && !DEBUG
            if (DevelopmentOnly()) gameObject.SetActive(false);
#endif
        }

        protected virtual void OnAwake() {
        }

        private bool _IsReady() {
            return IsReady();
        }

        protected virtual bool IsReady() {
            return true;
        }

        protected virtual bool ShouldDeactivate() {
            return false;
        }

        protected sealed override void Start() {
            if (!_IsReady()) return;
            _OnStart();
        }

        private void _OnStart() {
            OnStart();
            _started = true;
        }

        private bool _started;

        protected virtual void OnStart() {
        }

        protected sealed override void Update() {
            actualElapsedTime += Times.deltaTime;
            elapsedTime += Times.GetLocalDeltaTime(localTimeKey);
            gameObject.SetActive(!ShouldDeactivate());
            if (!_IsReady()) return;
            if (!_started) {
                _OnStart();
            }
            OnUpdate();
        }

        protected virtual void OnUpdate() {
        }

        protected sealed override void LateUpdate() {
            if (!_IsReady()) return;
            OnLateUpdate();
            prevpos = transform.position;
        }

        protected virtual void OnLateUpdate() {
        }

        protected sealed override void OnDestroy() {
            PreDestroy();
        }

        protected virtual void PreDestroy() {
        }

        protected virtual bool DevelopmentOnly() {
            return false;
        }
    }
}