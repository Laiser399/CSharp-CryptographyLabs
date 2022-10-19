using System.Windows;
using CryptographyLabs.Crypto;
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
            if (!TryGetKeyBytes(out var keyBytes))
            {
                return;
            }

            var targetFilePath = FilePath + ".rjn399";

            if (Mode == Rijndael_.Mode.ECB)
            {
                StartEcbTransform(targetFilePath, keyBytes);
            }
            else
            {
                if (!TryGetInitialVector(out var initialVector))
                {
                    return;
                }

                StartTransform(targetFilePath, keyBytes, initialVector);
            }
        }

        private bool TryGetKeyBytes(out byte[] keyBytes)
        {
            if (!StringEx.TryParse(Key, out keyBytes))
            {
                MessageBox.Show("Wrong key format.");
                return false;
            }

            if (keyBytes.Length != Rijndael_.GetBytesCount(KeySize))
            {
                MessageBox.Show("Wrong bytes count in key.");
                return false;
            }

            return true;
        }

        private bool TryGetInitialVector(out byte[] initialVector)
        {
            if (!StringEx.TryParse(IV, out initialVector))
            {
                MessageBox.Show("Wrong IV format.");
                return false;
            }

            if (initialVector.Length != Rijndael_.GetBytesCount(BlockSize))
            {
                MessageBox.Show($"Wrong IV bytes count. Must be {Rijndael_.GetBytesCount(BlockSize)}.");
                return false;
            }

            return true;
        }

        private void StartEcbTransform(string targetFilePath, byte[] keyBytes)
        {
            var transformVM = CreateTransformVM(targetFilePath);

            if (Multithread)
            {
                transformVM.StartMultiThread(Rijndael_.GetNice(keyBytes, BlockSize, CryptoDirection.Encrypt));
            }
            else
            {
                transformVM.Start(Rijndael_.Get(keyBytes, BlockSize, CryptoDirection.Encrypt));
            }

            _owner.ProgressViewModels.Add(transformVM);
        }

        private void StartTransform(string targetFilePath, byte[] keyBytes, byte[] initialVector)
        {
            var transformVM = CreateTransformVM(targetFilePath);

            transformVM.Start(Rijndael_.Get(keyBytes, BlockSize, initialVector, Mode, CryptoDirection.Encrypt));

            _owner.ProgressViewModels.Add(transformVM);
        }

        private TransformVM CreateTransformVM(string targetFilePath)
        {
            return new TransformVM(IsDeleteAfter, CryptoDirection.Encrypt)
            {
                CryptoName = "Rijndael",
                SourceFilePath = FilePath,
                DestFilePath = targetFilePath
            };
        }
    }
}