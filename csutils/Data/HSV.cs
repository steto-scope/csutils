#if CLR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace csutils.Data
{
    /// <summary>
    /// Represents the HSV color space
    /// </summary>
    public class HSV
    {
        private double h;
        private double s;
        private double l;
        private double a;

        /// <summary>
        /// Hue [0.0,1.0]
        /// </summary>
        public double H
        {
            get
            {
                return h;
            }
            set
            {
                h = Math.Min(1, Math.Max(0, value));
            }
        }
        /// <summary>
        /// Saturation [0.0,1.0]
        /// </summary>
        public double S
        {
            get
            {
                return s;
            }
            set
            {
                s = Math.Min(1, Math.Max(0, value));
            }
        }
        /// <summary>
        /// Value [0.0,1.0]
        /// </summary>
        public double V
        {
            get
            {
                return l;
            }
            set
            {
                l = Math.Min(1, Math.Max(0, value));
            }
        }

        /// <summary>
        /// Alpha [0.0,1.0]
        /// </summary>
        public double A
        {
            get
            {
                return a;
            }
            set
            {
                a = Math.Min(1, Math.Max(0, value));
            }
        }

        struct ColorPair
        {
            public double val;
            public string channel;
        }


        /// <summary>
        /// Converts a color into a HSV object
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static HSV FromColor(Color rgb)
        {
            HSV hsl = new HSV();
            hsl.H = 0; // default to black
            hsl.S = 0;
            hsl.V = 0;
            hsl.A = (double)rgb.A / 255.0;


            double deg1 = 1.0 / 360.0;
            double h = 0.0;

            ColorPair r = new ColorPair() { val = (double)rgb.R / 255.0, channel = "R" };
            ColorPair g = new ColorPair() { val = (double)rgb.G / 255.0, channel = "G" };
            ColorPair b = new ColorPair() { val = (double)rgb.B / 255.0, channel = "B" };
            ColorPair[] values = new ColorPair[] { r, g, b };

            ColorPair max = values.OrderByDescending(v => v.val).First();
            ColorPair min = values.OrderBy(v => v.val).First();

            if (min.channel == max.channel)
                h = 0;
            else
                switch (max.channel)
                {
                    case "R":
                        h = 60 * deg1 * (0 + (g.val - b.val) / (max.val - min.val));
                        break;
                    case "G":
                        h = 60 * deg1 * (2 + (b.val - r.val) / (max.val - min.val));
                        break;
                    case "B":
                        h = 60 * deg1 * (4 + (r.val - g.val) / (max.val - min.val));
                        break;
                    default:
                        h = 0;
                        break;
                }
            if (h < 0)
                h += 1;
            hsl.H = h;

            if (max.val == 0)
                hsl.S = 0;
            else
                hsl.S = (max.val - min.val) / max.val;

            hsl.V = max.val;

            return hsl;
        }

        /// <summary>
        /// Generiert aus dem HSL-Farb-Objekt wieder ein (RGB)Color-Objekt
        /// </summary>
        /// <returns></returns>
        public Color ToColor()
        {
            Color rgb = new Color();

            int r = (int)Math.Truncate((H / (1.0 / 6.0)));
            double f = (H / (1.0 / 6.0)) - r;
            double p = V * (1 - S);
            double q = V * (1 - S * f);
            double t = V * (1 - S * (1 - f));

            switch (r)
            {
                case 0:
                case 6:
                default:
                    rgb = new Color() { R = (byte)Math.Round(V * 255), G = (byte)Math.Round(t * 255), B = (byte)Math.Round(p * 255), A = (byte)(A*255) };
                    break;
                case 1:
                    rgb = new Color() { R = (byte)Math.Round(q * 255), G = (byte)Math.Round(V * 255), B = (byte)Math.Round(p * 255), A = (byte)(A * 255) };
                    break;
                case 2:
                    rgb = new Color() { R = (byte)Math.Round(p * 255), G = (byte)Math.Round(V * 255), B = (byte)Math.Round(t * 255), A = (byte)(A * 255) };
                    break;
                case 3:
                    rgb = new Color() { R = (byte)Math.Round(p * 255), G = (byte)Math.Round(q * 255), B = (byte)Math.Round(V * 255), A = (byte)(A * 255) };
                    break;
                case 4:
                    rgb = new Color() { R = (byte)Math.Round(t * 255), G = (byte)Math.Round(p * 255), B = (byte)Math.Round(V * 255), A = (byte)(A * 255) };
                    break;
                case 5:
                    rgb = new Color() { R = (byte)Math.Round(V * 255), G = (byte)Math.Round(p * 255), B = (byte)Math.Round(q * 255), A = (byte)(A * 255) };
                    break;
            }
            
            return rgb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToColor().ToString();
        }

    }
}
#endif