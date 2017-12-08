using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UniCommon {
    public class SecureValueTest {
        [Test]
        public void SecureFloatTest() {
            // Use the Assert class to test conditions.
            var v = new SecureFloat(0);
            Assert.True(Mathf.Approximately(v.Value, 0));
            v.Value += 1.22f;
            Assert.True(Mathf.Approximately(v.Value, 1.22f));
            v.Value = -0.911f;
            Assert.True(Mathf.Approximately(v.Value, -0.911f));
        }

        [Test]
        public void SecureIntTest() {
            var v = new SecureInt();
            Assert.True(v.Value == 0);
            v.Value = 2;
            Assert.True(v.Value == 2);
            v.Value += 10;
            Assert.True(v.Value == 12);
            v.Value -= 1000;
            Assert.True(v.Value == -988);
        }

        [Test]
        public void SecureUIntTest() {
            var v = new SecureUInt();
            Assert.True(v.Value == 0);
            v.Value = 2u;
            Assert.True(v.Value == 2);
            v.Value += 10u;
            Assert.True(v.Value == 12);
            v.Value -= 1000u;
            Assert.AreEqual(uint.MaxValue - 987, v.Value);
        }
    }
}