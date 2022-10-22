using System.Windows;
using CryptographyLabs.Crypto;
using CryptographyLabs.Helpers;
using Microsoft.WindowsAPICodePack.Dialogs;
using Module.Core.Enums;
using Module.Rijndael.Entities;
using IRijndaelCryptoTransformFactory = Module.Core.Factories.Abstract.ICryptoTransformFactory<
    Module.Rijndael.Entities.Abstract.IRijndaelBlockCryptoTransformParameters
>;

namespace CryptographyLabs.GUI
{
    class RijndaelDecryptVM : RijndaelVM
    {
        private readonly MainWindowVM _owner;
        private readonly IRijndaelCryptoTransformFactory _rijndaelCryptoTransformFactory;

        public RijndaelDecryptVM(
            MainWindowVM owner,
            IRijndaelCryptoTransformFactory rijndaelCryptoTransformFactory)
        {
            _owner = owner;
            _rijndaelCryptoTransformFactory = rijndaelCryptoTransformFactory;
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

            var cryptoTransform = _rijndaelCryptoTransformFactory.CreateEcb(
                TransformDirection.Decrypt,
                new RijndaelBlockCryptoTransformParameters(keyBytes, LegacyCodeHelper.Fix(BlockSize)),
                Multithread
            );

            transformVM.Start(cryptoTransform);

            _owner.ProgressViewModels.Add(transformVM);
        }

        private void StartTransform(string targetFilePath, byte[] keyBytes, byte[] initialVector)
        {
            var transformVM = CreateTransformVM(targetFilePath);

            var cryptoTransform = _rijndaelCryptoTransformFactory.Create(
                TransformDirection.Decrypt,
                new RijndaelBlockCryptoTransformParameters(keyBytes, LegacyCodeHelper.Fix(BlockSize)),
                LegacyCodeHelper.Fix(Mode),
                initialVector
            );

            transformVM.Start(cryptoTransform);

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