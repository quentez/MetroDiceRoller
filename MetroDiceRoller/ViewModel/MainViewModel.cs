using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace MetroDiceRoller.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public const string CountersPropertyName = "Counters";

        private ObservableCollection<DiceCounterViewModel> _counters = new ObservableCollection<DiceCounterViewModel>();
        public ObservableCollection<DiceCounterViewModel> Counters
        {
            get
            {
                return _counters;
            }

            set
            {
                if (_counters == value)
                {
                    return;
                }

                var oldValue = _counters;
                _counters = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(CountersPropertyName);
            }
        }
    }
}
