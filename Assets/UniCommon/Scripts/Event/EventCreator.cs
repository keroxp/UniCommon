using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniCommon {
    public static class EventCreator {
        public static void AppLaunched() {
            double interval = 0;
            var now = DateTimeOffset.Now;
            if (!AnalyticsStorage.IsFirstLaunch()) {
                var span = now.Subtract(AnalyticsStorage.LastLaunchedTime());
                interval = span.TotalDays;
            }
            EventStream.PublishOnce(new AnalyticsData.AppLaunched {
                os = SystemInfo.operatingSystem,
                device = SystemInfo.deviceModel,
                language = Application.systemLanguage.ToString(),
                date = now.ToString(),
                version = Application.version,
                count = AnalyticsStorage.LaunchCount() + 1,
                intervalDays = interval
            });
        }

        public static void LogReceived(string log, string stackTrace, LogType type) {
            EventStream.Publish(new ReceivedLog {log = log, stackTrace = stackTrace, type = type});
        }

        public static void PlayerTapped(Vector2 pos) {
            EventStream.Publish(new PlayerTapped {pos = pos});
        }

        public static void PlayerFlicked(Vector2 pos, Vector2 vec, float time) {
            EventStream.Publish(new PlayerFlicked {pos = pos, vector = vec, time = time});
        }

        public static void AnalyticsEvent(string eventName, IDictionary<string, object> data) {
            EventStream.Publish(new AnalyticsEvent {eventName = eventName, data = data});
        }

        public static void NetworkReachabilityChanged(NetworkReachability reachable) {
            EventStream.Publish(new NetworkReachabilityChanged() {reachability = reachable});
        }
    }
}