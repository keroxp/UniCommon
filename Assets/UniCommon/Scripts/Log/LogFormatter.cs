using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniRx;
using UnityEngine;

namespace UniCommon {
    public interface ILogFormatter {
        string Format(LogType logType, object log);
        string FormatException(Exception exception);
    }

    public static class LogFormatters {
        public static readonly Regex DateFormatRegex =
            new Regex(
                "\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}\\+\\d{2}:\\d{2} \\d{3}ms/\\d+?F <[AEWeL]> \\[.+?\\]: .*?$");

        public static readonly string DateFormat = "yyyy-MM-ddThh:mm:sszzz";
        public static readonly ILogFormatter Default = new DefaultFormatter();

        public static string Format(string log) {
            return string.Format("{0} {1}", DateTime.Now.ToString(DateFormat), log);
        }
    }

    internal class DefaultFormatter : ILogFormatter {
        private BehaviorSubject<int> frameCountSubject;
        private IDisposable subscription;
        private int frameCount;

        public DefaultFormatter() {
            frameCountSubject = new BehaviorSubject<int>(0);
            subscription = Observable.EveryUpdate()
                .ObserveOnMainThread()
                .Select(_ => Time.frameCount)
                .Subscribe(frameCountSubject.OnNext);
        }

        private static readonly Dictionary<LogType, string> LogTypeId = new Dictionary<LogType, string> {
            {LogType.Assert, "A"},
            {LogType.Error, "E"},
            {LogType.Warning, "W"},
            {LogType.Exception, "e"},
            {LogType.Log, "L"}
        };

        public string Format(LogType logType, object log) {
            var now = DateTime.Now;
            var date = now.ToString(LogFormatters.DateFormat);
            var msAndFrame = string.Format("{0:fff}ms/{1}F", now, frameCountSubject.Value);
            // 2017-06-01 20:59:53+09:00 230ms/433F <L> [Tag]: Message       
            return string.Format("{0} {1} <{2}> {3}", date, msAndFrame, LogTypeId[logType], log);
        }

        public string FormatException(Exception exception) {
            return Format(LogType.Exception, exception.Message);
        }
    }
}