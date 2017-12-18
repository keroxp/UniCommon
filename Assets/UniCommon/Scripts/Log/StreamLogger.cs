using System;
using System.IO;
using UnityEngine;

namespace UniCommon {
    public sealed class StreamLogger<T> : TaggedLogger, IDisposable where T : Stream {
        private readonly StreamLogHandler<T> _streamLogHandler;

        public StreamLogger(string tag, ILogFormatter formatter, StreamLogHandler<T> handler) : base(tag, formatter,
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