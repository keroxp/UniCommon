using System;
using System.IO;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniCommon {
    public sealed class FileLogHandler : StreamLogHandler<FileStream> {
        public readonly string path;

        public FileLogHandler(string path, TimeSpan flushInterval, bool writeNewLine = true,
            FileMode fileMode = FileMode.OpenOrCreate) : base(
            new FileStream(path, fileMode, FileAccess.Write, FileShare.Read), flushInterval, writeNewLine) {
            this.path = path;
        }
    }
}