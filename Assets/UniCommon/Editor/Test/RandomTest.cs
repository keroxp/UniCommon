using NUnit.Framework;

namespace UniCommon {
    [TestFixture]
    public class RandomTest {
        [Test]
        public void Shuffle_ShouldShuffle() {
            var list = new[] {1, 2, 3, 4};
            var list2 = new[] {1, 2, 3, 4};
            list.Shuffle();
            Debugs.Log(list.ToConcatenatedString());
            Debugs.Log(list2.ToConcatenatedString());
            var f = false;
            for (var i = 0; i < list.Length; i++) {
                if (list[i] != list2[i]) {
                    f = true;
                }
            }
            Assert.AreEqual(true, f);
        }
    }
}