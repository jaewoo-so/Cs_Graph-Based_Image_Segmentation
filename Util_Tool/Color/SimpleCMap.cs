using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util_Tool.Color
{
    public class SimpleCMap
    {
        public List<int[]> CreateCMap ( int count )
        {
            int step = 256*3 / (count+2);
            return Enumerable.Range(1,count)
                    .Select( i => i*step)
                    .ToArray()
                    .Select( pos =>
                                new int[3]
                                {
                                    pos < 256 ? pos : 0
                                    , pos > 255 && pos <= 256*2 ? pos -256 : 0
                                    , pos > 256*2 ? pos - 256*2 : 200
                                })
                    .ToList();
        }
    }
}
