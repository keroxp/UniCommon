using System.IO;
using System.Text;
using NUnit.Framework;

namespace UniCommon {
    [TestFixture]
    public class FilesTest {
        string path = "Temp/studio.hexat.test.tmp";

        [Test]
        public void TryWrite_ShoulWriteDataToFile() {
            var data = "Ten Times Tiner Than They Think";
            Files.TryWrite(path, Encoding.UTF8.GetBytes(data));
            Assert.True(File.Exists(path));
            var bytes = Files.TryRead(path);
            var read = Encoding.UTF8.GetString(bytes);
            Assert.AreEqual(data, read);
        }

        [Test]
        public void TryReadBackup_ShoulReadBackupFile() {
            var data = "Ten Times Tiner Than They Think";
            var data2 = data + " ver2";
            Files.TryWrite(path, Encoding.UTF8.GetBytes(data));
            Files.TryWrite(path, Encoding.UTF8.GetBytes(data2));
            Assert.True(Files.HasBackup(path));
            Assert.AreEqual(data, Encoding.UTF8.GetString(Files.TryReadBackup(path)));
            Assert.AreEqual(data2, Encoding.UTF8.GetString(Files.TryRead(path)));
        }

        [Test]
        public void TryWrite_ShouldRewriteDataIfFilExist() {
            var data = "hoge";
            Files.TryWrite(path, Encoding.ASCII.GetBytes(data));
            var data2 = "fuga";
            Files.TryWrite(path, Encoding.ASCII.GetBytes(data2));
            var read = Files.TryRead(path);
            Assert.AreEqual(data2, Encoding.ASCII.GetString(read));
        }

        [SetUp]
        public void SetUp() {
            if (File.Exists(path))
                File.Delete(path);
        }

        [TearDown]
        public void TearDown() {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}