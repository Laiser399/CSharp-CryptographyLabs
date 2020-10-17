using Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class Task5ViewModel : BaseViewModel
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
            if (Extended.TryParse(A, out uint a))
            {
                try
                {
                    Result = Bitops.Task5(a).ToString();
                }
                catch (ArgumentException)
                {
                    Result = "-";
                }
            }
            else
                Result = "-";
        }
    }
}
