using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniCommon {
    internal class TaggedLogger : ILogger {
        public string Tag { get; private set; }
        private readonly ILogFormatter _formatter;

        public TaggedLogger(string tag, ILogFormatter formatter, ILogHandler handler) {
            Tag = tag;
            _formatter = formatter;
            logHandler = handler;
            filterLogType = LogType.Log;
        }

        private const string FormatStr = "[{0}]: {1}";

        private string Format(LogType type, object message) {
            return Format(type, Tag, message);
        }

        private string WrapTag(string tag) {
            return string.Format("{0}/{1}", Tag, tag);
        }

        private string Format(LogType type, string tag, object message) {
            return _formatter.Format(type, string.Format(FormatStr, tag, message));
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
            logHandler.LogFormat(logType, context, format, args);
        }

        public void LogException(Exception exception, Object context) {
            logHandler.LogException(exception, context);
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

        public ILogHandler logHandler { get; set; }
        public bool logEnabled { get; set; }
        public LogType filterLogType { get; set; }
    }
}