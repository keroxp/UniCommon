using System;
using UniRx;
using UnityEngine;

namespace UniCommon {
    public class UniCommon : AManagedBehaviour {
        public bool initializeOnAwake = true;
        private static bool initialized;

        public static void Initialize(bool force = false) {
            if (!force && initialized) return;
            InitInternetReachability();
            initialized = true;
        }

        protected override void OnAwake() {
            base.OnAwake();
            if (initializeOnAwake)
                Initialize();
        }

        private static NetworkReachability _reachability;

        private static void InitInternetReachability() {
            _reachability = Application.internetReachability;
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => {
                if (_reachability != Application.internetReachability)
                    EventCreator.NetworkReachabilityChanged(_reachability = Application.internetReachability);
            });
        }
    }
}