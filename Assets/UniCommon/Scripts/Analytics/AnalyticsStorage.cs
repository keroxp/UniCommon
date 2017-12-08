using System;

namespace UniCommon {
    public interface IAnalyticsStorage {
    }

    public sealed class AnalyticsStorage : IAnalyticsStorage {
        private static readonly IKeyValueStorage Storage = KeyValueStorage.Prefixed("Analytics");
        private static readonly string Key_LaunchCount = "LanuchCount";
        private static readonly string Key_LastLaunchedAt = "LastLaunchedAt";

        private AnalyticsStorage() {
        }

        private static bool _initialized;

        public static void Initialize() {
            if (_initialized) return;
            EventStream.Subscribe<AnalyticsData.AppLaunched>(OnAppLaunch);
            _initialized = true;
        }

        private static void OnAppLaunch(AnalyticsData.AppLaunched ev) {
            Storage.Upsert(Key_LaunchCount, ev.count);
            Storage.Upsert(Key_LastLaunchedAt, ev.date);
        }


        public static void DeleteAllData() {
            Storage.Delete(Key_LaunchCount);
            Storage.Delete(Key_LastLaunchedAt);
        }

        public static bool IsFirstLaunch() {
            return LaunchCount() == 0;
        }

        public static int LaunchCount() {
            return Storage.GetInt(Key_LaunchCount);
        }

        public static DateTimeOffset LastLaunchedTime() {
            if (!Storage.HasKey(Key_LastLaunchedAt)) return DateTime.Now;
            var str = Storage.GetString(Key_LastLaunchedAt);
            return DateTimeOffset.Parse(str);
        }
    }
}