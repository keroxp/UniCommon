using System;
using System.Threading;
using NUnit.Framework;

namespace UniCommon {
    [TestFixture]
    public class AsyncExecutorTest {
        [Test]
        public void TestAsync() {
            var finished = false;
            var hasError = false;
            var start = new DateTime().Millisecond;
            Asyncs.Execute(new AsyncTask() {
                onEntry = () => Debugs.Log("entry"),
                action = () => Thread.Sleep(200),
                onCompleted = () => {
                    Debugs.Log("completed");
                    finished = true;
                },
                onError = e => {
                    hasError = true;
                    finished = true;
                }
            });
            while (!finished) {
                if (new DateTime().Millisecond - start > 300) break;
            }
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
            Asyncs.Execute(new AsyncTask<int>() {
                onEntry = () => Debugs.Log("entry"),
                action = () => {
                    Thread.Sleep(200);
                    return exp;
                },
                onCompleted = (i) => {
                    Debugs.Log("completed");
                    ret = i;
                    finished = true;
                },
                onError = e => {
                    hasError = true;
                    finished = true;
                }
            });
            while (!finished) {
                if (new DateTime().Millisecond - start > 300) break;
            }
            Assert.AreEqual(true, finished);
            Assert.AreEqual(false, hasError);
            Assert.AreEqual(exp, ret);
        }
    }
}