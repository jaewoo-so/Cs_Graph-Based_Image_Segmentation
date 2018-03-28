using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util_Tool.Color;
using SpeedyCoding;

namespace GrpahBasedImageSegementation
{
    public enum Connection { Eight, Four, HalfEight, HalfFour } 

    public static class GBS_Extesion
    {
        public enum Dirction { Up, UpRt, UpLt, Rt, Lt , Dw , DwRt , DwLt }
       
        public static Component Merge (
            this Component src 
            , Component child 
            , NDR_Edge edge
            )
        {
            return  src
                    .Act( @ths => @ths.Nodes = @ths.Nodes
                                                .Concat( child.Nodes )
                                                .ToList())
                    .Act( @ths => @ths.Intra = Math.Max( src.Intra , child.Intra ))
                    .Act( @ths => @ths.EdgeList.Add( edge ));
        }
     

        public static byte[][][] Coloring (
            this List<Component> src
            )
        {
            var camp = new SimpleCMap()
                            .CreateCMap(src.Count);

            var ymax = src.Select( c => c.Nodes
                                            .Select( n => n.Item1)
                                            .Max<int>())
                            .Max();

            var xmax = src.Select( c => c.Nodes
                                            .Select( n => n.Item2)
                                            .Max<int>())
                            .Max();

            var output =  (ymax+1).JArray<byte>( (xmax+1) , 3 );

            for ( int i = 0 ; i < src.Count ; i++ )
            {
                for ( int j = 0 ; j < src[i].Nodes.Count ; j++ )
                {
                    var n = src[i].Nodes[j];
                    var value = camp[i].Select( x => ( byte ) x ).ToArray();
            
                    output[n.Item1][n.Item2] = value;
                }
            }

            //var k = 
            //from i in src
            //from n in i.Nodes
            //select output[n.Item1][n.Item2] = camp[src.IndexOf( i )].Select( x => ( byte ) x.Print() ).ToArray();

            //src.SelectMany(
            //    c => c.Nodes
            //    , (c , n) => output[n.Item1][n.Item2] = camp[src.IndexOf( c )]
            //                                            .Select( x => ( byte ) x.Print() ).ToArray() );
            return output;
        }

        public static List<NDR_Edge>[][] GetEdgeMap (
            this byte[][] src
            , Connection connection)
        {
            int rowsize = src.Len() , colsize = src[0].Len();
            return src.Select((rows , j) =>
                            rows.Select((col,i) =>
                                    Tuple.Create(j,i) // Current Node
                                    .NearPoint( connection )
                                    .Where( x => ( x.Item1 >= 0 )
                                               && ( x.Item1 < rowsize )
                                               && ( x.Item2 >= 0 )
                                               && ( x.Item2 < colsize ) )
                                    .Select( x => new NDR_Edge(
                                       Tuple.Create( j , i )
                                       , x
                                       , ( int ) Math.Abs( src[j][i] - src[x.Item1][x.Item2] )
                                       ) // Create Linked Non Directied Edge Data 
                                  ).ToList()
                              ).ToArray()
                        ).ToArray();
        }

        public static List<NDR_Edge> [ ] [ ] GetEdgeMap(
           this byte [ ] [ ] src
           , Connection connection
           , Dictionary<Tuple<int , int , int , int> , double> cache  )
        {
            int rowsize = src.Len() , colsize = src[0].Len();
            return src.Select( ( rows , j ) =>
                             rows.Select( ( col , i ) =>
                                      Tuple.Create( j , i ) // Current Node
                                      .NearPoint(connection)
                                      .Where( x => ( x.Item1 >= 0 )
                                                 && ( x.Item1 < rowsize )
                                                 && ( x.Item2 >= 0 )
                                                 && ( x.Item2 < colsize ) )
                                      .Select( x => 
                                      {
                                          if ( cache.ContainsKey( Tuple.Create( j , i , x.Item1 , x.Item2 ) ) )
                                          {
                                              return new NDR_Edge(
                                                        Tuple.Create( j , i )
                                                        , x
                                                        , (int)cache [ Tuple.Create( j , i , x.Item1 , x.Item2 ) ] );

                                          }
                                          else
                                          {
                                              var w = ( int )Math.Abs( src [ j ] [ i ] - src [ x.Item1 ] [ x.Item2 ]);
                                              cache.Add( Tuple.Create( j , i , x.Item1 , x.Item2 ) , w );
                                              return new NDR_Edge(
                                                     Tuple.Create( j , i )
                                                     , x
                                                     , w);
                                          }
                                      } // Create Linked Non Directied Edge Data 
                                    ).ToList()
                               ).ToArray()
                        ).ToArray();
        }


        public static List<NDR_Edge> SortEdgebyW (
            this List<NDR_Edge>[][] src
            )
        {
            return src.Flatten()
                      .SelectMany(
                           basenode => basenode ,
                           (basenode , edge) => edge )
                      .ToList()
                      .OrderBy( k => k.W )
                      .ToList();
        }

        public static double MIntra (
          this Component src ,
          Component target ,
          double k
          )
        {
            return Math.Min( src.Intra + k / src.Size , target.Intra + k / target.Size );
        }

        public static List<Tuple<int , int>> NearPoint(
            this Tuple<int , int> src
            , Connection connection
            )
        {
            switch ( connection )
            {
                case Connection.Four:
                    return src.Near4Point();

                case Connection.HalfEight:
                    return src.NearRD4Point();

                case Connection.HalfFour:
                    return src.NearRD2Point();

                default:
                    return src.NearRD4Point();
            }
        }


        private static List<Tuple<int , int>> NearRD2Point (
            this Tuple<int , int> src
            )
        {
            int y = src.Item1 , x = src.Item2;
            return new List<Tuple<int , int>>()
                        .Act( @ths => @ths.Add( Tuple.Create( y , x ).ShiftClockWise( Dirction.Rt ) ) )
                        .Act( @ths => @ths.Add( Tuple.Create( y , x ).ShiftClockWise( Dirction.Dw ) ) );
        }

        private static List<Tuple<int , int>> NearRD4Point(
            this Tuple<int , int> src
            )
        {
            int y = src.Item1 , x = src.Item2;
            return new List<Tuple<int , int>>()
                        .Act( @ths => @ths.Add( Tuple.Create( y , x ).ShiftClockWise( Dirction.Rt   ) ) )
                        .Act( @ths => @ths.Add( Tuple.Create( y , x ).ShiftClockWise( Dirction.DwRt ) ) )
                        .Act( @ths => @ths.Add( Tuple.Create( y , x ).ShiftClockWise( Dirction.Dw   ) ) ) 
                        .Act( @ths => @ths.Add( Tuple.Create( y , x ).ShiftClockWise( Dirction.DwLt ) ) );
        }


        private static List<Tuple<int , int>> Near4Point (
            this Tuple<int , int> src
            )
        {
            int y = src.Item1 , x = src.Item2;
            return new List<Tuple<int , int>>()
                        .Act( @ths => 
                        {
                            foreach ( Dirction dirction in Enum.GetValues( typeof( Dirction ) ) )
                            {
                                @ths.Add( Tuple.Create( y , x ).ShiftClockWise( dirction ) );
                            }
                        } );
        }

        public static Tuple<int , int> ShiftClockWise (
            this Tuple<int , int> src
            , Dirction direction
            )
        {
            switch ( direction )
            {
                case Dirction.Up:
                    return Tuple.Create( src.Item1 - 1  , src.Item2     );
                case Dirction.UpRt:
                    return Tuple.Create( src.Item1 - 1  , src.Item2 + 1 );
                case Dirction.Rt:
                    return Tuple.Create( src.Item1      , src.Item2 + 1 );
                case Dirction.DwRt:
                    return Tuple.Create( src.Item1 + 1  , src.Item2 + 1 );
                  case Dirction.Dw:
                    return Tuple.Create( src.Item1 + 1  , src.Item2     );
                case Dirction.DwLt:
                    return Tuple.Create( src.Item1 + 1  , src.Item2 - 1 );
                case Dirction.Lt:
                    return Tuple.Create( src.Item1      , src.Item2 - 1 );
                case Dirction.UpLt:
                    return Tuple.Create( src.Item1 - 1  , src.Item2 - 1 );
                default:
                    return null;
            }
        }
    }

}

namespace Util_Tool.Color
{
	public class SimpleCMap
	{
		public List<int[]> CreateCMap( int count )
		{
			int step = 256*3 / (count+2);
			return Enumerable.Range( 1, count )
					.Select( i => i * step )
					.ToArray()
					.Select( pos =>
								new int[3]
								{
									pos < 256 ? pos : 0
									, pos > 255 && pos <= 256*2 ? pos -256 : 0
									, pos > 256*2 ? pos - 256*2 : 200
								} )
					.ToList();
		}
	}
}



