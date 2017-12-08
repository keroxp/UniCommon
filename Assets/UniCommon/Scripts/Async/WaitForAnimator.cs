using UnityEngine;

namespace UniCommon {
    public class WaitForAnimator : CustomYieldInstruction {
        private readonly Animator _animator;
        private readonly int _layerIndex;
        private readonly bool _forState;
        private readonly string _name;

        public static CustomYieldInstruction New(Animator animator, int layerIndex = 0) {
            var path = animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash;
            return new ClosureYieldInstruction(() => {
                var st = animator.GetCurrentAnimatorStateInfo(layerIndex);
                return st.fullPathHash == path && st.normalizedTime < 1.0f;
            });
        }

        public static CustomYieldInstruction State(Animator animator, string state, int layerIndex = 0) {
            return new WaitForAnimator(animator, state, layerIndex, true);
        }

        public static CustomYieldInstruction Tag(Animator animator, string tag, int layerIndex = 0) {
            return new WaitForAnimator(animator, tag, layerIndex, false);
        }

        private WaitForAnimator(Animator animator, string name, int layerIndex = 0, bool forState = false) {
            _animator = animator;
            _layerIndex = layerIndex;
            _forState = forState;
            _name = name;
        }

        public override bool keepWaiting {
            get {
                var st = _animator.GetCurrentAnimatorStateInfo(_layerIndex);
                if (_forState) return !st.IsName(_name) || st.normalizedTime < 1.0f;
                return !st.IsTag(_name) || st.normalizedTime < 1.0f;
            }
        }
    }
}