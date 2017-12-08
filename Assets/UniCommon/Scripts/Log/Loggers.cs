using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniCommon {
    public static class Loggers {
        private static readonly Dictionary<string, ILogger> LoggerMap = new Dictionary<string, ILogger>();

        public static ILogger Get(string tag) {
            if (LoggerMap.ContainsKey(tag)) return LoggerMap[tag];
            return LoggerMap[tag] = New(tag);
        }

        public static ILogger New(string tag = "") {
            return New(tag, LogFormatters.Default);
        }

        public static ILogger New(string tag, ILogFormatter formatter) {
            return New(tag, formatter, Debug.unityLogger);
        }

        public static ILogger New(string tag, ILogFormatter formatter, ILogHandler logger) {
            return new TaggedLogger(tag, formatter, logger);
        }

        public static ILogger AsFileLogger(this ILogger self, string path, TimeSpan flushInterval) {
            var fileLogger = new FileLogger(path, flushInterval);
            self.logHandler = new ComposedLogHandler(self.logHandler, fileLogger);
            return self;
        }
    }
}