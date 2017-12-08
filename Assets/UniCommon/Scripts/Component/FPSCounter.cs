using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace UniCommon {
    public class FPSCounter : SingletonBehaviour<FPSCounter>, IObservable<int> {
        private int fpsSum;
        private int seconds;
        private int prevFrame;
        private readonly Subject<int> _subject = new Subject<int>();

        public int Average {
            get { return fpsSum / seconds; }
        }

        protected override void OnStart() {
            base.OnStart();
            Reset();
            StartCoroutine(CountFps());
        }

        private IEnumerator CountFps() {
            while (true) {
                yield return new WaitForSeconds(1);
                var fps = Times.frameCount - prevFrame;
                fpsSum += fps;
                seconds++;
                prevFrame = Times.frameCount;
                _subject.OnNext(fps);
            }
        }

        public IDisposable Subscribe(IObserver<int> observer) {
            return _subject.Subscribe(observer);
        }

        public void Reset() {
            fpsSum = 0;
            seconds = 0;
            prevFrame = Times.frameCount;
        }
    }
}