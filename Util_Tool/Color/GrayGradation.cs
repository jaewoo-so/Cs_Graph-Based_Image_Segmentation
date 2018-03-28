using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorMap
{
    public class GrayGradation : IGradation
    {
        public System.Drawing.Color[] GetGradation(int count, bool inverse)
        {
            System.Drawing.Color[] result = new System.Drawing.Color[count];
            int index = 0;
            for (double i = 0; i < 0.8; i += 0.8 / (double)(count))
            {
                if (inverse)
                {
                    result[result.Length - index - 1] = GrayToRgb(i);
                }
                else
                {
                    result[index] = GrayToRgb(i);
                }
                index++;

            }
            return result;
        }

        public System.Drawing.Color GrayToRgb(double h)
        {
            System.Drawing.Color rgb = System.Drawing.Color.FromArgb(
                    Convert.ToByte(h * 255.0f),
                    Convert.ToByte(h * 255.0f),
                    Convert.ToByte(h * 255.0f));
            return rgb;
        }
    }
}
