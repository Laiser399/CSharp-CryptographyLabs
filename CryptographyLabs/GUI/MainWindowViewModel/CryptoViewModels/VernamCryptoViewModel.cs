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
    class VernamCryptoViewModel : BaseViewModel
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

        private ObservableCollection<CryptoProgressViewModel> _progressViewModels;
        public ObservableCollection<CryptoProgressViewModel> ProgressViewModels =>
            _progressViewModels ?? (_progressViewModels = new ObservableCollection<CryptoProgressViewModel>());

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
            var viewModel = new CryptoProgressViewModel
            {
                CryptoName = "Vernam",
                Filename = Filename
            };
            ProgressViewModels.Add(viewModel);

            Task task0;
            if (IsEncrypt)
            {
                task0 = Vernam.EncryptFileAsync(Filename, progress => viewModel.CryptoProgress = progress);
                viewModel.StatusString = "Encrypting";
            }
            else
            {
                task0 = Vernam.DecryptFileAsync(Filename, KeyFilename,
                    progress => viewModel.CryptoProgress = progress);
                viewModel.StatusString = "Decrypting";
            }
            

            task0.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    viewModel.StatusString = "Error: " + task.Exception.InnerException.Message;
                }
                else if (IsDeleteFileAfter)
                {
                    viewModel.StatusString = "Deleting file";
                    try
                    {
                        File.Delete(Filename);
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
