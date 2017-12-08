using System;
using System.Threading;
using UnityEngine;

namespace UniCommon {
    public struct AsyncError {
        public Exception exception;
    }

    public class AsyncTask<T> {
        public Action onEntry = () => { };
        public Func<T> action;
        public Action<AsyncError> onError = e => { };
        public Action<T> onCompleted;
    }

    public class AsyncTask {
        public Action onEntry = () => { };
        public Action action;
        public Action<AsyncError> onError = e => { };
        public Action onCompleted;
    }

    public interface IAsyncExecutor {
        void Execute(AsyncTask task);
    }

    public static class Asyncs {
        private static bool _initialized;

        public static void Execute(Action action) {
            Execute(new AsyncTask {action = action});
        }

        public static void Execute(AsyncTask task) {
            Execute(new AsyncTask<object>() {
                onEntry = task.onEntry,
                onError = task.onError,
                onCompleted = o => task.onCompleted(),
                action = () => {
                    task.action();
                    return null;
                }
            });
        }

        public static void Execute<T>(AsyncTask<T> task) {
            if (!_initialized) {
                var cnum = SystemInfo.processorCount;
                int min = 1;
                int max = cnum;
                //ThreadPool.GetAvailableThreads(out workerCnt, out completionPortCnt);
                ThreadPool.SetMinThreads(min, cnum);
                ThreadPool.SetMaxThreads(cnum, cnum);
                Debugs.Log(string.Format("[Asyncs] ThreadPool initialized: CPUs: {0}, Min: {1}, Max: {2}", cnum, min,
                    max));
                _initialized = true;
            }
            ThreadPool.QueueUserWorkItem(o => {
                try {
                    task.onEntry();
                    var ret = task.action();
                    task.onCompleted(ret);
                } catch (Exception e) {
                    task.onError(new AsyncError() {exception = e});
                }
            });
        }
    }
}