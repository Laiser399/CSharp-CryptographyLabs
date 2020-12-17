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
    class MainWindowVM : BaseViewModel
    {
        #region Bindings

        // TODO all result textBoxes IsReadOnly=true
        private Task11VM _task11VM;
        public Task11VM Task11VM => _task11VM ?? (_task11VM = new Task11VM());

        private Task12VM _task12VM;
        public Task12VM Task12VM => _task12VM ?? (_task12VM = new Task12VM());

        private Task13VM _task13VM;
        public Task13VM Task13VM => _task13VM ?? (_task13VM = new Task13VM());

        private Task14VM _task14VM;
        public Task14VM Task14VM => _task14VM ?? (_task14VM = new Task14VM());

        private Task21VM _task21VM;
        public Task21VM Task21VM => _task21VM ?? (_task21VM = new Task21VM());

        private Task22VM _task22VM;
        public Task22VM Task22VM => _task22VM ?? (_task22VM = new Task22VM());

        private Task3VM _task3VM;
        public Task3VM Task3VM => _task3VM ?? (_task3VM = new Task3VM());

        private Task4VM _task4VM;
        public Task4VM Task4VM => _task4VM ?? (_task4VM = new Task4VM());

        private Task5VM _task5VM;
        public Task5VM Task5VM => _task5VM ?? (_task5VM = new Task5VM());

        private Task6VM _task6VM;
        public Task6VM Task6VM => _task6VM ?? (_task6VM = new Task6VM());

        private Task7VM _task7VM;
        public Task7VM Task7VM => _task7VM ?? (_task7VM = new Task7VM());

        private Task8VM _task8VM;
        public Task8VM Task8VM => _task8VM ?? (_task8VM = new Task8VM());

        private VernamVM _vernamVM;
        public VernamVM VernamVM => 
            _vernamVM ?? (_vernamVM = new VernamVM(this));

        private DesVM _desVM;
        public DesVM DESVM => _desVM ?? (_desVM = new DesVM(this));

        private RC4VM _rc4VM;
        public RC4VM RC4VM => _rc4VM ?? (_rc4VM = new RC4VM(this));

        private ObservableCollection<BaseTransformVM> _progressViewModels;
        public ObservableCollection<BaseTransformVM> ProgressViewModels =>
            _progressViewModels ?? (_progressViewModels = new ObservableCollection<BaseTransformVM>());

        #endregion

    }


}
