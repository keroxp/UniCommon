using UnityEngine;

namespace UniCommon {
    [RequireComponent(typeof(Renderer))]
    public class AutoDestroyOnNotVisible : ACommonBehaviour {
        private Renderer _renderer;

        protected override void OnStart() {
            base.OnStart();
            _renderer = GetComponent<Renderer>();
        }

        private bool _isAppeared;

        protected override void OnUpdate() {
            base.OnUpdate();
            if (!_isAppeared && _renderer.isVisible) {
                _isAppeared = true;
            }
            if (_isAppeared && !_renderer.isVisible) {
                DoEndOfFrame(() => { Destroy(gameObject); });
            }
        }
    }
}