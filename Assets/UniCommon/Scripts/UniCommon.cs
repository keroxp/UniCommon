namespace UniCommon {
    public class UniCommon : AManagedBehaviour {
        public bool initializeOnAwake = true;
        private static bool initialized;

        public static void Initialize(bool force = false) {
            if (!force && initialized) return;
            NetworkReachabilities.Initialize();
            Analytics.Initialize();
            AnalyticsStorage.Initialize();
            initialized = true;
        }

        protected override void OnAwake() {
            base.OnAwake();
            if (initializeOnAwake)
                Initialize();
        }

    }
}