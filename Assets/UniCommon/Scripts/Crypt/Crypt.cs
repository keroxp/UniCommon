using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UniCommon.Crypt;

namespace UniCommon {
    public interface ICrypter {
        byte[] TryEncrypt(byte[] data);
        byte[] TryDecrypt(byte[] data);
    }

    public static class Crypter {
        private static readonly Dictionary<string, ICrypter> Crypters = new Dictionary<string, ICrypter>();

        public static ICrypter Default(string version) {
            if (Crypters.ContainsKey(version)) return Crypters[version];
            return Crypters[version] = new ValueCrypter(version);
        }

        public class EncryptFailedException : Exception {
            public EncryptFailedException(Exception innerException) : base("failed to encrypt data", innerException) {
            }
        }

        public class DecryptFailedException : Exception {
            public DecryptFailedException(Exception innerException) : base("failed to decrypt data", innerException) {
            }
        }
    }

    internal abstract class AVersionedCrypter : ICrypter {
        protected string Version;

        protected AVersionedCrypter(string version) {
            Version = version;
        }

        public abstract byte[] TryEncrypt(byte[] data);
        public abstract byte[] TryDecrypt(byte[] data);
    }

    internal class ValueCrypter : AVersionedCrypter {
        private readonly string SALT = "kQlvDXm3d3L6tZR1";
        private readonly RijndaelManaged rijndael;

        public ValueCrypter(string version) : base(version) {
            rijndael = new RijndaelManaged();
            byte[] key, iv;
            GenerateKeyFromPassword(rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;
        }

        public override byte[] TryEncrypt(byte[] data) {
            try {
                using (var encryptor = rijndael.CreateEncryptor()) {
                    return encryptor.TransformFinalBlock(data, 0, data.Length);
                }
            } catch (Exception e) {
                throw new Crypter.EncryptFailedException(e);
            }
        }

        public override byte[] TryDecrypt(byte[] data) {
            try {
                using (var decryptor = rijndael.CreateDecryptor()) {
                    return decryptor.TransformFinalBlock(data, 0, data.Length);
                }
            } catch (Exception e) {
                throw new Crypter.DecryptFailedException(e);
            }
        }

        private void GenerateKeyFromPassword(int keySize, out byte[] key, int blockSize, out byte[] iv) {
            var salt = Encoding.UTF8.GetBytes(SALT);
            var pass = Secret.GetSecret(this.Version);
            var deriveBytes = new Rfc2898DeriveBytes(pass, salt);
            deriveBytes.IterationCount = 1000;
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }
    }
}