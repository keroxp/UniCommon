using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniCommon {
    public class ComposedLogHandler : ILogHandler {
        private readonly IList<ILogHandler> _handlers;

        public ComposedLogHandler(params ILogHandler[] handlers) {
            _handlers = handlers;
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args) {
            for (var i = 0; i < _handlers.Count; i++) _handlers[i].LogFormat(logType, context, format, args);
        }

        public void LogException(Exception exception, Object context) {
            for (var i = 0; i < _handlers.Count; i++) _handlers[i].LogException(exception, context);
        }
    }
}