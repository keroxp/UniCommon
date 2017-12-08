using UnityEngine;

namespace UniCommon {
    public class WaitForAsyncOperation : CustomYieldInstruction {
        private readonly AsyncOperation _operation;

        public WaitForAsyncOperation(AsyncOperation operation, bool allowActivation = true) {
            _operation = operation;
            _operation.allowSceneActivation = allowActivation;
        }

        public override bool keepWaiting {
            get {
                if (_operation.allowSceneActivation) return !_operation.isDone;
                return _operation.progress < 0.9f;
            }
        }
    }
}