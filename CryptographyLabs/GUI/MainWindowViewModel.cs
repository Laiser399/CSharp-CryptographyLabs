using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class MainWindowViewModel : BaseViewModel
    {
        // for example
        private string _someProp = "aga";
        public string SomeProp
        {
            get => _someProp;
            set
            {
                _someProp = value;
                NotifyPropertyChanged(nameof(SomeProp));
                Console.WriteLine(value);
            }
        }

        // for example
        private RelayCommand _someCommand;
        public RelayCommand SomeCommand
        {
            get =>
                _someCommand ?? (_someCommand = new RelayCommand(_ => DoSomething()));
        }
        private void DoSomething()
        {

        }


    }
}
