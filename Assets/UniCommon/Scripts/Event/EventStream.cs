using System;
using System.Collections.Generic;
using UniRx;

namespace UniCommon {
    public sealed class EventStream {
        public static readonly EventStream Instance = new EventStream();
        private Dictionary<Type, object> _observables = new Dictionary<Type, object>();

        private static readonly Subject<IEventData> AllSubscriber = new Subject<IEventData>();

        public static IObservable<IEventData> All {
            get { return AllSubscriber; }
        }

        public static void OnAny(IEventData data) {
            AllSubscriber.OnNext(data);
        }

        public static IObservable<T> Get<T>() where T : IEventData {
            if (!Instance._observables.ContainsKey(typeof(T))) {
                var stream = new Subject<T>();
                stream.Select(ev => (IEventData) ev).Subscribe(OnAny);
                Instance._observables[typeof(T)] = stream;
            }
            return (IObservable<T>) Instance._observables[typeof(T)];
        }

        private EventStream() {
        }

        private Subject<T> Subject<T>() where T : IEventData {
            return (Subject<T>) Get<T>();
        }

        public static IDisposable Subscribe<T>(Action<T> onNext) where T : IEventData {
            return Get<T>().Subscribe(onNext);
        }

        internal static void Publish<T>(T ev) where T : IEventData {
            Instance.Subject<T>().OnNext(ev);
        }

        private static readonly Dictionary<Type, bool> OnceMap = new Dictionary<Type, bool>();

        internal static void PublishOnce<T>(T ev) where T : IEventData {
            lock (OnceMap) {
                if (OnceMap.ContainsKey(typeof(T))) return;
                OnceMap[typeof(T)] = true;
                Publish(ev);
            }
        }
    }
}