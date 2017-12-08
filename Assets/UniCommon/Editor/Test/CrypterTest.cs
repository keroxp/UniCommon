using System;
using System.Text;
using NUnit.Framework;
using UnityEngine;

namespace UniCommon {
    [TestFixture]
    public class CrypterTest {
        string str = "Hexatすたじお☺";
        private ICrypter c;

        [SetUp]
        public void SetUp() {
            c = Crypter.Default(Application.version);
        }

        [Test]
        public void Encrypt_String() {
            var d = c.TryDecrypt(c.TryEncrypt(Encoding.UTF8.GetBytes(str)));
            Assert.AreEqual(str, Encoding.UTF8.GetString(d));
        }

        [Test]
        public void Encrypt_Int() {
            var d = c.TryDecrypt(c.TryEncrypt(BitConverter.GetBytes(100)));
            Assert.AreEqual(100, BitConverter.ToInt32(d, 0));
        }

        [Test]
        public void Encrypt_UInt() {
            var d = c.TryDecrypt(c.TryEncrypt(BitConverter.GetBytes((uint) 100)));
            Assert.AreEqual(100, BitConverter.ToUInt32(d, 0));
        }

        [Test]
        public void Encrypt_Float() {
            var d = c.TryDecrypt(c.TryEncrypt(BitConverter.GetBytes(Mathf.PI)));
            Assert.AreEqual(Mathf.PI, BitConverter.ToSingle(d, 0));
        }

        [Test]
        public void Encrypt_Empty() {
            var d = c.TryDecrypt(c.TryEncrypt(new byte[0]));
            Assert.AreEqual(0, d.Length);
        }

        [Test]
        public void Encrypt_Null() {
            Assert.Throws<Crypter.EncryptFailedException>(() => c.TryEncrypt(null));
        }

        [Test]
        public void Decrypt_Null() {
            Assert.Throws<Crypter.DecryptFailedException>(() => c.TryDecrypt(null));
        }
    }
}