using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UniCommon {
    public interface ILogFormatter {
        string Format(LogType logType, object log);
        string FormatException(Exception exception);
    }

    public static class LogFormatters {
        public static readonly Regex DateFormatRegex = new Regex("\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}.\\d{3}");
        public static readonly string DateFormat = "yyyy-MM-ddThh:mm:ss.fffzzz";
        public static readonly ILogFormatter Default = new DefaultFormatter();

        public static string Format(string log) {
            return string.Format("{0} {1}", DateTime.Now.ToString(DateFormat), log);
        }
    }

    internal class DefaultFormatter : ILogFormatter {
        private static readonly string FormatStr = "{0} <{1}> {2}";

        private static readonly Dictionary<LogType, string> LogTypeId = new Dictionary<LogType, string> {
            {LogType.Assert, "A"},
            {LogType.Error, "E"},
            {LogType.Warning, "W"},
            {LogType.Exception, "e"},
            {LogType.Log, "L"}
        };

        public string Format(LogType logType, object log) {
            // 2017-06-01 20:59:53.843517+0900 <L> [Tag]: Message
            return string.Format(FormatStr, DateTime.Now.ToString(LogFormatters.DateFormat), LogTypeId[logType], log);
        }

        public string FormatException(Exception exception) {
            return Format(LogType.Exception, exception.Message);
        }
    }
}