using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorMap
{
    public interface IGradation
    {
        System.Drawing.Color[] GetGradation(int count, bool inverse);
    }
}
