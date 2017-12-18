using System;
using System.IO;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniCommon {
    public abstract class StreamLogHandler<T> : ILogHandler, IDisposable where T : Stream {
        protected bool _isDirty;
        protected IDisposable _subscription;
        protected ILogger _logger;
        protected readonly T _stream;
        protected StreamWriter _writer;
        protected bool _writeNewLine;

        protected StreamLogHandler(T stream, TimeSpan flushInterval, bool writeNewLine = true) {
            _stream = stream;
            _subscription = Observable.Interval(flushInterval).Subscribe(_ => Flush());
            _logger = Loggers.New(GetType().Name);
            _writeNewLine = writeNewLine;
        }

        private bool _autoFlush = true;

        public bool AutoFlush {
            get { return _autoFlush; }
            set { _autoFlush = value; }
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args) {
            try {
                var writer = GetWriter(_stream);
                lock (writer) {
                    if (_writeNewLine) {
                        writer.WriteLine(format, args);
                    } else {
                        writer.Write(format, args);
                    }
                }
                _isDirty = true;
            } catch (Exception e) {
                _logger.LogError("LogFormat",
                    string.Format("an error occured while writing log into file. exception = {0}", e));
            }
        }

        public void LogException(Exception exception, Object context) {
            LogFormat(LogType.Exception, context, "{0}", exception.Message);
            LogFormat(LogType.Exception, context, "{0}", exception.StackTrace);
            _isDirty = true;
        }

        protected virtual StreamWriter GetWriter(Stream stream) {
            return _writer ?? (_writer = new StreamWriter(stream));
        }

        public void Flush(bool sync = false) {
            if (!_isDirty) return;
            if (sync) {
                FlushInternal();
            } else {
                Asyncs.Execute(FlushInternal);
            }
        }

        private void FlushInternal() {
            if (!_autoFlush) return;
            var writer = GetWriter(_stream);
            lock (writer) {
                writer.Flush();
                _isDirty = false;
            }
        }

        public void Dispose() {
            _subscription.Dispose();
            GetWriter(_stream).Dispose();
        }
    }
}