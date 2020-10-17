using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class CryptoProgressViewModel : BaseViewModel
    {
        private long _startTime = DateTime.Now.Ticks;
        public long StartTime => _startTime;

        private string _filename = "";
        public string Filename
        {
            get => _filename;
            set
            {
                _filename = value;
                NotifyPropertyChanged(nameof(Filename));
            }
        }

        private string _statusString = "aga";
        public string StatusString
        {
            get => _statusString;
            set
            {
                _statusString = value;
                NotifyPropertyChanged(nameof(StatusString));
            }
        }

        private double _cryptoProgress = 0;
        public double CryptoProgress
        {
            get => _cryptoProgress;
            set
            {
                if (value > 100)
                    _cryptoProgress = 100;
                else if (value < 0)
                    _cryptoProgress = 0;
                else
                    _cryptoProgress = value;
                NotifyPropertyChanged(nameof(CryptoProgress));
            }
        }

        private bool _isDone = false;
        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                NotifyPropertyChanged(nameof(IsDone));
            }
        }

        private string _cryptoName = "";
        public string CryptoName
        {
            get => _cryptoName;
            set
            {
                _cryptoName = value;
                NotifyPropertyChanged(nameof(CryptoName));
            }
        }

    }
}
