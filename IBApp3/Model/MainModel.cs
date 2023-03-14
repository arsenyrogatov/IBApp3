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
        private const string DecryptPassword = "qwerty";

        public string? Content;
        private string? _path;

        private void ReadFile(string key)
        {
            if (_path != null)
                using (StreamReader reader = new (_path))
                {
                    Content = CryptoClass.Decrypt(reader.ReadToEnd(), String.Concat(Enumerable.Repeat(key, 4)));
                    MessageBox.Show($"'{Content}'");
                    RaisePropertyChanged("DecryptedContent");
                }
        }

        private void WriteFile(string key)
        {
            if (_path != null && Content != null)
                using (StreamWriter writer = new(_path, false))
                {
                    writer.WriteLine(CryptoClass.Encrypt(Content, String.Concat(Enumerable.Repeat(key, 4))));
                }
        }

        public void DecryptFile() //расшифровать файл
        {
            SelectFile();

            if (_path != null)
            {
                string? key = GetKey();
                if (key != null)
                {
                    ReadFile(key);
                    MessageBox.Show("Файл расшифрован!");
                }
            }
        }

        private string? GetKey ()
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
            if (_path == null)
                SelectFile();
            string? key = GetKey();
            if (key != null)
            {
                WriteFile(key);
                MessageBox.Show("Файл сохранен");
            }
        }

        public bool SelectFile() //выбрать файл из диалога
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _path = openFileDialog.FileName;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SelectFile(string path) //выбрать файл из драг н дропа
        {
            _path = path;
        }

        
    }
}
