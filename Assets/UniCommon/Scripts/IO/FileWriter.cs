using System;
using System.IO;
using System.Text;

namespace UniCommon {
    public static class FileWriters {
        public static IStringFileWriter NewStringFileWriter(string filePath, bool writeNewLine = true) {
            return new StringFileWriter(filePath, writeNewLine);
        }
    }

    public interface IFileWriter<in T> : IDisposable {
        string FilePath { get; }
        void TryWrite(T log);
        void Flush();
    }

    public interface IStringFileWriter : IFileWriter<string> {
        bool WriteNewLine { get; set; }
    }

    public class StringFileWriter : IStringFileWriter {
        private FileStream _fileStream;
        private StreamWriter _writer;
        public bool WriteNewLine { get; set; }
        public string FilePath { get; private set; }

        public StringFileWriter(string filePath, bool writeNewLine = true) {
            FilePath = filePath;
            WriteNewLine = writeNewLine;
        }

        public void Dispose() {
            lock (this) {
                if (_writer != null) {
                    _writer.Dispose();
                    _writer = null;
                }
                if (_fileStream != null) {
                    _fileStream.Dispose();
                    _fileStream = null;
                }
            }
        }

        public void Flush() {
            GetWriter().Flush();
        }

        private StreamWriter GetWriter() {
            if (_fileStream != null && _fileStream.CanWrite) return _writer;
            _fileStream = new FileStream(FilePath, File.Exists(FilePath) ? FileMode.Append : FileMode.Create);
            _writer = new StreamWriter(_fileStream, Encoding.UTF8, 1024) {AutoFlush = false};
            return _writer;
        }

        public void TryWrite(string log) {
            lock (this) {
                var w = GetWriter();
                if (WriteNewLine) {
                    w.WriteLine(log);
                } else {
                    w.Write(log);
                }
            }
        }
    }
}