
#if CLR
using csutils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace System
{
    /// <summary>
    /// Extension class for color
    /// </summary>
    public static class csutilsColorExtension
    {

        /// <summary>
        /// Convert to hex string
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ToHex(this Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }


        /// <summary>
        /// Calculate complementary color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToComplement(this Color color)
        {
            Color hsl = color;
            int[] c = new int[] { hsl.R, hsl.G, hsl.B };
            c[0] = c[0] >= 128 ? c[0] - 128 : c[0] + 128;
            c[1] = c[1] >= 128 ? c[1] - 128 : c[1] + 128;
            c[2] = c[2] >= 128 ? c[2] - 128 : c[2] + 128;

            return Color.FromRgb((byte)c[0], (byte)c[1], (byte)c[2]);
        }

        /// <summary>
        /// To HSV color 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static HSV ToHSV(this Color color)
        {
            return HSV.FromColor(color);
        }
    }
}
#endif