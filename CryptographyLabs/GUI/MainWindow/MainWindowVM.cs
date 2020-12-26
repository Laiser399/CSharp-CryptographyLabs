using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.ComponentModel;

namespace CryptographyLabs.GUI
{
    // TODO multithreading ECB (gui)
    // TODO IV for coupling
    // TODO possibility input key and IV from file
    class MainWindowVM : BaseViewModel
    {
        #region Bindings

        private VernamVM _vernamVM;
        public VernamVM VernamVM => 
            _vernamVM ?? (_vernamVM = new VernamVM(this));

        private DesVM _desVM;
        public DesVM DESVM => _desVM ?? (_desVM = new DesVM(this));

        private RC4VM _rc4VM;
        public RC4VM RC4VM => _rc4VM ?? (_rc4VM = new RC4VM(this));

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

        private ObservableCollection<BaseTransformVM> _progressViewModels;
        public ObservableCollection<BaseTransformVM> ProgressViewModels =>
            _progressViewModels ?? (_progressViewModels = new ObservableCollection<BaseTransformVM>());

        #endregion

        public MainWindowVM()
        {
            _rijndaelEncryptVM = new RijndaelEncryptVM(this);
            _rijndaelDecryptVM = new RijndaelDecryptVM(this);

            UpdateRijndaelVM();
        }

        private void UpdateRijndaelVM()
        {
            if (_rijndaelIsEncrypt)
                RijndaelVM = _rijndaelEncryptVM;
            else
                RijndaelVM = _rijndaelDecryptVM;
        }
    }


}
