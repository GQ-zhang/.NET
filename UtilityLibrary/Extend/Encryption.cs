using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLibrary.Extend
{
    public static class Encryption
    {
        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StringEncryption(this string input)
        {
            using var md5Hasher = SHA256.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().ToUpper();
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string text, string sKey = "79151B511E10409FB0583F3AC84FBE17")
        {
            using var aes = Aes.Create();
            byte[] encrypted;
            var key = Encoding.ASCII.GetBytes(SHAHash(sKey).Substring(0, 32));
            var iv = Encoding.ASCII.GetBytes(SHAHash(sKey).Substring(0, 16));
            ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(text);
                }
                encrypted = msEncrypt.ToArray();
            }
            StringBuilder ret = new StringBuilder();
            foreach (byte b in encrypted)
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string text, string sKey = "79151B511E10409FB0583F3AC84FBE17")
        {
            using Aes aesAlg = Aes.Create();
            string decryptText;
            int len;
            len = text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            var key = Encoding.ASCII.GetBytes(SHAHash(sKey).Substring(0, 32));
            var iv = Encoding.ASCII.GetBytes(SHAHash(sKey).Substring(0, 16));
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv);

            using MemoryStream msDecrypt = new MemoryStream(inputByteArray);
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                decryptText = srDecrypt.ReadToEnd();
            }

            return decryptText;
        }

        /// <summary>
        /// SHA-256算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string SHAHash(string input)
        {
            using var md5Hasher = SHA256.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
