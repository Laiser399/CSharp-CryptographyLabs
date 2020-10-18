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

        private VernamViewModel _vernamVM;
        public VernamViewModel VernamVM => 
            _vernamVM ?? (_vernamVM = new VernamViewModel());

        private DESViewModel _desVM;
        public DESViewModel DESVM => _desVM ?? (_desVM = new DESViewModel());

        private RC4ViewModel _rc4VM;
        public RC4ViewModel RC4VM => _rc4VM ?? (_rc4VM = new RC4ViewModel());

        private ObservableCollection<CryptoProgressViewModel> _progressViewModels;
        public ObservableCollection<CryptoProgressViewModel> ProgressViewModels =>
            _progressViewModels ?? (_progressViewModels = new ObservableCollection<CryptoProgressViewModel>());

        public MainWindowViewModel()
        {
            VernamVM.AddCryptoProgressVM = AddCryptoProgressVM;
            DESVM.AddCryptoProgressVM = AddCryptoProgressVM;
            RC4VM.AddCryptoProgressVM = AddCryptoProgressVM;
        }

        private void AddCryptoProgressVM(CryptoProgressViewModel viewModel)
        {
            ProgressViewModels.Add(viewModel);
        }

    }

    
}
