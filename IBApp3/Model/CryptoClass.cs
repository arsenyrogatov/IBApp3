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
        private static readonly byte[] KEY = Enumerable.Range(0, 32).Select(x => (byte)x).ToArray();
        //генерируем ключ (числа от 0 до 31 в виде байт)

        public static string? Encrypt(string? text, string key) //зашифровать файл
        {
            if (text != null)
            {
                using Aes aes = Aes.Create(); //создаем AES
                //aes.Key = KEY; //задаем ключ
                aes.Key = Encoding.UTF8.GetBytes(key); //задаем ключ
                using MemoryStream ms = new();
                ms.Write(aes.IV); //создаем поток для зашифрованных данных
                using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write, true);
                //создаем поток для шифрования
                cs.Write(Encoding.UTF8.GetBytes(text)); //шифруем данные
                return Convert.ToBase64String(ms.ToArray()); //возвращаем зашифрованную строку
            }
            else
                return null;
        }

        public static string? Decrypt(string? base64, string key) //расшифровать файл
        {
            if (base64 != null)
            {
                using MemoryStream ms = new(Convert.FromBase64String(base64)); //создаем поток данных
                byte[] iv = new byte[16];
                ms.Read(iv); //считываем вектор инициализации
                using Aes aes = Aes.Create(); //создаем AES
                //aes.Key = KEY; //ключ
                aes.Key = Encoding.UTF8.GetBytes(key); //ключ
                aes.IV = iv; //вектор инициализации
                using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read, true);
                //создаем поток для расшифровки
                using MemoryStream output = new();
                cs.CopyTo(output); //расшифровываем данные
                return Encoding.UTF8.GetString(output.ToArray()); //возвращаем расшифрованную строку
            }
            else
                return null;
        }
    }
}
