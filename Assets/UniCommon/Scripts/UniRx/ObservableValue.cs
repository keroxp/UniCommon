using System;
using UniRx;

namespace UniCommon {
    public interface IObservableValue<T> : IObservable<T> {
        T Value { get; }
    }

    public class ObservableValue<T> : IObservableValue<T>, ISubject<T> {
        protected Subject<T> _subject = new Subject<T>();

        public ObservableValue(T value = default(T)) {
            _value = value;
        }

        private T _value;

        public T Value {
            get { return _value; }
            set { OnNext(_value = value); }
        }

        public void OnCompleted() {
            _subject.OnCompleted();
        }

        public void OnError(Exception error) {
            _subject.OnError(error);
        }

        public void OnNext(T value) {
            _subject.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<T> observer) {
            return _subject.Subscribe(observer);
        }
    }
}