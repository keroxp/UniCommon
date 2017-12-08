using System.Text;
using NUnit.Framework;

namespace UniCommon {
    [TestFixture]
    public class VersionedTest {
        [Test]
        public void Wrap_UnWrap() {
            var data = "Hexatすたじお☺";
            var version = "1.0.0";
            var ret = Versioned.TryWrap(version, Encoding.UTF8.GetBytes(data));
            var decrypted = Versioned.TryUnwrap(ret);
            Assert.AreEqual(version, decrypted.Version);
            Assert.AreEqual(data, Encoding.UTF8.GetString(decrypted.Data));
        }

        [Test]
        public void Wrap_Null_Version() {
            Assert.Throws<Versioned.WrapFailedException>(() => Versioned.TryWrap(null, new byte[] {1, 2, 3}));
        }

        [Test]
        public void Wrap_Empty_Version() {
            var a = Versioned.TryUnwrap(Versioned.TryWrap("", new byte[] {1, 2, 3}));
            Assert.AreEqual("", a.Version);
        }

        [Test]
        public void Wrap_Null_Value() {
            Assert.Throws<Versioned.WrapFailedException>(() => Versioned.TryWrap("1.01.0", null));
        }

        [Test]
        public void Wrap_Empty_Value() {
            var a = Versioned.TryUnwrap(Versioned.TryWrap("1.01.0", new byte[0]));
            Assert.AreEqual(0, a.Data.Length);
        }

        [Test]
        public void UnWrap_Null_Data() {
            Assert.Throws<Versioned.UnwrapFailedException>(() => Versioned.TryUnwrap(null));
        }

        [Test]
        public void UnWrap_Invalid_Data() {
            var d = new byte[] {1, 2, 3, 3, 4, 5};
            Assert.Throws<Versioned.UnwrapFailedException>(() => Versioned.TryUnwrap(d));
        }
    }
}