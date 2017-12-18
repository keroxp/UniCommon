using System;
using System.IO;
using UnityEngine;

namespace UniCommon {
    public interface IStreamLogger<T> : ILogger, IDisposable where T : Stream {
        void Flush(bool sync);
        bool AutoFlush { get; set; }
    }

    internal sealed class StreamLogger<T> : TaggedLogger, IStreamLogger<T> where T : Stream {
        private readonly StreamLogHandler<T> _streamLogHandler;

        public StreamLogger(string tag, ILogFormatter formatter, StreamLogHandler<T> handler) : base(tag,
            formatter,
            handler) {
            _streamLogHandler = handler;
        }

        public void Flush(bool sync = false) {
            _streamLogHandler.Flush(sync);
        }

        public bool AutoFlush {
            get { return _streamLogHandler.AutoFlush; }
            set { _streamLogHandler.AutoFlush = value; }
        }

        public void Dispose() {
            _streamLogHandler.Dispose();
        }
    }
}