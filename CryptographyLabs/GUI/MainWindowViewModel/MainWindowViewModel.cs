using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class MainWindowViewModel : BaseViewModel
    {
        // TODO all result textBoxes IsReadOnly=true
        private Task11ViewModel _task11VM;
        public Task11ViewModel Task11VM => _task11VM ?? (_task11VM = new Task11ViewModel());

        private Task12ViewModel _task12VM;
        public Task12ViewModel Task12VM => _task12VM ?? (_task12VM = new Task12ViewModel());

        private Task13ViewModel _task13VM;
        public Task13ViewModel Task13VM => _task13VM ?? (_task13VM = new Task13ViewModel());

        private Task14ViewModel _task14VM;
        public Task14ViewModel Task14VM => _task14VM ?? (_task14VM = new Task14ViewModel());

        private Task21ViewModel _task21VM;
        public Task21ViewModel Task21VM => _task21VM ?? (_task21VM = new Task21ViewModel());

        private Task22ViewModel _task22VM;
        public Task22ViewModel Task22VM => _task22VM ?? (_task22VM = new Task22ViewModel());

        private Task3ViewModel _task3VM;
        public Task3ViewModel Task3VM => _task3VM ?? (_task3VM = new Task3ViewModel());

        private Task4ViewModel _task4VM;
        public Task4ViewModel Task4VM => _task4VM ?? (_task4VM = new Task4ViewModel());

        private Task5ViewModel _task5VM;
        public Task5ViewModel Task5VM => _task5VM ?? (_task5VM = new Task5ViewModel());

        private Task6ViewModel _task6VM;
        public Task6ViewModel Task6VM => _task6VM ?? (_task6VM = new Task6ViewModel());

        private Task7ViewModel _task7VM;
        public Task7ViewModel Task7VM => _task7VM ?? (_task7VM = new Task7ViewModel());

        private Task8ViewModel _task8VM;
        public Task8ViewModel Task8VM => _task8VM ?? (_task8VM = new Task8ViewModel());



        private ObservableCollection<CryptoItemViewModel> _items;
        public ObservableCollection<CryptoItemViewModel> Items =>
            _items ?? (_items = new ObservableCollection<CryptoItemViewModel>());

        private ObservableCollection<string> _col1, _col2;
        public ObservableCollection<string> Col1 => _col1 ?? (_col1 = new ObservableCollection<string>());
        public ObservableCollection<string> Col2 => _col2 ?? (_col2 = new ObservableCollection<string>());


        public MainWindowViewModel()
        {
            Random random = new Random(123);
            for (int i = 0; i < 40; ++i)
                Items.Add(new CryptoItemViewModel { Filename=$"/{random.Next()}", CryptoProgress = random.NextDouble() * 100 });

            Col1.Add("col1");
            Col2.Add("col2");
        }

    }

    class CryptoItemViewModel : BaseViewModel
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


    }
}
