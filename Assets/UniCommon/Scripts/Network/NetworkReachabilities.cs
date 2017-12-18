using System;
using UniRx;
using UnityEngine;

namespace UniCommon {
    internal static class NetworkReachabilities {
        private static NetworkReachability _reachability;

        public static void Initialize() {
            _reachability = Application.internetReachability;
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ => {
                    if (_reachability != Application.internetReachability)
                        EventCreator.NetworkReachabilityChanged(_reachability = Application.internetReachability);
                });
        }
    }
}