using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml.Media;

namespace MetroDiceRoller.ViewModel
{
    public class DiceCounterViewModel : ViewModelBase
    {
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

                var oldValue = _name;
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

                var oldValue = _brush;
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
                if (_diceCount == value || value < 0 || value > 30)
                {
                    return;
                }

                var oldValue = _diceCount;
                _diceCount = value;

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

                var oldValue = _diceMax;
                _diceMax = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(DiceMaxPropertyName);
            }
        }
    }
}
