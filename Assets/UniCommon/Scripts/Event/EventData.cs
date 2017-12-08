using System.Collections.Generic;
using UnityEngine;

namespace UniCommon {
    public interface IEventData {
    }

    public struct PlayerTapped : IEventData {
        public Vector2 pos;
    }

    public struct PlayerFlicked : IEventData {
        public Vector2 pos;
        public Vector2 vector;
        public float time;
    }

    public struct LocalTimeScaleChanged : IEventData {
        public string key;
        public float prev;
        public float next;
    }

    public struct ReceivedLog : IEventData {
        public string log;
        public string stackTrace;
        public LogType type;
    }

    public struct AnalyticsEvent : IEventData {
        public string eventName;
        public IDictionary<string, object> data;
    }

    public struct NetworkReachabilityChanged : IEventData {
        public NetworkReachability reachability;
    }
}