using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace MetroDiceRoller.Controls
{
    public class TapGrid : Grid
    {
        public TapGrid()
        {
            // Hide the control.
            this.Opacity = 0;

            // Down animation storyboard.
            var downAnim = new FadeInThemeAnimation
            {
                SpeedRatio = 5
            };
            Storyboard.SetTarget(downAnim, this);

            downStory = new Storyboard();
            downStory.Children.Add(downAnim);

            // Up animation storyboard.
            var upAnim = new FadeOutThemeAnimation
            {
                SpeedRatio = 0.5
            };
            Storyboard.SetTarget(upAnim, this);

            upStory = new Storyboard();
            upStory.Children.Add(upAnim);

            // Set event handlers.
            this.PointerPressed += (sender, e) =>
            {
                this.Opacity = 1;
                this.CapturePointer(e.Pointer);
                downStory.Begin();

                if (this.Command.CanExecute(null))
                {
                    this.Command.Execute(null);
                }
            };

            this.PointerReleased += (sender, e) =>
            {
                upStory.Begin();
                this.ReleasePointerCapture(e.Pointer);
            };
        }

        private Storyboard downStory;
        private Storyboard upStory;
        
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(TapGrid), new PropertyMetadata(null)); 
    }
}
