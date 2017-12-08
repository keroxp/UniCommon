using System.Collections.Generic;
using UnityEngine;

namespace UniCommon {
    public static class ComponentExt {
        public static void RotateBy2D(this Transform self, float rotateBy) {
            var r = self.rotation.eulerAngles;
            self.rotation = Quaternion.Euler(r.x, r.y, r.z + rotateBy);
        }

        public static List<T> FindDirectChildren<T>(this Transform self) where T : Component {
            var tmp = new List<T>();
            for (var i = 0; i < self.transform.childCount; i++) {
                var child = self.transform.GetChild(i);
                var g = child.GetComponent<T>();
                if (g != null) {
                    tmp.Add(g);
                }
            }
            return tmp;
        }

        public static T GetComponentInSelfOrChildren<T>(this Component self) where T : Component {
            var q = new Queue<Transform>();
            q.Enqueue(self.transform);
            while (q.Count != 0) {
                var t = q.Dequeue();
                var c = t.GetComponent<T>();
                if (c != null) return c;
                for (var i = 0; i < t.childCount; i++)
                    q.Enqueue(t.GetChild(i));
            }
            return null;
        }
    }
}