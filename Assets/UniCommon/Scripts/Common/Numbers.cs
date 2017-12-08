using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniCommon {
    public static class FloatExt {
        public static float InvertIf(this float self, bool flag) {
            return flag ? self * -1 : self;
        }

        public enum RangeOption {
            InEx,
            InIn,
            ExIn,
            ExEx
        }

        public static bool IsInRange(this float self, float min, float max, RangeOption opt = RangeOption.InEx) {
            switch (opt) {
                case RangeOption.InEx:
                    return min <= self && self < max;
                case RangeOption.InIn:
                    return min <= self && self <= max;
                case RangeOption.ExIn:
                    return min < self && self <= max;
                case RangeOption.ExEx:
                    return min < self && self < max;
                default:
                    throw new ArgumentOutOfRangeException("opt", opt, null);
            }
        }

        public static float FlipXIf(this float self, bool flag) {
            if (!flag) return self;
            var v = self;
            v %= 360f;
            return 180 - v;
        }

        public static Vector3 Deg2Vec(this float self) {
            return Vectors.Deg2Vec3(self);
        }

        public static Vector3 ToVec(this float self) {
            return new Vector3(self, self, self);
        }

        public static float Clamp(this float self, float min, float max) {
            return Mathf.Clamp(self, min, max);
        }
    }

    public static class IntExt {
        public delegate void IterateFunc(int i);

        public static void Times(this int self, IterateFunc func) {
            for (var i = 0; i < self; i++) func(i);
        }

        public static IEnumerable<T> Map<T>(this int self, Func<int, T> func) {
            var ret = new List<T>();
            for (var i = 0; i < self; i++) ret.Add(func(i));
            return ret;
        }
    }
}