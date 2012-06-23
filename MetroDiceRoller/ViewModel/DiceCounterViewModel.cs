using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Media;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using MetroDiceRoller.Helpers;

namespace MetroDiceRoller.ViewModel
{
    public class DiceCounterViewModel : ViewModelBase
    {
        public DiceCounterViewModel()
        {
            Increment = new RelayCommand<object>(
                (o) => this.DiceCount++,
                (o) => this.DiceCount < maxValue);

            Decrement = new RelayCommand<object>(
                (o) => this.DiceCount--,
                (o) => this.DiceCount > minValue);
        }

        #region Constants

        private const int minValue = 0;
        private const int maxValue = 30;

        #endregion

        #region Logic

        private void AddDice()
        {
            var newDice = new DiceViewModel
            {
                Number = RandomHelpers.Next(1, this.DiceMax + 1),
                SizeRatio = (this.DiceMax + 10) / 20.0,
                Brush = this.Brush
            };

            this.DiceList.Add(newDice);
        }

        private void RemoveFirstDice()
        {
            this.DiceList.RemoveAt(0);
        }

        #endregion

        #region Properties

        public const string NamePropertyName = "Name";

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(NamePropertyName);
            }
        }

        public const string BrushPropertyName = "Brush";

        private SolidColorBrush _brush = null;
        public SolidColorBrush Brush
        {
            get
            {
                return _brush;
            }

            set
            {
                if (_brush == value)
                {
                    return;
                }

                _brush = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(BrushPropertyName);
            }
        }

        public const string DiceCountPropertyName = "DiceCount";

        private int _diceCount = 0;
        public int DiceCount
        {
            get
            {
                return _diceCount;
            }

            set
            {
                if (_diceCount == value || value < minValue || value > maxValue)
                {
                    return;
                }

                // Call methods to add or remove dice depending on the new number of dice.
                for (var index = _diceCount; index != value; index += value > _diceCount ? 1 : -1)
                {
                    if (value > _diceCount)
                    {
                        AddDice();
                    }
                    else
                    {
                        RemoveFirstDice();
                    }
                }

                _diceCount = value;

                // Update the commands on the MainViewModel;
                // Todo: Change this to avoid tight coupling.
                App.ViewModel.Reset.RaiseCanExecuteChanged();
                App.ViewModel.ReRoll.RaiseCanExecuteChanged();

                // Update bindings, no broadcast
                RaisePropertyChanged(DiceCountPropertyName);
            }
        }

        public const string DiceMaxPropertyName = "DiceMax";

        private int _diceMax = 0;
        public int DiceMax
        {
            get
            {
                return _diceMax;
            }

            set
            {
                if (_diceMax == value)
                {
                    return;
                }

                _diceMax = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(DiceMaxPropertyName);
            }
        }

        public const string DiceListPropertyName = "DiceList";

        private ObservableCollection<DiceViewModel> _diceList = new ObservableCollection<DiceViewModel>();
        public ObservableCollection<DiceViewModel> DiceList
        {
            get
            {
                return _diceList;
            }

            set
            {
                if (_diceList == value)
                {
                    return;
                }

                _diceList = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(DiceListPropertyName);
            }
        }

        #endregion

        #region Commands

        public RelayCommand<object> Increment { get; private set; }
        public RelayCommand<object> Decrement { get; private set; }

        #endregion
    }
}
