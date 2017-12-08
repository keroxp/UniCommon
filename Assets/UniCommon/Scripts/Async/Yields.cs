using System;
using System.Collections;
using UnityEngine;

namespace UniCommon {
    public static class Yields {
        public static CustomYieldInstruction Wrap(IEnumerator it) {
            return new ClosureYieldInstruction(it.MoveNext);
        }

        public static CustomYieldInstruction Await(Func<bool> predicate) {
            return new ClosureYieldInstruction(predicate);
        }

        public static IEnumerator New(Action action) {
            return new ClosureYieldInstruction(() => {
                action();
                return false;
            });
        }

        public static IEnumerator Interval(float interval, Func<bool> predicate) {
            while (predicate()) yield return new WaitForSeconds(interval);
        }

        public static IEnumerator Interval(float interval, Action action) {
            while (true) {
                action();
                yield return new WaitForSeconds(interval);
            }
        }

        public static IEnumerator Delay(float delay, Action action) {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static IEnumerator WaitForSeconds(float duration) {
            yield return new WaitForSeconds(duration);
        }

        public static IEnumerator WaitForSeconds(float duration, string localTimeKey) {
            var t = 0f;
            while (t < duration) {
                t += Times.GetLocalDeltaTime(localTimeKey);
                yield return 0;
            }
        }

        public static DisposableCoroutine ToDisposable(this IEnumerator coroutine) {
            return new DisposableCoroutine(coroutine);
        }

        public static IEnumerator WaitForTap(float delay = 0) {
            if (delay > 0) yield return WaitForSeconds(delay);
            var tapped = false;
//            EventStream.Subscribe<PlayerTapped>(tap => tapped = true);
            while (!tapped) {
                yield return 0;
            }
        }
    }
}