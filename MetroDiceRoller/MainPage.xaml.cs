using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Devices.Sensors;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

using MetroDiceRoller.Helpers;
using MetroDiceRoller.ViewModel;

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

            // Set event handlers.
            this.Loaded += new RoutedEventHandler(Page_Loaded);
            DisplayProperties.OrientationChanged += DisplayProperties_OrientationChanged;

            // Original values for animation.
            GridMenu.Opacity = 0;
        }
        
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DisplayProperties_OrientationChanged(this);
        }

        #region Event handlers

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the different dice types... Could be loaded from files later (or part of user settings).
            App.ViewModel.Counters.Add(new DiceCounterViewModel
            {
                Name = "D4",
                Brush = new SolidColorBrush(Colors.Black),
                DiceMax = 4
            });

            App.ViewModel.Counters.Add(new DiceCounterViewModel
            {
                Name = "D6",
                Brush = new SolidColorBrush(ColorHelpers.ColorFromHexString("#2B2B2B")),
                DiceMax = 6
            });

            App.ViewModel.Counters.Add(new DiceCounterViewModel
            {
                Name = "D8",
                Brush = new SolidColorBrush(ColorHelpers.ColorFromHexString("#5C5C5C")),
                DiceMax = 8
            });

            App.ViewModel.Counters.Add(new DiceCounterViewModel
            {
                Name = "D10",
                Brush = new SolidColorBrush(ColorHelpers.ColorFromHexString("#9D1414")),
                DiceMax = 10
            });

            App.ViewModel.Counters.Add(new DiceCounterViewModel
            {
                Name = "D20",
                Brush = new SolidColorBrush(ColorHelpers.ColorFromHexString("#DC0000")),
                DiceMax = 20
            });

            // Entrance animations.
            var menuEntranceAnim = new RepositionThemeAnimation
            {
                SpeedRatio = 0.2
            };

            if (DisplayProperties.CurrentOrientation == DisplayOrientations.Portrait
                || DisplayProperties.CurrentOrientation == DisplayOrientations.PortraitFlipped)
            {
                menuEntranceAnim.FromHorizontalOffset = (this.ActualWidth / 2.0) + (GridMenu.ActualWidth / 2.0);
            }
            else
            {
                menuEntranceAnim.FromVerticalOffset = (this.ActualHeight / 2.0) + (GridMenu.ActualHeight / 2.0);
            }

            Storyboard.SetTarget(menuEntranceAnim, GridMenu);

            var menuOpacityAnim = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.2))
            };

            Storyboard.SetTarget(menuOpacityAnim, GridMenu);
            Storyboard.SetTargetProperty(menuOpacityAnim, "Opacity");

            var menuEntranceStory = new Storyboard();

            menuEntranceStory.Children.Add(menuEntranceAnim);
            menuEntranceStory.Children.Add(menuOpacityAnim);

            menuEntranceStory.BeginTime = TimeSpan.FromSeconds(0.5);
            menuEntranceStory.Begin();
        }

        private void DisplayProperties_OrientationChanged(object sender)
        {
            if (DisplayProperties.CurrentOrientation == DisplayOrientations.Portrait
                || DisplayProperties.CurrentOrientation == DisplayOrientations.PortraitFlipped)
            {
                GridMenu.HorizontalAlignment = HorizontalAlignment.Center;
                GridMenu.VerticalAlignment = VerticalAlignment.Bottom;
            }
            else
            {
                GridMenu.HorizontalAlignment = HorizontalAlignment.Left;
                GridMenu.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        #endregion
    }
}
