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
                    },
                    e => {
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
            var start = new DateTime().Millisecond;
            var ret = 0;
            var exp = 10;
            Asyncs.Execute(() => {
                    Thread.Sleep(100);
                    return exp;
                })
                .Subscribe(r => {
                        ret = r;
                        finished = true;
                    },
                    err => { hasError = true; });
            Thread.Sleep(300);
            Assert.AreEqual(true, finished);
            Assert.AreEqual(false, hasError);
            Assert.AreEqual(exp, ret);
        }
    }
}