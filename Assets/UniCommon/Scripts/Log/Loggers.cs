using System;
using System.Collections.Generic;
using System.IO;
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

        public static IStreamLogger<FileStream> NewFileLogger(string tag, string path, TimeSpan flushInterval,
            ILogFormatter formatter = null, bool writeNewLine = true, FileMode fileMode = FileMode.OpenOrCreate) {
            var fileLogger = new FileLogHandler(path, flushInterval, writeNewLine, fileMode);
            return new StreamLogger<FileStream>(tag, formatter ?? LogFormatters.Default, fileLogger);
        }
    }
}