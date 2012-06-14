using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using MetroDiceRoller.ViewModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MetroDiceRoller
{
    public sealed partial class DiceCounter : UserControl
    {
        public DiceCounter()
        {
            this.InitializeComponent();
        }

        #region Logic method

        private void DiceCountUpdate(int offset)
        {
            // Retrieve the ViewModel for this DiceCounter.
            var diceCounterViewModel = this.DataContext as DiceCounterViewModel;
            if (diceCounterViewModel == null)
            {
                return;
            }

            diceCounterViewModel.DiceCount += offset;
        }

        #endregion

        #region Event handlers

        private void TapGridPlus_Released(object sender, PointerRoutedEventArgs e)
        {
            DiceCountUpdate(1);
        }

        private void TapGridLess_Released(object sender, PointerRoutedEventArgs e)
        {
            DiceCountUpdate(-1);
        }

        #endregion
    }
}
