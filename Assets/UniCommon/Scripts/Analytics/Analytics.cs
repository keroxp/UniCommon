using System.Collections.Generic;
using UniRx;
using UnityAnalytics = UnityEngine.Analytics;

namespace UniCommon {
    public interface IUnityAnalyticsExport {
        void Export(ref IDictionary<string, object> dict, PrefixResolver prefix);
    }

    public class PrefixResolver {
        private readonly string _prefix = "";
        public static PrefixResolver Default = new PrefixResolver();

        public PrefixResolver(string prefix = "") {
            _prefix = prefix;
        }

        public void Set(ref IDictionary<string, object> dict, string key, object value) {
            dict[Resolve(key)] = value;
        }

        public string Resolve(string id) {
            if (string.IsNullOrEmpty(_prefix)) return id;
            return string.Format("{0}.{1}", _prefix, id);
        }

        public PrefixResolver Append(string id) {
            return new PrefixResolver(Resolve(id));
        }
    }

    public class Analytics {
        private static readonly IDictionary<string, object> _dictBuffer = new Dictionary<string, object>();
        private static bool _initialized;

        public static void Initialize() {
            if (_initialized) return;
            EventStream.All.Where(ev => ev is IAnalyticsEventData).Select(ev => ev as IAnalyticsEventData)
                .Subscribe(ev => CustomEvent(ev.GetType().Name, ev));
            _initialized = true;
        }

        private static IDictionary<string, object> GetDictionary() {
            _dictBuffer.Clear();
            return _dictBuffer;
        }

        private static void CustomEvent(string eventName, IUnityAnalyticsExport data = null) {
            Debugs.Log("Analytics/CustomEvent", eventName);
            if (data != null) {
                var dict = GetDictionary();
                data.Export(ref dict, PrefixResolver.Default);
                UnityAnalytics.Analytics.CustomEvent(eventName, dict);
                EventCreator.AnalyticsEvent(eventName, dict);
            } else {
                UnityAnalytics.Analytics.CustomEvent(eventName);
                EventCreator.AnalyticsEvent(eventName, null);
            }
        }
    }
}