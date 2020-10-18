using Crypto;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class VernamViewModel : BaseViewModel
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

        private string _keyFilename = "";
        public string KeyFilename
        {
            get => _keyFilename;
            set
            {
                _keyFilename = value;
                NotifyPropertyChanged(nameof(KeyFilename));
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

        private RelayCommand _changeKeyFilenameCommand;
        public RelayCommand ChangeKeyFilenameCommand =>
            _changeKeyFilenameCommand ?? (_changeKeyFilenameCommand = new RelayCommand(_ => ChangeKeyFilename()));

        private RelayCommand _goCommand;
        public RelayCommand GoCommand =>
            _goCommand ?? (_goCommand = new RelayCommand(_ => Go()));

        private void ChangeFilename()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                if (!IsEncrypt)
                    dialog.Filters.Add(new CommonFileDialogFilter("Encrypted file", ".v399"));
                dialog.Filters.Add(new CommonFileDialogFilter("Any file", "*"));
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    Filename = dialog.FileName;
            }
        }

        private void ChangeKeyFilename()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Filters.Add(new CommonFileDialogFilter("Key file", ".vkey399"));
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    KeyFilename = dialog.FileName;
            }
        }

        private void Go()
        {
            string filename = Filename;
            string keyFilename = KeyFilename;
            bool isDeleteAfter = IsDeleteFileAfter;

            var viewModel = new CryptoProgressViewModel
            {
                CryptoName = "Vernam",
                Filename = filename
            };
            AddCryptoProgressVM?.Invoke(viewModel);

            Task task0;
            if (IsEncrypt)
            {
                viewModel.StatusString = "Encrypting";
                task0 = Vernam.EncryptFileAsync(filename, progress => viewModel.CryptoProgress = progress);
            }
            else
            {
                viewModel.StatusString = "Decrypting";
                task0 = Vernam.DecryptFileAsync(filename, keyFilename,
                    progress => viewModel.CryptoProgress = progress);
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
