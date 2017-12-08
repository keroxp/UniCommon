using System;
using System.Collections.Generic;

namespace UniCommon {
    public static class EqualityComparers {
        public static IEqualityComparer<T> New<T>(Func<T, T, bool> equaler, Func<T, int> hasher) {
            return new ClosureEuqalityCompaerer<T>(equaler, hasher);
        }
    }

    class ClosureEuqalityCompaerer<T> : IEqualityComparer<T> {
        private Func<T, T, bool> equaler;
        private Func<T, int> hasher;

        public ClosureEuqalityCompaerer(Func<T, T, bool> equaler, Func<T, int> hasher) {
            this.equaler = equaler;
            this.hasher = hasher;
        }

        public bool Equals(T x, T y) {
            return equaler(x, y);
        }

        public int GetHashCode(T obj) {
            return hasher(obj);
        }
    }
}