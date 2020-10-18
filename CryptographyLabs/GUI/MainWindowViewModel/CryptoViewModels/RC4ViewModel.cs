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
    class RC4ViewModel : BaseViewModel
    {
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
                dialog.Filters.Add(new CommonFileDialogFilter("Any file", "*"));
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    Filename = dialog.FileName;
            }
        }

        private void Go()
        {
            byte[] keyBytes;
            if (!Extended.TryParse(Key, out keyBytes))
            {
                MessageBox.Show("Wrong key format.", "Input error");
                return;
            }
            if (keyBytes.Length < 1 || keyBytes.Length > 256)
            {
                MessageBox.Show("Size of key must be 1 to 256 bytes.", "Input error");
                return;
            }

            string filename = Filename;
            bool isDeleteAfter = IsDeleteFileAfter;

            var viewModel = new CryptoProgressViewModel
            {
                CryptoName = "RC4",
                Filename = filename
            };
            AddCryptoProgressVM?.Invoke(viewModel);

            string destFilename;
            if (filename.EndsWith(".rc4399"))
                destFilename = filename.Substring(0, filename.Length - 7);
            else
                destFilename = filename + ".rc4399";

            viewModel.StatusString = "Crypting";
            Task task0 = RC4.CryptFileAsync(filename, destFilename, keyBytes, 
                progress => viewModel.CryptoProgress = progress);

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
