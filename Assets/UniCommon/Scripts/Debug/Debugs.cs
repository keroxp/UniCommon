using System;
using System.Text;
using UnityEngine;

namespace UniCommon {
    public sealed class Debugs {
        private static readonly string separator = " , ";
        private static readonly ILogger DebugLogger = Loggers.New();

        public static string Concat(params object[] objs) {
            var _string = new StringBuilder();
            var i = 0;
            for (; i < objs.Length - 1; i++) {
                _string.Append(objs[i]);
                _string.Append(separator);
            }
            _string.Append(objs[i]);
            return _string.ToString();
        }

        public static void Log(params object[] msgs) {
            DebugLogger.Log(Concat(msgs));
        }

        public static void Log(Type type, object message) {
            Log(type.Name, message);
        }

        public static void Log(string tag, object message) {
            DebugLogger.Log(tag, message);
        }

        public static void Error(params object[] msgs) {
            DebugLogger.Log(LogType.Error, Concat(msgs));
        }

        public static void Error(Type type, object objs) {
            Error(type.Name, objs);
        }

        public static void Error(string tag, object objs) {
            DebugLogger.LogError(tag, Concat(objs));
        }

        public static void Exception(object objs) {
            DebugLogger.Log(LogType.Exception, Concat(objs));
        }

        public static void Exception(Exception exception) {
            DebugLogger.LogException(exception);
        }
    }
}