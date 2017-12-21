using System;
using UniRx;

namespace UniCommon {
    public static class Asyncs {
        public static UniRx.IObservable<bool> Execute(Action action) {
            return Execute(() => {
                action();
                return true;
            });
        }

        public static void Execute(Action action, Action callback, bool receiveOnMainThread = true) {
            var ob = Execute(action);
            if (receiveOnMainThread) {
                ob.SubscribeOnMainThread().Subscribe(_ => callback());
            } else {
                ob.Subscribe(_ => callback());
            }
        }

        public static UniRx.IObservable<T> Execute<T>(Func<T> task) {
            return Observable.Create<T>(observer => {
                    try {
                        observer.OnNext(task());
                    } catch (Exception e) {
                        observer.OnError(e);
                    } finally {
                        observer.OnCompleted();
                    }
                    return null;
                })
                .SubscribeOn(Scheduler.ThreadPool);
        }

        public static void Execute<T>(Func<T> task, Action<T> callback, bool receiveOnMainThread = true) {
            var ob = Execute(task);
            if (receiveOnMainThread) {
                ob.SubscribeOnMainThread().Subscribe(callback);
            } else {
                ob.Subscribe(callback);
            }
        }
    }
}