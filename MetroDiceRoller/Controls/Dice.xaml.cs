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
    public sealed partial class Dice : UserControl
    {
        public Dice()
        {
            this.InitializeComponent();
        }

        public double RealWidth
        {
            get
            {
                var diceModel = this.DataContext as DiceViewModel;
                return this.Width * (diceModel == null ? 1 : diceModel.SizeRatio);
            }
        }

        public double RealHeight
        {
            get
            {
                var diceModel = this.DataContext as DiceViewModel;
                return this.Height * (diceModel == null ? 1 : diceModel.SizeRatio);
            }
        }
    }
}
