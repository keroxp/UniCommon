using NUnit.Framework;
using UnityEngine;

namespace UniCommon {
    [TestFixture]
    public class KeyValueStorageSecureTest {
        private IKeyValueStorage kvs;
        private string key = "key";

        [SetUp]
        public void SetUp() {
            kvs = KeyValueStorage.Secure("Test", Application.version, Crypter.Default(Application.version));
        }

        [TearDown]
        public void TearDown() {
            kvs.Delete(key);
        }

        [Test]
        public void Upsert_String() {
            kvs.Upsert(key, "hogehoge");
            Assert.AreEqual("hogehoge", kvs.GetString(key));
        }

        [Test]
        public void Upsert_Int() {
            kvs.Upsert(key, 100);
            Assert.AreEqual(100, kvs.GetInt(key));
        }

        [Test]
        public void Upsert_Float() {
            kvs.Upsert(key, Mathf.PI);
            Assert.AreEqual(Mathf.PI, kvs.GetFloat(key));
        }

        [Test]
        public void Upsert_Bool() {
            kvs.Upsert(key, true);
            Assert.AreEqual(true, kvs.GetBool(key));
        }
    }
}