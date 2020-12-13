using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class Task12ViewModel : BaseViewModel
    {
        private string _a = "";
        public string A
        {
            get => _a;
            set
            {
                _a = value;
                NotifyPropertyChanged(nameof(A));
                Apply();
            }
        }

        private string _k = "";
        public string K
        {
            get => _k;
            set
            {
                _k = value;
                NotifyPropertyChanged(nameof(K));
                Apply();
            }
        }

        private bool _isSet = false;
        public bool IsSet
        {
            get => _isSet;
            set
            {
                _isSet = value;
                NotifyPropertyChanged(nameof(IsSet));
                Apply();
            }
        }

        private string _result = "";
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                NotifyPropertyChanged(nameof(Result));
            }
        }

        private void Apply()
        {
            if (StringEx.TryParse(A, out uint a) && int.TryParse(K, out int k))
            {
                if (k >= 0 && k <= 31)
                    Result = "0b" + Convert.ToString(Bitops.SwitchKthBit(a, k, IsSet), 2);
                else
                    Result = "-";
            }
            else
            {
                Result = "-";
            }
        }

    }
}
