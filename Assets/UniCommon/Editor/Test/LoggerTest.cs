using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace UniCommon {
    [TestFixture]
    public class LoggerTest {
        [Test]
        public void TestFileLogger() {
            using (var logger = Loggers.NewFileLogger("test",
                "Temp/test.log",
                TimeSpan.FromMinutes(1),
                null,
                true,
                FileMode.Create)) {
                logger.Log("way", "loglog");
                var bytes = Files.TryRead("Temp/test.log");
                // 書き込まれてない
                Assert.AreEqual(0, bytes.Length);
                logger.Flush(true);
                // 書き込まれてる
                bytes = Files.TryRead("Temp/test.log");
                var line = Encoding.UTF8.GetString(bytes);
                Assert.AreEqual(true, bytes.Length > 0);
                Debugs.Log(line);
                var regex = new Regex(
                    "\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}\\+\\d{2}:\\d{2} \\d{3}ms/\\d+?F <L> \\[test/way\\]: loglog$");
                Assert.AreEqual(true, regex.Match(line).Success);
            }
        }
    }
}