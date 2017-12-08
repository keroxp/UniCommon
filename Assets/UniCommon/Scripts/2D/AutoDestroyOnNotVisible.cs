using UnityEngine;

namespace UniCommon.TwoD {
    [RequireComponent(typeof(Renderer))]
    public class AutoDestroyOnNotVisible : AManagedBehaviour {
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