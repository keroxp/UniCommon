using System.IO;
using System.Text;
using NUnit.Framework;

namespace UniCommon {
    [TestFixture]
    public class FileManagerTest {
        string path = "Temp/studio.hexat.test.tmp";

        [Test]
        public void TryWrite_ShoulWriteDataToFile() {
            var data = "Ten Times Tiner Than They Think";
            FileManager.TryWrite(path, Encoding.UTF8.GetBytes(data));
            Assert.True(File.Exists(path));
            var bytes = FileManager.TryRead(path);
            var read = Encoding.UTF8.GetString(bytes);
            Assert.AreEqual(data, read);
        }

        [Test]
        public void TryReadBackup_ShoulReadBackupFile() {
            var data = "Ten Times Tiner Than They Think";
            var data2 = data + " ver2";
            FileManager.TryWrite(path, Encoding.UTF8.GetBytes(data));
            FileManager.TryWrite(path, Encoding.UTF8.GetBytes(data2));
            Assert.True(FileManager.HasBackup(path));
            Assert.AreEqual(data, Encoding.UTF8.GetString(FileManager.TryReadBackup(path)));
            Assert.AreEqual(data2, Encoding.UTF8.GetString(FileManager.TryRead(path)));
        }

        [Test]
        public void TryWrite_ShouldRewriteDataIfFilExist() {
            var data = "hoge";
            FileManager.TryWrite(path, Encoding.ASCII.GetBytes(data));
            var data2 = "fuga";
            FileManager.TryWrite(path, Encoding.ASCII.GetBytes(data2));
            var read = FileManager.TryRead(path);
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