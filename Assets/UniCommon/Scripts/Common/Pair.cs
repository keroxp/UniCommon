using System;

namespace UniCommon {
    [Serializable]
    public struct Pair<TA, TB> {
        public readonly TA left;
        public readonly TB right;

        public Pair(TA left, TB right) {
            this.left = left;
            this.right = right;
        }

        public override bool Equals(object obj) {
            if (!(obj is Pair<TA, TB>)) return false;
            var other = (Pair<TA, TB>) obj;
            return left.Equals(other.left) && right.Equals(other.right);
        }

        public override int GetHashCode() {
            var h0 = left.GetHashCode();
            h0 = (h0 << 5) + h0 ^ right.GetHashCode();
            return h0;
        }
    }
}