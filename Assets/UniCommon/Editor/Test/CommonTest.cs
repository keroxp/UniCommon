using NUnit.Framework;

namespace UniCommon {
    [TestFixture]
    public class CommonTest {
        [Test]
        public void Test_StringExt_TrimSpaceAndParenthesis() {
            const string s = "hoge (way) (wawwa)  ";
            Assert.AreEqual("hoge", s.TrimSpaceAndParenthesis());
        }
    }
}