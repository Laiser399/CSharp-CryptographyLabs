using System.Windows;
using CryptographyLabs.Crypto;
using Microsoft.WindowsAPICodePack.Dialogs;

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
            if (!TryGetKeyBytes(out var keyBytes))
            {
                return;
            }

            if (!TryGetTargetFilePath(out var targetFilePath))
            {
                return;
            }

            if (Mode == Rijndael_.Mode.ECB)
            {
                StartEcbTransform(targetFilePath, keyBytes);
                return;
            }

            if (!TryGetInitialVector(out var initialVector))
            {
                return;
            }

            StartTransform(targetFilePath, keyBytes, initialVector);
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

        private bool TryGetTargetFilePath(out string targetFilePath)
        {
            if (!FilePath.EndsWith(".rjn399"))
            {
                MessageBox.Show("Wrong extenstion of encrypted file. Must be \".rjn399\".");

                targetFilePath = string.Empty;
                return false;
            }

            targetFilePath = FilePath[..^7];
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
                transformVM.StartMultiThread(Rijndael_.GetNice(keyBytes, BlockSize, CryptoDirection.Decrypt));
            }
            else
            {
                transformVM.Start(Rijndael_.Get(keyBytes, BlockSize, CryptoDirection.Decrypt));
            }

            _owner.ProgressViewModels.Add(transformVM);
        }

        private void StartTransform(string targetFilePath, byte[] keyBytes, byte[] initialVector)
        {
            var transformVM = CreateTransformVM(targetFilePath);

            transformVM.Start(Rijndael_.Get(keyBytes, BlockSize, initialVector, Mode, CryptoDirection.Decrypt));

            _owner.ProgressViewModels.Add(transformVM);
        }

        private TransformVM CreateTransformVM(string targetFilePath)
        {
            return new TransformVM(IsDeleteAfter, CryptoDirection.Decrypt)
            {
                CryptoName = "Rijndael",
                SourceFilePath = FilePath,
                DestFilePath = targetFilePath
            };
        }
    }
}