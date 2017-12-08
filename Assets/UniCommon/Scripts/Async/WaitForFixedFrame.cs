using UnityEngine;

namespace UniCommon {
    public class WaitForFixedFrame : CustomYieldInstruction {
        private int count;
        private int i;

        public WaitForFixedFrame(int count) {
            this.count = count;
        }

        public override bool keepWaiting {
            get { return ++i < count; }
        }
    }
}