using System;
using UnityEngine;

namespace UniCommon {
    public class ClosureYieldInstruction : CustomYieldInstruction {
        private Func<bool> _predicate;

        public ClosureYieldInstruction(Func<bool> predicate) {
            _predicate = predicate;
        }

        public override bool keepWaiting {
            get { return _predicate(); }
        }
    }
}