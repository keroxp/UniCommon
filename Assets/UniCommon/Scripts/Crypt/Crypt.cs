using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace UniCommon {
    public interface ICrypter {
        byte[] TryEncrypt(byte[] data);
        byte[] TryDecrypt(byte[] data);
    }

    public static class Crypter {
        public static ICrypter Default(SecretProvider secretProvider) {
            return new ValueCrypter(secretProvider);
        }
    }

    internal abstract class ABasicCtypter : ICrypter {
        public abstract byte[] TryEncrypt(byte[] data);
        public abstract byte[] TryDecrypt(byte[] data);
    }

    public delegate string SecretProvider();

    internal class ValueCrypter : ABasicCtypter {
        private readonly string SALT = "kQlvDXm3d3L6tZR1";
        private readonly RijndaelManaged rijndael;
        private SecretProvider secretProvider;

        public ValueCrypter(SecretProvider secretProvider) {
            rijndael = new RijndaelManaged();
            this.secretProvider = secretProvider;
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
                throw new Exception("failed to enctypt", e);
            }
        }

        public override byte[] TryDecrypt(byte[] data) {
            try {
                using (var decryptor = rijndael.CreateDecryptor()) {
                    return decryptor.TransformFinalBlock(data, 0, data.Length);
                }
            } catch (Exception e) {
                throw new Exception("failed to decrypt", e);
            }
        }

        private void GenerateKeyFromPassword(int keySize, out byte[] key, int blockSize, out byte[] iv) {
            var salt = Encoding.UTF8.GetBytes(SALT);
            var pass = secretProvider();
            var deriveBytes = new Rfc2898DeriveBytes(pass, salt);
            deriveBytes.IterationCount = 1000;
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }
    }
}