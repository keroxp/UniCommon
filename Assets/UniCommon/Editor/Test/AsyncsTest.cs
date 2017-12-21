using System;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UniRx;

namespace UniCommon {
    [TestFixture]
    public class AsyncExecutorTest {
        [Test]
        public void TestAsync() {
            var finished = false;
            var hasError = false;
            var start = new DateTime().Millisecond;
            Asyncs.Execute(() => { Thread.Sleep(100); })
                .Subscribe(_ => {
                    Debugs.Log("completed");
                    finished = true;
                }, e => {
                    Debugs.Exception(e);
                    hasError = true;
                    finished = true;
                });
            Thread.Sleep(300);
            Assert.AreEqual(true, finished);
            Assert.AreEqual(false, hasError);
        }

        [Test]
        public void TestAsyncGeneric() {
            var finished = false;
            var hasError = false;
            var ret = 0;
            var exp = 10;
            Asyncs.Execute(() => {
                    Thread.Sleep(100);
                    return exp;
                })
                .Subscribe(r => {
                    ret = r;
                    finished = true;
                }, err => { hasError = true; });
            Thread.Sleep(300);
            Assert.AreEqual(true, finished);
            Assert.AreEqual(false, hasError);
            Assert.AreEqual(exp, ret);
        }

        [Test]
        public void TestAsyncCallback() {
            var finished = false;
            var receivedOnBackgroundThread = false;
            // メインスレッドで受け取るテストはThread.Sleepを使うテストだと無理な気がする…
            Asyncs.Execute(() => { Thread.Sleep(100); }, () => {
                finished = true;
                receivedOnBackgroundThread = Thread.CurrentThread.IsBackground;
            }, false);
            Thread.Sleep(300);
            Assert.True(finished);
            Assert.True(receivedOnBackgroundThread);
        }

        [Test]
        public void TestAsyncReturnCallback() {
            var finished = false;
            var receivedOnBackgroundThread = false;
            var ret = 0;
            Asyncs.Execute(() => {
                Thread.Sleep(100);
                return 123;
            }, i => {
                finished = true;
                receivedOnBackgroundThread = Thread.CurrentThread.IsBackground;
                ret = i;
            }, false);
            Thread.Sleep(300);
            Assert.True(finished);
            Assert.True(receivedOnBackgroundThread);
            Assert.AreEqual(123, ret);
        }
    }
}