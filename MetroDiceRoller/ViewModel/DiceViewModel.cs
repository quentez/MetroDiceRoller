using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Windows.UI.Xaml.Media;

namespace MetroDiceRoller.ViewModel
{
    public class DiceViewModel : ViewModelBase
    {
        #region Attached properties

        public const string NumberPropertyName = "Number";

        private int _number = 0;
        public int Number
        {
            get
            {
                return _number;
            }

            set
            {
                if (_number == value)
                {
                    return;
                }

                _number = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(NumberPropertyName);
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

        public const string SizeRatioPropertyName = "SizeRatio";

        private double _sizeRatio = 1;
        public double SizeRatio
        {
            get
            {
                return _sizeRatio;
            }

            set
            {
                if (_sizeRatio == value)
                {
                    return;
                }

                _sizeRatio = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(SizeRatioPropertyName);
            }
        }

        #endregion
    }
}
