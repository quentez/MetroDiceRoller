using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MetroDiceRoller.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            // Reset all the counters.
            Reset = new RelayCommand<object>(
                (o) =>
                {
                    foreach (var diceCounter in this.Counters)
                    {
                        diceCounter.DiceCount = 0;
                    }
                },
                (o) =>
                {
                    var totalCount = 0;

                    foreach (var diceCounter in this.Counters)
                    {
                        totalCount += diceCounter.DiceCount;
                    }

                    return (totalCount != 0);
                });

            // Reroll all the dice.
            ReRoll = new RelayCommand<object>(
                (o) =>
                {
                    foreach (var diceCounter in this.Counters)
                    {
                        var diceCount = diceCounter.DiceCount;
                        diceCounter.DiceCount = 0;
                        diceCounter.DiceCount = diceCount;
                    }
                },
                (o) =>
                {
                    var totalCount = 0;

                    foreach (var diceCounter in this.Counters)
                    {
                        totalCount += diceCounter.DiceCount;
                    }

                    return (totalCount != 0);
                });
        }

        #region Properties

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

        #endregion

        #region Commands

        public RelayCommand<object> Reset { get; private set; }
        public RelayCommand<object> ReRoll { get; private set; }

        #endregion
    }
}
