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

            // Orientation Change event.
            DisplayProperties.OrientationChanged += DisplayProperties_OrientationChanged;
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

        private void BTReset_Click(object sender, RoutedEventArgs e)
        {
            // Reset the count of every DiceCounterViewModel in the menu.
            foreach (var counter in App.ViewModel.Counters)
            {
                counter.DiceCount = 0;
            }
        }

        private void BTRoll_Click(object sender, RoutedEventArgs e)
        {
            var rdm = new Random();

            var storyFadeOldDice = new Storyboard();
            var storyRollDice = new Storyboard();

            var otherDiceRects = new List<Rect>();
            var maxWidth = CanvasDice.ActualWidth;
            var maxHeight = CanvasDice.ActualHeight;
            
            // Retrieve the center position of the menu, to use as a start point for the animation later.
            var menuCenter = (GridMenu.VerticalAlignment == VerticalAlignment.Center) 
                                ? new Point(GridMenu.ActualWidth / 2.0, maxHeight / 2.0)
                                : new Point(maxWidth / 2.0, maxHeight - (GridMenu.ActualHeight / 2.0));

            // Prepare to remove old controls.
            var oldDiceList = new List<UIElement>();
            foreach (UIElement oldDice in CanvasDice.Children)
            {
                var fadeOutAnim = new FadeOutThemeAnimation
                {
                    SpeedRatio = 0.5
                };

                Storyboard.SetTarget(fadeOutAnim, oldDice);
                storyFadeOldDice.Children.Add(fadeOutAnim);

                oldDiceList.Add(oldDice);
            }

            // Event to remove the old dice after animation.
            storyFadeOldDice.Completed += (sender2, e2) =>
            {
                foreach (var oldDice in oldDiceList)
                {
                    CanvasDice.Children.Remove(oldDice);
                }
            };

            // Create the new controls.
            foreach (var counter in App.ViewModel.Counters)
            {
                for (int index = 0; index < counter.DiceCount; index++)
                {
                    // Create the Dice object.
                    var dice = new Dice
                    {
                        DataContext = new DiceViewModel
                        {
                            Number = rdm.Next(1, counter.DiceMax + 1),
                            SizeRatio = (counter.DiceMax + 10) / 20.0,
                            Brush = counter.Brush
                        }
                    };

                    // Add it to the Canvas.
                    CanvasDice.Children.Add(dice);

                    // Find a position to place the dice in max 10 loops.
                    Rect posRect;
                    bool posOk = false;
                    short tryCount = 0;

                    while (!posOk && tryCount < 10)
                    {
                        double newTop;
                        double newLeft;

                        if (GridMenu.VerticalAlignment == VerticalAlignment.Center)
                        {
                            newTop = (double)rdm.Next(0, (int)(maxHeight - dice.RealHeight));
                            newLeft = ((newTop > (maxHeight / 2.0) ? newTop - (maxHeight / 2.0) : (maxHeight / 2.0) - newTop - dice.RealHeight) > (GridMenu.ActualHeight / 2.0)) ? (double)rdm.Next(0, (int)(maxWidth - dice.RealWidth)) : (double)rdm.Next((int)GridMenu.ActualWidth, (int)(maxWidth - dice.RealWidth));
                        }
                        else
                        {
                            newLeft = (double)rdm.Next(0, (int)(maxWidth - dice.RealWidth));
                            newTop = ((newLeft > (maxWidth / 2.0) ? newLeft - (maxWidth / 2.0) : (maxWidth / 2.0) - newLeft - dice.RealWidth) > (GridMenu.ActualWidth / 2.0)) ? (double)rdm.Next(0, (int)(maxHeight - dice.RealHeight)) : (double)rdm.Next(0, (int)(maxHeight - dice.RealHeight - GridMenu.ActualHeight));
                        }

                        posRect = new Rect(newLeft, newTop, dice.RealWidth, dice.RealHeight);
                        posOk = true;

                        foreach (var otherDiceRect in otherDiceRects)
                        {
                            var intersection = otherDiceRect;
                            intersection.Intersect(posRect);

                            if (!intersection.IsEmpty)
                            {
                                posOk = false;
                                tryCount++;
                                continue;
                            }
                        }
                    }

                    // Add the current dice's rect to the list.
                    otherDiceRects.Add(posRect);

                    Canvas.SetLeft(dice, posRect.X);
                    Canvas.SetTop(dice, posRect.Y);

                    var repositionAnim = new RepositionThemeAnimation
                    {
                        FromHorizontalOffset = (menuCenter.X - (dice.RealWidth / 2.0)) - posRect.X,
                        FromVerticalOffset = (menuCenter.Y - (dice.RealHeight / 2.0)) - posRect.Y,
                        SpeedRatio = 0.2
                    };

                    Storyboard.SetTarget(repositionAnim, dice);
                    storyRollDice.Children.Add(repositionAnim);
                }
            }

            // Start roll animation.
            storyFadeOldDice.Begin();
            storyRollDice.Begin();
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
