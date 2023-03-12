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

        private string? _content;
        public string? Content 
        { 
            get { return _content; } 
            set { _content = value; } 
        }
        private string? _path;

        private void ReadFile()
        {
            if (_path != null)
                using (StreamReader reader = new (_path))
                {
                    _content = CryptoClass.Decrypt(reader.ReadToEnd());
                    RaisePropertyChanged("DecryptedContent");
                }
        }

        private void WriteFile()
        {
            if (_path != null && _content != null)
                using (StreamWriter writer = new(_path, false))
                {
                    writer.WriteLine(CryptoClass.Encrypt(_content));
                }
        }

        public void DecryptFile() //расшифровать файл
        {
            if (_path == null)
                SelectFile();

            if (_path != null)
            {
                PasswordWindow passwordWindow = new();
                passwordWindow.OKButton.Click += (sender, eventArgs) =>
                {
                    if (passwordWindow.PasswordBox.Password == DecryptPassword)
                        ReadFile();
                    else
                        MessageBox.Show("Неправильный пароль");
                    passwordWindow.Close();
                };
                passwordWindow.ShowDialog();
            }
        }

        public void EncryptFile() //зашифровать файл
        {
            WriteFile();
            MessageBox.Show("Файл сохранен");
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
