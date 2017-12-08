using System.Collections.Generic;
using UnityEngine;

namespace UniCommon {
    public class Times {
        private Times() {
        }

        public static readonly string DefaultKey = "Default";
        public static readonly string Unscaled = "Unscaled";
        public static readonly string UI = "UI";

        public static void SetLocalTimeScale(string key, float scale) {
//            EventStream.Publish(new LocalTimeScaleChanged()
//            {
//                key = key,
//                prev = GetLocalTimeScale(key),
//                next = TimeScales[key] = scale
//            });
        }

        public static float GetLocalTimeScale(string key) {
            if (TimeScales.ContainsKey(key)) return TimeScales[key];
            return TimeScales[key] = timeScale;
        }

        public static float GetLocalDeltaTime(string key) {
            return deltaTime * GetLocalTimeScale(key);
        }

        public static float GetLocalSmoothDeltaTime(string key) {
            return smoothDeltaTime * GetLocalTimeScale(key);
        }

        private static Dictionary<string, float> TimeScales = new Dictionary<string, float>();

        public static float time {
            get { return Time.time; }
        }

        public static float deltaTime {
            get { return Time.deltaTime; }
        }

        public static float smoothDeltaTime {
            get { return Time.smoothDeltaTime; }
        }

        public static float timeScale {
            get { return Time.timeScale; }
            set { Time.timeScale = value; }
        }

        public static int frameCount {
            get { return Time.frameCount; }
        }
    }
}