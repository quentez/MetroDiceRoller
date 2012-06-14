using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Windows.UI;

namespace MetroDiceRoller.Helpers
{
    public static class ColorHelpers
    {
        public static Color ColorFromHexString(string src)
        {
            // Remove the leading # if present.
            if (src.IndexOf('#') == 0)
            {
                src = src.Remove(0, 1);
            }

            // Check the length.
            if (src.Length != 6 && src.Length != 8)
            {
                return Colors.White;
            }

            byte a = 255;
            byte r, g, b = 255;
            short offset = 0;

            // Extract the alpha channel if needed.
            if (src.Length == 8)
            {
                byte.TryParse(src.Substring(0, 2), NumberStyles.HexNumber, null, out a);
                offset += 2;
            }
                      
            byte.TryParse(src.Substring(offset, 2), NumberStyles.HexNumber, null, out r);
            byte.TryParse(src.Substring(offset + 2, 2), NumberStyles.HexNumber, null, out g);
            byte.TryParse(src.Substring(offset + 4, 2), NumberStyles.HexNumber, null, out b);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
