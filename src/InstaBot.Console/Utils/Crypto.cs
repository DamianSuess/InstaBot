using System.Security.Cryptography;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace InstaBot.Console.Utils
{
    public class SHA256 : HMAC
    {
        public SHA256(byte[] key)
        {
            HashName = "System.Security.Cryptography.SHA256CryptoServiceProvider";
            HashSizeValue = 256;
            BlockSizeValue = 64;
            Key = (byte[])key.Clone();
        }
    }

    public static class Crypto
    {

        public static string CreateToken(string message, string secret)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            SHA256 hmacsha512 = new SHA256(encoding.GetBytes(secret));
            byte[] result = hmacsha512.ComputeHash(encoding.GetBytes(message));
            string sbinary = "";
            for (int i = 0; i < result.Length; i++)
                sbinary += result[i].ToString("x2"); /* hex format */
            return sbinary;
        }

        //    public static string CreateToken(string message, string secret)
        //    {
        //        byte[] hash = HashHMAC(HexDecode(secret), StringEncode(message));
        //        return HashEncode(hash);
        //    }

        private static byte[] HashSHA(byte[] innerKey, byte[] outerKey, byte[] message)
        {
            var hash = new SHA256Managed();

            // Compute the hash for the inner data first
            byte[] innerData = new byte[innerKey.Length + message.Length];
            Buffer.BlockCopy(innerKey, 0, innerData, 0, innerKey.Length);
            Buffer.BlockCopy(message, 0, innerData, innerKey.Length, message.Length);
            byte[] innerHash = hash.ComputeHash(innerData);

            // Compute the entire hash
            byte[] data = new byte[outerKey.Length + innerHash.Length];
            Buffer.BlockCopy(outerKey, 0, data, 0, outerKey.Length);
            Buffer.BlockCopy(innerHash, 0, data, outerKey.Length, innerHash.Length);
            byte[] result = hash.ComputeHash(data);

            return result;
        }
        private static byte[] HashHMAC(byte[] message, byte[] key)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }
        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        private static byte[] HexDecode(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }
    }
}
