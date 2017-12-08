using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace UniCommon {
    public class Bytes {
        public static byte[] Serialize<T>(T data) {
            using (var mem = new MemoryStream()) {
                var bf = new BinaryFormatter();
                bf.Serialize(mem, data);
                return mem.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] bytes) {
            using (var mem = new MemoryStream(bytes.Length)) {
                mem.Write(bytes, 0, bytes.Length);
                mem.Seek(0, SeekOrigin.Begin);
                var bf = new BinaryFormatter();
                return (T) bf.Deserialize(mem);
            }
        }
    }
}