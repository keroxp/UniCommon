using UnityEngine;

namespace UniCommon {
    public abstract class SingletonBehaviour<T> : AManagedBehaviour where T : SingletonBehaviour<T> {
        protected static T _instance;

        public static T Instance {
            get { return _instance ?? (_instance = FindObjectOfType<T>()); }
        }

        protected SingletonBehaviour() {
        }

        protected bool isOriginal;

        protected override void Awake() {
            if (Instance != this) {
                Destroy(gameObject);
                return;
            }
            base.Awake();
            isOriginal = true;
            _instance = (T) this;
            if (Application.isPlaying && !ShouldDestroyOnLoad())
                DontDestroyOnLoad(gameObject);
            PostAwake();
        }

        protected virtual void PostAwake() {
        }

        protected override void PreDestroy() {
            base.PreDestroy();
            if (Instance == this) _instance = null;
        }

        protected virtual bool ShouldDestroyOnLoad() {
            return false;
        }
    }
}