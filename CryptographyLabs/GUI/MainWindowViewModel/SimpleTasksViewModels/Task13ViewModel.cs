using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class Task13ViewModel : BaseViewModel
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

        private string _i = "";
        public string I
        {
            get => _i;
            set
            {
                _i = value;
                NotifyPropertyChanged(nameof(I));
                Apply();
            }
        }

        private string _j = "";
        public string J
        {
            get => _j;
            set
            {
                _j = value;
                NotifyPropertyChanged(nameof(J));
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
            if (StringEx.TryParse(A, out uint a) && int.TryParse(I, out int i) && int.TryParse(J, out int j))
            {
                if (i >= 0 && i <= 31 && j >= 0 && j <= 31)
                    Result = "0b" + Convert.ToString(Bitops.SwapBits(a, i, j), 2);
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
