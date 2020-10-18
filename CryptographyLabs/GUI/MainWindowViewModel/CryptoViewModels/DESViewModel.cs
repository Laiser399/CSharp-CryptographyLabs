using Crypto;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CryptographyLabs.GUI
{
    class DESViewModel : BaseViewModel
    {
        private bool _isEncrypt = true;
        public bool IsEncrypt
        {
            get => _isEncrypt;
            set
            {
                _isEncrypt = value;
                NotifyPropertyChanged(nameof(IsEncrypt));
            }
        }

        private DES.Mode _mode = DES.Mode.ECB;
        public int ModeIndex
        {
            get => (int)_mode;
            set
            {
                _mode = (DES.Mode)value;
                NotifyPropertyChanged(nameof(ModeIndex));
            }
        }

        private string _filename = "";
        public string Filename
        {
            get => _filename;
            set
            {
                _filename = value;
                NotifyPropertyChanged(nameof(Filename));
            }
        }

        private string _key = "";
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                NotifyPropertyChanged(nameof(Key));
            }
        }

        private bool _isDeleteFileAfter = false;
        public bool IsDeleteFileAfter
        {
            get => _isDeleteFileAfter;
            set
            {
                _isDeleteFileAfter = value;
                NotifyPropertyChanged(nameof(IsDeleteFileAfter));
            }
        }

        public Action<CryptoProgressViewModel> AddCryptoProgressVM;

        private RelayCommand _changeFilenameCommand;
        public RelayCommand ChangeFilenameCommand =>
            _changeFilenameCommand ?? (_changeFilenameCommand = new RelayCommand(_ => ChangeFilename()));

        private RelayCommand _goCommand;
        public RelayCommand GoCommand =>
            _goCommand ?? (_goCommand = new RelayCommand(_ => Go()));

        private void ChangeFilename()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                if (!IsEncrypt)
                    dialog.Filters.Add(new CommonFileDialogFilter("Encrypted file", ".des399"));
                dialog.Filters.Add(new CommonFileDialogFilter("Any file", "*"));
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    Filename = dialog.FileName;
            }
        }

        private void Go()
        {
            ulong key56;
            if (!Extended.TryParse(Key, out key56))
            {
                MessageBox.Show("Wrong key format.", "Error");
                return;
            }

            string filename = Filename;
            bool isDeleteAfter = IsDeleteFileAfter;

            var viewModel = new CryptoProgressViewModel
            {
                CryptoName = "DES",
                Filename = filename
            };
            AddCryptoProgressVM?.Invoke(viewModel);

            Task task0;
            if (IsEncrypt)
            {
                viewModel.StatusString = "Encrypting";
                task0 = DES.EncryptFileAsync(filename, key56, _mode, progress => viewModel.CryptoProgress = progress);
            }
            else
            {
                viewModel.StatusString = "Decrypting";
                task0 = DES.DecryptFileAsync(filename, key56, progress => viewModel.CryptoProgress = progress);
            }

            task0.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    viewModel.StatusString = "Error: " + task.Exception.InnerException.Message;
                }
                else if (isDeleteAfter)
                {
                    viewModel.StatusString = "Deleting file";
                    try
                    {
                        File.Delete(filename);
                        viewModel.StatusString = "Done successfully";
                    }
                    catch (Exception e)
                    {
                        viewModel.StatusString = "Error: " + e.Message;
                    }
                }
                else
                    viewModel.StatusString = "Done successfully";

                viewModel.IsDone = true;
            });
        }

    }
}
