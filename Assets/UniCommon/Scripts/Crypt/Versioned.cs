using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace UniCommon {
    public interface IVersionedData<T> {
        string Version { get; }
        T Data { get; }
    }

    public static class Versioned {
        private struct VersionedData<T> : IVersionedData<T> {
            private readonly T _data;
            private readonly string _version;

            public string Version {
                get { return _version; }
            }

            public T Data {
                get { return _data; }
            }

            public VersionedData(string version, T data) {
                _version = version;
                _data = data;
            }
        }

        public static string WithExtension(string basename) {
            return basename + ".vsn";
        }

        public class WrapFailedException : Exception {
            public WrapFailedException(Exception innerException) : base("failed to wrap data", innerException) {
            }
        }

        public class UnwrapFailedException : Exception {
            public UnwrapFailedException(Exception innerException) : base("failed to unwrap data", innerException) {
            }
        }

        private static readonly string Signature = "unicommonvsn";

        private struct VersionedStructure {
            public char[] signature;
            public UInt32 versionLength;
            public byte[] version;
            public UInt32 dataLength;
            public byte[] data;

            public int Size() {
                return signature.Length + 4 + version.Length + 4 + data.Length;
            }
        }

        private static VersionedStructure MakeVersioned(string version, byte[] data) {
            return new VersionedStructure() {
                signature = Signature.ToCharArray(),
                versionLength = (uint) Encoding.UTF8.GetByteCount(version),
                version = Encoding.UTF8.GetBytes(version),
                dataLength = (uint) data.Length,
                data = data
            };
        }

        public static byte[] TryWrap(string version, byte[] data) {
            try {
                var vsn = MakeVersioned(version, data);
                var buf = new byte[vsn.Size()];
                using (var stream = new MemoryStream(buf)) {
                    using (var writer = new BinaryWriter(stream)) {
                        writer.Write(vsn.signature);
                        writer.Write(vsn.versionLength);
                        writer.Write(vsn.version);
                        writer.Write(vsn.dataLength);
                        writer.Write(vsn.data);
                        writer.Flush();
                    }
                }
                return buf;
            } catch (Exception e) {
                throw new WrapFailedException(e);
            }
        }

        public static IVersionedData<byte[]> TryUnwrap(byte[] data) {
            try {
                using (var strem = new MemoryStream(data)) {
                    using (var reader = new BinaryReader(strem)) {
                        var chars = reader.ReadChars(Signature.Length);
                        var sig = new string(chars);
                        if (sig != Signature)
                            throw new Exception(string.Format("invalid signature: {0}", sig));
                        var version = Encoding.UTF8.GetString(reader.ReadBytes((int) reader.ReadUInt32()));
                        var bytes = reader.ReadBytes((int) reader.ReadUInt32());
                        return new VersionedData<byte[]>(version, bytes);
                    }
                }
            } catch (Exception e) {
                throw new UnwrapFailedException(e);
            }
        }
    }
}