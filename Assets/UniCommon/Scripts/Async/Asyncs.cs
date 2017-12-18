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
    }
}