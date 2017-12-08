using System;
using UniRx;

namespace UniCommon {
    public interface IObservableWrap<T> : IObservable<T> {
        void OnNextIfNeeded();
    }

    public class ObservableWraps {
        public static IObservableWrap<T> New<T>(Func<T> everyFrameProvider) {
            return new ObservableWrapImpl<T>(everyFrameProvider);
        }

        private class ObservableWrapImpl<T> : IObservableWrap<T> {
            private readonly Func<T> _provider;
            private readonly Subject<T> _subject = new Subject<T>();
            private T _prev;

            public ObservableWrapImpl(Func<T> provider) {
                _provider = provider;
                _prev = provider();
            }

            public IDisposable Subscribe(IObserver<T> observer) {
                return _subject.Subscribe(observer);
            }

            public void OnNextIfNeeded() {
                var next = _provider();
                if (!next.Equals(_prev)) {
                    _subject.OnNext(next);
                }
                _prev = next;
            }
        }
    }
}