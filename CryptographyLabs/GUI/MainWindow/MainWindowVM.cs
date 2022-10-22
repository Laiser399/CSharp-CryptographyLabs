using System.Collections.ObjectModel;
using CryptographyLabs.GUI.AbstractViewModels;
using IRijndaelCryptoTransformFactory = Module.Core.Factories.Abstract.ICryptoTransformFactory<
    Module.Rijndael.Entities.Abstract.IRijndaelParameters
>;

namespace CryptographyLabs.GUI
{
    public class MainWindowVM : BaseViewModel
    {
        #region Bindings

        // Vernam
        private VernamVM _vernamVM;
        public VernamVM VernamVM => 
            _vernamVM ?? (_vernamVM = new VernamVM(this));

        // DES
        private DesEncryptVM _desEncryptVM;
        private DesDecryptVM _desDecryptVM;

        private bool _desIsEncrypt = true;
        public bool DesIsEncrypt
        {
            get => _desIsEncrypt;
            set
            {
                if (value == _desIsEncrypt)
                    return;
                _desIsEncrypt = value;
                NotifyPropChanged(nameof(DesIsEncrypt));
                UpdateDesVM();
            }
        }

        private BaseViewModel _desVM;
        public BaseViewModel DesVM
        {
            get => _desVM;
            set
            {
                _desVM = value;
                NotifyPropChanged(nameof(DesVM));
            }
        }

        // RC4
        private RC4VM _rc4VM;
        public RC4VM RC4VM => _rc4VM ?? (_rc4VM = new RC4VM(this));

        // RSA
        public IPrimesGenerationVM PrimesGeneration { get; }
        public IRSAKeyGenerationVM RSAKeyGeneration { get; }
        public IRSATransformationVM RSATransformation { get; }
        public IRSAAttackVM RSAAttack { get; }
        
        // Rijndael
        private RijndaelEncryptVM _rijndaelEncryptVM;
        private RijndaelDecryptVM _rijndaelDecryptVM;

        private bool _rijndaelIsEncrypt = true;
        public bool RijndaelIsEncrypt
        {
            get => _rijndaelIsEncrypt;
            set
            {
                if (value == _rijndaelIsEncrypt)
                    return;
                _rijndaelIsEncrypt = value;
                NotifyPropChanged(nameof(RijndaelIsEncrypt));
                UpdateRijndaelVM();
            }
        }

        private BaseViewModel _rijndaelVM;
        public BaseViewModel RijndaelVM
        {
            get => _rijndaelVM;
            set
            {
                _rijndaelVM = value;
                NotifyPropChanged(nameof(RijndaelVM));
            }
        }

        // FROG
        private FrogVM _frogVM;
        public FrogVM FrogVM => _frogVM;

        // 
        private ObservableCollection<TransformVM> _progressViewModels;
        public ObservableCollection<TransformVM> ProgressViewModels =>
            _progressViewModels ?? (_progressViewModels = new ObservableCollection<TransformVM>());

        #endregion

        public MainWindowVM(
            IPrimesGenerationVM primesGenerationVM,
            IRSAKeyGenerationVM rsaKeyGenerationVM,
            IRSATransformationVM rsaTransformation,
            IRSAAttackVM rsaAttackVM,
            IRijndaelCryptoTransformFactory rijndaelCryptoTransformFactory)
        {
            PrimesGeneration = primesGenerationVM;
            RSAKeyGeneration = rsaKeyGenerationVM;
            RSATransformation = rsaTransformation;
            RSAAttack = rsaAttackVM;

            _rijndaelEncryptVM = new RijndaelEncryptVM(this, rijndaelCryptoTransformFactory);
            _rijndaelDecryptVM = new RijndaelDecryptVM(this, rijndaelCryptoTransformFactory);

            var desVM = new DesVM();
            _desEncryptVM = new DesEncryptVM(desVM, this);
            _desDecryptVM = new DesDecryptVM(desVM, this);

            _frogVM = new FrogVM(this);

            UpdateRijndaelVM();
            UpdateDesVM();
        }

        private void UpdateRijndaelVM()
        {
            if (_rijndaelIsEncrypt)
                RijndaelVM = _rijndaelEncryptVM;
            else
                RijndaelVM = _rijndaelDecryptVM;
        }

        private void UpdateDesVM()
        {
            if (DesIsEncrypt)
                DesVM = _desEncryptVM;
            else
                DesVM = _desDecryptVM;
        }
    }
}