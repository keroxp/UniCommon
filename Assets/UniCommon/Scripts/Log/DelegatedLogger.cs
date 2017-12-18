using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniCommon {
    public sealed class DelegatedLogger : ILogger {
        public delegate void LogFormatDelegate(LogType logType, Object context, string format, params object[] args);

        public delegate void LogExceptionDelegate(Exception e, Object context);

        private string Tag;
        private LogFormatDelegate _logFormatDelegate;
        private LogExceptionDelegate _logExceptionDelegate;
        private ILogFormatter _logFormatter;

        public DelegatedLogger(string tag, ILogFormatter formatter, LogFormatDelegate logFormatDelegate,
            LogExceptionDelegate logExceptionDelegate = null) {
            Tag = tag;
            _logFormatter = formatter;
            _logFormatDelegate = logFormatDelegate;
            _logExceptionDelegate = logExceptionDelegate ??
                                    ((e, ctx) => _logFormatDelegate(LogType.Exception, null, "{0}", e));
        }

        private const string FormatStr = "[{0}]: {1}";

        private string Format(LogType type, object message) {
            return Format(type, Tag, message);
        }

        private string WrapTag(string tag) {
            return string.Format("{0}/{1}", Tag, tag);
        }

        private string Format(LogType type, string tag, object message) {
            return _logFormatter.Format(type, string.Format(FormatStr, tag, message));
        }

        private void _Log(LogType logType, object message, Object context = null) {
            LogFormat(logType, context, "{0}", Format(logType, message));
        }

        private void _Log(LogType logType, string tag, object message, Object context = null) {
            LogFormat(logType, context, "{0}", (object) Format(logType, WrapTag(tag), message));
        }

        public void Log(LogType logType, object message) {
            _Log(logType, message);
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args) {
            if (!IsLogTypeAllowed(logType)) return;
            _logFormatDelegate(logType, context, format, args);
        }

        public void LogException(Exception exception, Object context) {
            _logExceptionDelegate(exception, context);
        }

        public void LogFormat(LogType logType, string format, params object[] args) {
            LogFormat(logType, null, format, args);
        }

        public void LogException(Exception exception) {
            LogException(exception, null);
        }

        public bool IsLogTypeAllowed(LogType logType) {
            // error / log
            return logType <= filterLogType;
        }

        public void Log(LogType logType, object message, Object context) {
            _Log(logType, message, context);
        }

        public void Log(LogType logType, string tag, object message) {
            _Log(logType, tag, message);
        }

        public void Log(LogType logType, string tag, object message, Object context) {
            _Log(logType, tag, message, context);
        }

        public void Log(object message) {
            _Log(LogType.Log, message);
        }

        public void Log(string tag, object message) {
            _Log(LogType.Log, tag, message);
        }

        public void Log(string tag, object message, Object context) {
            _Log(LogType.Log, tag, message, context);
        }

        public void LogWarning(string tag, object message) {
            _Log(LogType.Warning, tag, message);
        }

        public void LogWarning(string tag, object message, Object context) {
            _Log(LogType.Warning, tag, message, context);
        }

        public void LogError(string tag, object message) {
            _Log(LogType.Error, tag, message);
        }

        public void LogError(string tag, object message, Object context) {
            _Log(LogType.Error, tag, message, context);
        }

        public ILogHandler logHandler {
            get { return this; }
            set { throw new Exception("DelegatedLogger cannnot change logHandler. "); }
        }

        public bool logEnabled { get; set; }
        public LogType filterLogType { get; set; }
    }
}