using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBApp3
{
    internal class MainViewModel: BindableBase
    {
        readonly MainModel _model = new();
        public MainViewModel()
        {
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            DecryptFile = new DelegateCommand(() => {
                _model.DecryptFile();
            });
            EncryptFile = new DelegateCommand(() => {
                _model.EncryptFile();
            });
        }
        public DelegateCommand DecryptFile { get; }
        public DelegateCommand EncryptFile { get; }
        public string? DecryptedContent 
        {
            get { return _model.Content; }
            set { _model.Content = value; }
        }
    }
}
