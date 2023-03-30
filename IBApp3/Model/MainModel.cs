using IBApp3.Model;
using IBApp3.View;
using Microsoft.Win32;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IBApp3
{
    internal class MainModel : BindableBase
    {
        public string? Content; //расшифрованный текст

        private void ReadFile(string key) //считать файл
        {
            var _path = GetFilePath();
            if (_path != null)
                using (StreamReader reader = new(_path))
                {
                    Content = CryptoClass.Decrypt(reader.ReadToEnd(), string.Concat(Enumerable.Repeat(key, 4)));
                    RaisePropertyChanged("DecryptedContent");
                    _path = null;
                }
        }

        private void WriteFile(string key) //записать файл
        {
            var _path = GetFilePath();
            if (_path != null && Content != null)
            {
                using (StreamWriter writer = new(_path, false))
                {
                    writer.WriteLine(CryptoClass.Encrypt(Content, string.Concat(Enumerable.Repeat(key, 4))));
                }
                Content = null;
                RaisePropertyChanged("DecryptedContent");
            }
        }

        public void DecryptFile() //расшифровать файл
        {
            var key = GetKey();
            if (key != null)
            {
                ReadFile(key);
                MessageBox.Show("Файл расшифрован!");
            }
        }

        private string? GetKey() //получить ключ шифрования
        {
            PasswordWindow passwordWindow = new();
            string? password = null;
            passwordWindow.OKButton.Click += (sender, eventArgs) =>
            {
                if (passwordWindow.PasswordBox.Password.Length != 8)
                    MessageBox.Show("Введите 8 символов!");
                else
                    password = passwordWindow.PasswordBox.Password;
                passwordWindow.Close();
            };
            passwordWindow.ShowDialog();
            return password;
        }

        public void EncryptFile() //зашифровать файл
        {
            string? key = GetKey();
            if (key != null)
            {
                WriteFile(key);
                MessageBox.Show("Файл сохранен");
            }
        }

        public string? GetFilePath() //выбрать файл
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
            };

            return (bool)openFileDialog.ShowDialog() ? openFileDialog.FileName : null;
        }
    }
}
