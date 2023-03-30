using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IBApp3.Model
{
    public static class CryptoClass
    {
        public static string? Encrypt(string? text, string key) //зашифровать файл
        {
            if (text != null)
            {
                using (Aes aesAlgorithm = Aes.Create())
                {
                    aesAlgorithm.Key = Encoding.UTF8.GetBytes(key);

                    byte[] encryptedData;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aesAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(text);
                            }
                            encryptedData = ms.ToArray();
                        }
                    }

                    return Convert.ToBase64String(encryptedData)+"-"+Convert.ToBase64String(aesAlgorithm.IV);
                }
            }
            else
                return null;
        }

        public static string? Decrypt(string? base64, string key) //расшифровать файл
        {
            if (base64 != null)
            {
                using (Aes aesAlgorithm = Aes.Create())
                {
                    var splitted = base64.Substring(0, base64.Length - 2).Split("-");

                    aesAlgorithm.Key = Encoding.UTF8.GetBytes(key);
                    aesAlgorithm.IV = Convert.FromBase64String(splitted[1]);
                    byte[] cipher = Convert.FromBase64String(splitted[0]);

                    using (MemoryStream ms = new MemoryStream(cipher))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aesAlgorithm.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            else
                return null;
        }
    }
}
