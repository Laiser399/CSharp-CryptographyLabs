using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptographyLabs.Crypto;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace CryptographyLabs.GUI
{
    class RijndaelEncryptVM : RijndaelVM
    {
        private MainWindowVM _owner;

        public RijndaelEncryptVM(MainWindowVM owner)
        {
            _owner = owner;
        }

        protected override void ChangeFilePath()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Filters.Add(new CommonFileDialogFilter("Any file", "*"));
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    FilePath = dialog.FileName;
            }
        }

        protected override void Go()
        {
            if (!StringEx.TryParse(Key, out byte[] keyBytes))
            {
                MessageBox.Show("Wrong key format.");
                return;
            }
            if (keyBytes.Length != Rijndael_.GetBytesCount(KeySize))
            {
                MessageBox.Show("Wrong bytes count in key.");
                return;
            }

            string encryptPath = FilePath + ".rjn399";
            var vm = new RijndaelEncryptTransformVM(FilePath, encryptPath, keyBytes, BlockSize, IsDeleteAfter);
            _owner.ProgressViewModels.Add(vm);
        }
    }
}
