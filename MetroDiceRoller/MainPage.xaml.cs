using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using MetroDiceRoller.Helpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MetroDiceRoller
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            MainDiceMenu.DataContext = new List<dynamic>
            {
                new {
                    CounterName = "D4",
                    Color = new SolidColorBrush(Colors.Black)
                },
                new {
                    CounterName = "D6",
                    Color = new SolidColorBrush(ColorHelpers.ColorFromHexString("#2B2B2B"))
                },
                new {
                    CounterName = "D8",
                    Color = new SolidColorBrush(ColorHelpers.ColorFromHexString("#5C5C5C"))
                },
                new {
                    CounterName = "D10",
                    Color = new SolidColorBrush(ColorHelpers.ColorFromHexString("#9D1414"))
                },
                new {
                    CounterName = "D20",
                    Color = new SolidColorBrush(ColorHelpers.ColorFromHexString("#DC0000"))
                }
            };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
