using UnityEngine;

namespace UniCommon {
    public static class Gizmos {
        public static void DrawRect2D(float l, float t, float r, float b) {
            DrawLine2D(l, t, r, t);
            DrawLine2D(r, t, r, b);
            DrawLine2D(r, b, l, b);
            DrawLine2D(l, b, l, t);
        }

        public static void DrawLine2D(float sx, float sy, float ex, float ey) {
            UnityEngine.Gizmos.DrawLine(new Vector3(sx, sy, 0), new Vector3(ex, ey, 0));
        }

        public static void DrawCross2D(Vector3 pos, float rad) {
            DrawLine2D(pos.x - rad, pos.y, pos.x + rad, pos.y);
            DrawLine2D(pos.x, pos.y - rad, pos.x, pos.y + rad);
        }

        public static void DrawBounds2D(Bounds bounds) {
            DrawRect2D(bounds.min.x, bounds.max.y, bounds.max.x, bounds.min.y);
        }

        public static void DrawGrid2D(Vector3 center, Vector2 cellSize, Vector2 gridSize) {
            var left = center.x - cellSize.x * gridSize.x * .5f;
            var top = center.y + cellSize.y * gridSize.y * .5f;
            for (var i = 0; i < gridSize.x; i++) {
                for (var j = 0; j < gridSize.y; j++) {
                    var l = left + i * cellSize.x;
                    var t = top - j * cellSize.y;
                    var r = l + cellSize.x;
                    var b = t - cellSize.y;
                    DrawRect2D(l, t, r, b);
                }
            }
        }
    }
}