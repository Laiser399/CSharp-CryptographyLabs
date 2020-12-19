using CryptographyLabs.Crypto;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CryptographyLabs.GUI
{
    class RijndaelDecryptVM : RijndaelVM
    {
        private MainWindowVM _owner;

        public RijndaelDecryptVM(MainWindowVM owner)
        {
            _owner = owner;
        }

        protected override void ChangeFilePath()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Filters.Add(new CommonFileDialogFilter("Encrypted file", ".rjn399"));
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

            string decryptPath;
            if (FilePath.EndsWith(".rjn399"))
                decryptPath = FilePath.Substring(0, FilePath.Length - 7);
            else
            {
                MessageBox.Show("Wrong extenstion of encrypted file. Must be \".rjn399\".");
                return;
            }

            var vm = new RijndaelDecryptTransformVM(FilePath, decryptPath, keyBytes, BlockSize, IsDeleteAfter);
            _owner.ProgressViewModels.Add(vm);
        }
    }
}
