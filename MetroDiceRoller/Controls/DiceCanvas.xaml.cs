using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using MetroDiceRoller.ViewModel;
using MetroDiceRoller.Helpers;

namespace MetroDiceRoller
{
    public sealed partial class DiceCanvas : UserControl
    {
        public DiceCanvas()
        {
            this.InitializeComponent();
        }

        #region Locals

        private Dictionary<DiceViewModel, Rect> diceRects = new Dictionary<DiceViewModel, Rect>();

        #endregion

        #region Event Handlers

        private void DiceCounters_Changed()
        {
            this.DiceCounters.CollectionChanged += (sender, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (DiceCounterViewModel diceCounter in e.NewItems)
                    {
                        diceCounter.DiceList.CollectionChanged += DiceList_CollectionChanged;

                        foreach (var dice in diceCounter.DiceList)
                        {
                            AddDice(dice);
                        }
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (DiceCounterViewModel diceCounter in e.OldItems)
                    {
                        diceCounter.DiceList.CollectionChanged -= DiceList_CollectionChanged;

                        foreach (var dice in diceCounter.DiceList)
                        {
                            RemoveDice(dice);
                        }
                    }
                }
            };
        }

        private void DiceList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DiceViewModel dice in e.NewItems)
                {
                    AddDice(dice);
                }
            }

            if (e.OldItems != null)
            {
                foreach (DiceViewModel dice in e.OldItems)
                {
                    RemoveDice(dice);
                }
            }
        }

        #endregion

        #region Logic
        
        private void AddDice(DiceViewModel newDiceViewModel)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var storyRollDice = new Storyboard();

                var maxWidth = MainCanvas.ActualWidth;
                var maxHeight = MainCanvas.ActualHeight;

                // Retrieve the center position of the menu, to use as a start point for the animation later.
                var menuCenter = (Menu.VerticalAlignment == VerticalAlignment.Center)
                                    ? new Point(Menu.ActualWidth / 2.0, maxHeight / 2.0)
                                    : new Point(maxWidth / 2.0, maxHeight - (Menu.ActualHeight / 2.0));


                // Create the Dice object.
                var dice = new Dice
                {
                    DataContext = newDiceViewModel
                };

                // Add it to the Canvas.
                MainCanvas.Children.Add(dice);

                // Find a position to place the dice in max 10 loops.
                Rect posRect;
                bool posOk = false;
                short tryCount = 0;

                while (!posOk && tryCount < 10)
                {
                    double newTop;
                    double newLeft;

                    if (Menu.VerticalAlignment == VerticalAlignment.Center)
                    {
                        newTop = (double)RandomHelpers.Next(0, (int)(maxHeight - dice.RealHeight));
                        newLeft = ((newTop > (maxHeight / 2.0) ? newTop - (maxHeight / 2.0) : (maxHeight / 2.0) - newTop - dice.RealHeight) > (Menu.ActualHeight / 2.0)) ? (double)RandomHelpers.Next(0, (int)(maxWidth - dice.RealWidth)) : (double)RandomHelpers.Next((int)Menu.ActualWidth, (int)(maxWidth - dice.RealWidth));
                    }
                    else
                    {
                        newLeft = (double)RandomHelpers.Next(0, (int)(maxWidth - dice.RealWidth));
                        newTop = ((newLeft > (maxWidth / 2.0) ? newLeft - (maxWidth / 2.0) : (maxWidth / 2.0) - newLeft - dice.RealWidth) > (Menu.ActualWidth / 2.0)) ? (double)RandomHelpers.Next(0, (int)(maxHeight - dice.RealHeight)) : (double)RandomHelpers.Next(0, (int)(maxHeight - dice.RealHeight - Menu.ActualHeight));
                    }

                    posRect = new Rect(newLeft, newTop, dice.RealWidth, dice.RealHeight);
                    posOk = true;

                    foreach (var otherDiceRect in diceRects.Values)
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
                diceRects.Add(newDiceViewModel, posRect);

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

                // Run the animaton.
                storyRollDice.Begin();
            });
        }

        private void RemoveDice(DiceViewModel oldDiceViewModel)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Find the visual Dice to remove.
                var dice = MainCanvas.Children
                    .Where(u => ((FrameworkElement)u).DataContext == oldDiceViewModel)
                    .FirstOrDefault() as Dice;

                if (dice == null)
                {
                    return;
                }

                // Remove the associated rect from the collection.
                diceRects.Remove(oldDiceViewModel);

                // Build the animation to remove the visual Dice.
                var storyFadeOldDice = new Storyboard();

                var fadeOutAnim = new FadeOutThemeAnimation
                {
                    SpeedRatio = 0.5
                };

                Storyboard.SetTarget(fadeOutAnim, dice);
                storyFadeOldDice.Children.Add(fadeOutAnim);

                // Event to remove the old dice after animation.
                storyFadeOldDice.Completed += (sender2, e2) =>
                {
                    MainCanvas.Children.Remove(dice);
                };

                // Run the animation.
                storyFadeOldDice.Begin();
            });
        }

        #endregion

        #region Attached properties

        public ObservableCollection<DiceCounterViewModel> DiceCounters
        {
            get { return (ObservableCollection<DiceCounterViewModel>)GetValue(DiceCountersProperty); }
            set { SetValue(DiceCountersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DiceCounters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DiceCountersProperty =
            DependencyProperty.Register("DiceCounters", typeof(ObservableCollection<DiceCounterViewModel>), typeof(DiceCanvas), new PropertyMetadata(null, new PropertyChangedCallback(DiceCounters_ChangedStatic)));

        public static void DiceCounters_ChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var diceCanvas = sender as DiceCanvas;
            if (diceCanvas != null)
            {
                diceCanvas.DiceCounters_Changed();
            }
        }
        
        public FrameworkElement Menu
        {
            get { return (FrameworkElement)GetValue(MenuProperty); }
            set { SetValue(MenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Menu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register("Menu", typeof(FrameworkElement), typeof(DiceCanvas), new PropertyMetadata(null));
          
        #endregion
    }
}
