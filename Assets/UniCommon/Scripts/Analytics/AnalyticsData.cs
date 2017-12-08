using System.Collections.Generic;

namespace UniCommon {
    public interface IAnalyticsEventData : IEventData, IUnityAnalyticsExport {
    }

    public class AnalyticsData {
        public struct AppLaunched : IAnalyticsEventData {
            public string os;
            public string device;
            public string language;
            public int count;
            public double intervalDays;
            public string date;
            public string version;

            public void Export(ref IDictionary<string, object> dict, PrefixResolver prefix) {
                dict[prefix.Resolve("os")] = os;
                dict[prefix.Resolve("device")] = device;
                dict[prefix.Resolve("count")] = count;
                dict[prefix.Resolve("interval_days")] = intervalDays;
                dict[prefix.Resolve("date")] = date;
                dict[prefix.Resolve("version")] = version;
            }
        }
    }
}