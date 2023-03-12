using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IBApp3.Model
{
    public static class CryptoClass
    {
        private static readonly byte[] KEY = Enumerable.Range(0, 32).Select(x => (byte)x).ToArray();

        public static string? Encrypt(string? text) //зашифровать файл
        {
            if (text != null)
            {
                using Aes aes = Aes.Create();
                aes.Key = KEY;
                using MemoryStream ms = new();
                ms.Write(aes.IV);
                using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write, true))
                {
                    cs.Write(Encoding.UTF8.GetBytes(text));
                }
                return Convert.ToBase64String(ms.ToArray());
            }
            else
                return null;
        }

        public static string? Decrypt(string? base64) //расшифровать файл
        {
            if (base64 != null)
            {
                using MemoryStream ms = new(Convert.FromBase64String(base64));
                byte[] iv = new byte[16];
                ms.Read(iv);
                using Aes aes = Aes.Create();
                aes.Key = KEY;
                aes.IV = iv;
                using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read, true);
                using MemoryStream output = new();
                cs.CopyTo(output);
                return Encoding.UTF8.GetString(output.ToArray());
            }
            else
                return null;
        }
    }
}
