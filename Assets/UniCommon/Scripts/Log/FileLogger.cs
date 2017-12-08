using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniCommon {
    public class FileLogger : ILogHandler {
        public readonly string path;
        private readonly IStringFileWriter _writer;
        private bool _isDirty;
        private IDisposable _subscription;
        private ILogger _logger;

        public FileLogger(string path, TimeSpan flushInterval) {
            this.path = path;
            _writer = FileWriters.NewStringFileWriter(path);
            _subscription = Observable.Interval(flushInterval).Subscribe(Flush);
            _logger = Loggers.New(GetType().Name);
        }

        ~FileLogger() {
            _subscription.Dispose();
        }

        private void Flush(long i) {
            if (!_isDirty) return;
            Asyncs.Execute(FlushInternal);
        }

        private void FlushInternal() {
            lock (_writer) {
                _writer.Flush();
                _isDirty = false;
            }
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args) {
            try {
                lock (_writer) {
                    _writer.TryWrite(string.Format(format, args));
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
    }
}