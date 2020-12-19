using CryptographyLabs.Crypto;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CryptographyLabs.GUI
{
    class DesVM : BaseViewModel
    {
        private MainWindowVM _owner;

        #region Bindings

        private bool _isEncrypt = true;
        public bool IsEncrypt
        {
            get => _isEncrypt;
            set
            {
                _isEncrypt = value;
                NotifyPropChanged(nameof(IsEncrypt));
            }
        }

        private DES_.Mode _mode = DES_.Mode.ECB;
        public int ModeIndex
        {
            get => (int)_mode;
            set
            {
                _mode = (DES_.Mode)value;
                NotifyPropChanged(nameof(ModeIndex));
            }
        }

        private string _filename = "";
        public string Filename
        {
            get => _filename;
            set
            {
                _filename = value;
                NotifyPropChanged(nameof(Filename));
            }
        }

        private string _key = "";
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                NotifyPropChanged(nameof(Key));
            }
        }

        private bool _isDeleteFileAfter = false;
        public bool IsDeleteFileAfter
        {
            get => _isDeleteFileAfter;
            set
            {
                _isDeleteFileAfter = value;
                NotifyPropChanged(nameof(IsDeleteFileAfter));
            }
        }

        private RelayCommand _changeFilenameCommand;
        public RelayCommand ChangeFilenameCommand =>
            _changeFilenameCommand ?? (_changeFilenameCommand = new RelayCommand(_ => ChangeFilename()));

        private RelayCommand _goCommand;
        public RelayCommand GoCommand =>
            _goCommand ?? (_goCommand = new RelayCommand(_ => Go()));

        #endregion

        public DesVM(MainWindowVM owner)
        {
            _owner = owner;
        }

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
            if (!StringEx.TryParse(Key, out key56))
            {
                MessageBox.Show("Wrong key format.", "Error");
                return;
            }

            string filePath = Filename;

            if (IsEncrypt)
            {
                string encryptPath = filePath + ".des399";
                var vm = new DESEncryptTransformVM(filePath, encryptPath, key56, _mode, IsDeleteFileAfter);
                _owner.ProgressViewModels.Add(vm);
            }
            else
            {
                string decryptPath;
                if (filePath.EndsWith(".des399"))
                    decryptPath = filePath.Substring(0, filePath.Length - 7);
                else
                {
                    MessageBox.Show("Wrong extension of file.");
                    return;
                }

                var vm = new DESDecryptTransformVM(filePath, decryptPath, key56, _mode, IsDeleteFileAfter);
                _owner.ProgressViewModels.Add(vm);
            }
        }

    }
}
