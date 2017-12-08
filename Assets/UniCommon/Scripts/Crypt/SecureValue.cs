using System;

namespace UniCommon {
    public interface ISecureValue<T> {
        T Value { get; set; }
    }

    [Serializable]
    public class SecureLong : ISecureValue<long> {
        private long _value;
        private readonly long _seed;

        public SecureLong(long value) {
            _value = value;
            var rnd = new Random();
            _seed = rnd.Next() << 32 | rnd.Next();
            Value = _value;
        }

        public SecureLong(long encoded, long seed) {
            _seed = seed;
            _value = encoded;
        }

        public long Value {
            get { return _value ^ _seed; }
            set { _value = value ^ _seed; }
        }
    }

    [Serializable]
    public class SecureInt : ISecureValue<int> {
        private int _value;
        private readonly int seed;

        public int Value {
            get { return _value ^ seed; }
            set { _value = value ^ seed; }
        }

        public SecureInt(int value = 0) {
            var rnd = new Random();
            seed = rnd.Next() << 32 | rnd.Next();
            Value = value;
        }

        public SecureInt(int encodedValue, int seed) {
            this.seed = seed;
            _value = encodedValue;
        }
    }

    [Serializable]
    public class SecureUInt : ISecureValue<uint> {
        private uint _value;
        private readonly uint seed;

        public SecureUInt(uint value = 0) {
            var rnd = new Random();
            seed = (uint) (rnd.Next() << 32 | rnd.Next());
            Value = value;
        }

        public SecureUInt(uint encodedValue, uint seed) {
            _value = encodedValue;
            this.seed = seed;
        }

        public uint Value {
            get { return _value ^ seed; }
            set { _value = value ^ seed; }
        }
    }

    [Serializable]
    public class SecureFloat : ISecureValue<float> {
        private byte[] _buffer;
        private readonly byte[] _bytes;
        private readonly byte seed;

        public SecureFloat(float v = 0) {
            _buffer = new byte[4];
            _bytes = new byte[4];
            var rnd = new Random();
            seed = (byte) (rnd.Next() << 32 | rnd.Next());
            Value = v;
        }

        public SecureFloat(byte[] encodedValue, byte seed) {
            _bytes = encodedValue;
            this.seed = seed;
        }

        public float Value {
            get {
                _bytes.CopyTo(_buffer, 0);
                Xor(ref _buffer);
                return BitConverter.ToSingle(_buffer, 0);
            }
            set {
                _buffer = BitConverter.GetBytes(value);
                Xor(ref _buffer);
                _buffer.CopyTo(_bytes, 0);
            }
        }

        private void Xor(ref byte[] arr) {
            arr[0] ^= seed;
            arr[1] ^= seed;
            arr[2] ^= seed;
            arr[3] ^= seed;
        }
    }
}