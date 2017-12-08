using UnityEngine;

namespace UniCommon {
    public static class Vectors {
        public static bool Approximately(this Vector3 self, Vector3 other) {
            return Mathf.Approximately(self.x, other.x) && Mathf.Approximately(self.y, other.y) &&
                   Mathf.Approximately(self.z, other.z);
        }

        public static Vector3 Deg2Vec3(float angle) {
            return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        }

        public static Vector3 ToVector3(this Vector2 self, float z = 0) {
            return new Vector3(self.x, self.y, z);
        }

        public static Vector2 ToVector2(this Vector3 self) {
            return new Vector2(self.x, self.y);
        }

        public static float ToDeg(this Vector3 self) {
            var r = Mathf.Atan2(self.y, self.x);
            if (r < 0) r += 360;
            return r;
        }

        public static Vector3 Rotate2D(this Vector3 self, float angle) {
            var theta = Mathf.Deg2Rad * angle;
            var cost = Mathf.Cos(theta);
            var sint = Mathf.Sin(theta);
            return new Vector3(self.x * cost - self.y * sint, self.x * sint + self.y * cost, self.z);
        }

        public static Vector3 Clamp(this Vector3 self, Vector3 min, Vector3 max) {
            var ret = self;
            ret.x = ret.x.Clamp(min.x, max.x);
            ret.y = ret.y.Clamp(min.y, max.y);
            ret.z = ret.z.Clamp(min.z, max.z);
            return ret;
        }
    }
}