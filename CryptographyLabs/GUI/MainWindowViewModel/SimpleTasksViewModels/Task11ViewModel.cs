using Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class Task11ViewModel : BaseViewModel
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
            if (Extended.TryParse(A, out uint a) && int.TryParse(K, out int k))
            {
                if (k >= 0 && k <= 31)
                    Result = Bitops.GetKthBit(a, k).ToString();
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
