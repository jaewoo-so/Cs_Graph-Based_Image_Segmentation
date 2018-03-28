using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util_Tool.UI
{
    public static class Corrdinate
    {
        public static Func<double[] , double[]> Convt_Window2Real( double winW , double winH , double imgW , double imgH )
        {
            return new Func<double[] , double[]>( ( winPos ) =>
            {
                return new double[2]{ (winPos[0] * imgH )/winH,
                                      (winPos[1] * imgW)/winW };
            } );
        }

        public static Func<double[] , double[]> Convt_Real2window( double winW , double winH , double imgW , double imgH )
        {
            return new Func<double[] , double[]>( ( realPos ) => {
                return new double[2]{ (realPos[0] * winH )/imgH,
                                      (realPos[1] * winW)/imgW };
            } );
        }
    }
}
