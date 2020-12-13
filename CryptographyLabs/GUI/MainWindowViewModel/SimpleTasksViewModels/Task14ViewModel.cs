using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class Task14ViewModel : BaseViewModel
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

        private string _m = "";
        public string M
        {
            get => _m;
            set
            {
                _m = value;
                NotifyPropertyChanged(nameof(M));
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
            if (StringEx.TryParse(A, out uint a) && int.TryParse(M, out int m))
            {
                if (m >= 0 && m <= 32)
                    Result = "0b" + Convert.ToString(Bitops.NullifyMLowBits(a, m), 2);
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
