namespace UniCommon {
    public class DontDestroyOnLoad : ACommonBehaviour {
        private static DontDestroyOnLoad _instance;

        protected override void OnAwake() {
            if (_instance == null) {
                DontDestroyOnLoad(gameObject);
                _instance = this;
            }
            if (_instance == this) return;
            Destroy(gameObject);
        }
    }
}