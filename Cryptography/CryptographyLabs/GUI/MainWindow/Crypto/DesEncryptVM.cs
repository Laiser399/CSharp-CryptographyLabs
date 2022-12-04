using System.Windows;
using CryptographyLabs.Crypto;
using CryptographyLabs.Helpers;
using Microsoft.WindowsAPICodePack.Dialogs;
using Module.Core.Enums;
using Module.Core.Factories.Abstract;
using Module.DES.Entities;
using Module.DES.Entities.Abstract;

namespace CryptographyLabs.GUI
{
    public class DesEncryptVM : BaseViewModel
    {
        public DesVM DesVM { get; }

        private readonly MainWindowVM _owner;
        private readonly ICryptoTransformFactory<IDesParameters> _desCryptoTransformFactory;

        public DesEncryptVM(
            DesVM desVM,
            MainWindowVM owner,
            ICryptoTransformFactory<IDesParameters> desCryptoTransformFactory)
        {
            DesVM = desVM;
            _owner = owner;
            _desCryptoTransformFactory = desCryptoTransformFactory;
        }

        #region Bindings

        private string _filenameToEncrypt = "";

        public string FilenameToEncrypt
        {
            get => _filenameToEncrypt;
            set
            {
                _filenameToEncrypt = value;
                NotifyPropChanged(nameof(FilenameToEncrypt));
            }
        }

        private RelayCommand _changeFilenameCmd;

        public RelayCommand ChangeFilenameCmd =>
            _changeFilenameCmd ?? (_changeFilenameCmd = new RelayCommand(_ => ChangeFilename()));

        private RelayCommand _goEncryptCmd;

        public RelayCommand GoEncryptCmd
            => _goEncryptCmd ?? (_goEncryptCmd = new RelayCommand(_ => GoEncrypt()));

        #endregion

        private void ChangeFilename()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Filters.Add(new CommonFileDialogFilter("Any file", "*"));
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    FilenameToEncrypt = dialog.FileName;
            }
        }

        private void GoEncrypt()
        {
            if (!TryGetKey(out var key56))
            {
                return;
            }

            var sourceFilePath = FilenameToEncrypt;
            var targetFilePath = sourceFilePath + ".des399";

            if (DesVM.Mode == DES_.Mode.ECB)
            {
                StartEcbTransform(sourceFilePath, targetFilePath, key56);
            }
            else
            {
                if (!TryGetInitialVector(out var initialVector))
                {
                    return;
                }

                StartTransform(sourceFilePath, targetFilePath, key56, initialVector);
            }
        }


        private bool TryGetKey(out ulong key56)
        {
            if (!StringEx.TryParse(DesVM.Key, out key56))
            {
                MessageBox.Show("Wrong key format.", "Error");
                return false;
            }

            return true;
        }

        private bool TryGetInitialVector(out byte[] initialVector)
        {
            if (!StringEx.TryParse(DesVM.IV, out initialVector))
            {
                MessageBox.Show("Wrong IV format.");
                return false;
            }

            if (initialVector.Length != DES_.BlockSize)
            {
                MessageBox.Show($"Wrong IV bytes count. Must be {DES_.BlockSize}.");
                return false;
            }

            return true;
        }

        private void StartEcbTransform(string sourceFilePath, string targetFilePath, ulong key56)
        {
            var transformVM = CrateTransformVM(sourceFilePath, targetFilePath);

            var cryptoTransform = _desCryptoTransformFactory.CreateEcb(
                TransformDirection.Encrypt,
                new DesParameters(key56),
                DesVM.Multithreading
            );

            transformVM.Start(cryptoTransform);

            _owner.ProgressViewModels.Add(transformVM);
        }

        private void StartTransform(string sourceFilePath, string targetFilePath, ulong key56, byte[] initialVector)
        {
            var transformVM = CrateTransformVM(sourceFilePath, targetFilePath);

            var cryptoTransform = _desCryptoTransformFactory.Create(
                TransformDirection.Encrypt,
                new DesParameters(key56),
                LegacyCodeHelper.Fix(DesVM.Mode),
                initialVector
            );

            transformVM.Start(cryptoTransform);

            _owner.ProgressViewModels.Add(transformVM);
        }

        private TransformVM CrateTransformVM(string sourceFilePath, string targetFilePath)
        {
            return new TransformVM(DesVM.IsDeleteFileAfter, CryptoDirection.Encrypt)
            {
                CryptoName = "DES",
                SourceFilePath = sourceFilePath,
                DestFilePath = targetFilePath,
            };
        }
    }
}