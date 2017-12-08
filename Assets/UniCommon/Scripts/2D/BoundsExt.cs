using UnityEngine;

namespace UniCommon {
    public static class BoundsExt {
        public static bool Contains2D(this Bounds self, Vector3 point) {
            return self.min.x <= point.x && self.min.y <= point.y && point.x <= self.max.x && point.y <= self.max.y;
        }

        public static bool ContainsBounds2D(this Bounds self, Bounds bounds) {
            return self.min.x <= bounds.min.x && self.min.y <= bounds.min.y && bounds.max.x <= self.max.x &&
                   bounds.max.y <= self.max.y;
        }

        public static bool IntersectsBounds2D(this Bounds self, Bounds bounds) {
            var tl = bounds.min;
            var tr = bounds.min + new Vector3(bounds.size.x, 0, 0);
            var bl = bounds.min + new Vector3(0, bounds.size.y, 0);
            var br = bounds.max;
            return self.Contains2D(tl) || self.Contains2D(tr) || self.Contains2D(bl) || self.Contains2D(br);
        }

        public static bool CrossesBounds2D(this Bounds self, Bounds bounds) {
            return !self.ContainsBounds2D(bounds) && self.IntersectsBounds2D(bounds);
        }

        public static Vector2[] ToPath(this Bounds self) {
            var ret = new Vector2[4];
            ret[0] = new Vector2(self.min.x, self.min.y);
            ret[1] = new Vector2(self.max.x, self.min.y);
            ret[2] = new Vector2(self.max.x, self.max.y);
            ret[3] = new Vector2(self.min.x, self.max.y);
            return ret;
        }
    }
}