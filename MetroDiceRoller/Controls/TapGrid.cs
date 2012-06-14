using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // Down animation storyboard.
            var downAnim = new PointerDownThemeAnimation();
            Storyboard.SetTarget(downAnim, this);

            downStory = new Storyboard();
            downStory.Children.Add(downAnim);

            // Up animation storyboard.
            var upAnim = new PointerUpThemeAnimation();
            Storyboard.SetTarget(upAnim, this);

            upStory = new Storyboard();
            upStory.Children.Add(upAnim);

            // Set event handlers.
            this.PointerPressed += (sender, e) =>
            {
                this.CapturePointer(e.Pointer);
                downStory.Begin();
            };

            this.PointerReleased += (sender, e) =>
            {
                upStory.Begin();
                this.ReleasePointerCapture(e.Pointer);
            };
        }

        private Storyboard downStory;
        private Storyboard upStory;
    }
}
