using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using System.Diagnostics;

namespace GrpahBasedSegementation
{
    public partial class GBS
    {

        public List<Component> Processing (byte[][] src , int k_value , Connection connection = Connection.HalfEight)
        {
			Stopwatch stw = new Stopwatch();
            if ( src == null ) return null;
            int rows = src.Len() , cols = src.Len(1);
            Data = new BaseDataClass( src , 3 );
            Data.SetEdge(Data.Source.GetEdgeMap( connection  , Data.EdgeTable) ); // cache added 
			stw.Start();
            var CompList = Enumerable.Range(0,rows)
                            .SelectMany(
                                j => Enumerable.Range(0,cols),
                                (j,i) => new Component()
                                                .Act(@ths => @ths.Set_Initial(
                                                                        Tuple.Create(j,i))))
                            .ToList();

			stw.Stop();
			Console.WriteLine( "ComList Time : " + (stw.ElapsedMilliseconds / 1000).ToString() );
			stw.Reset();
            var edgeslit = Data.Sorted_Edges;

			stw.Start();
            foreach ( var edge in Data.Sorted_Edges )
            {
                var basecomp   = CompList.Where( x => x.Nodes.Contains( edge.BaseNode) ).ToList()[0];
                var linkedcomp = CompList.Where( x => x.Nodes.Contains( edge.LinkedNode) ).ToList()[0];
                if ( basecomp == linkedcomp ) continue;
                double MIntra = Math.Min(
                                         basecomp.Intra + Data.k_value / basecomp.Size
                                         , linkedcomp.Intra + Data.k_value / linkedcomp.Size);

                if ( edge.W <= MIntra )
                {
                    CompList.RemoveAt( CompList.IndexOf( basecomp ) );
                    CompList.RemoveAt( CompList.IndexOf( linkedcomp ) );
                    CompList.Add( basecomp.Merge( linkedcomp , edge ) );
                }
            }
			Console.WriteLine( "var edge in Data.Sorted_Edges Time : " + ( stw.ElapsedMilliseconds / 1000 ).ToString() );
			stw.Reset();
			stw.Start();
			for ( int i = 0 ; i < Data.Sorted_Edges.Count ; i++ )
            {  
                    var weight  = edgeslit[i].W;
                    var baseN   = edgeslit[i].BaseNode;
                    var linkedN = edgeslit[i].LinkedNode;

                    var basecomp   = CompList.Where( x => x.Nodes.Contains( baseN) ).ToList()[0];
                    var linkedcomp = CompList.Where( x => x.Nodes.Contains( linkedN) ).ToList()[0]; 
                    if ( basecomp == linkedcomp ) continue;

                    double MIntra = Math.Min(
                                         basecomp.Intra + Data.k_value / basecomp.Size
                                         , linkedcomp.Intra + Data.k_value / linkedcomp.Size);

                    if ( weight <= MIntra )
                    {
                        CompList.RemoveAt( CompList.IndexOf( basecomp ) );
                        CompList.RemoveAt( CompList.IndexOf( linkedcomp ) );
                        CompList.Add( basecomp.Merge( linkedcomp 
                                      , new NDR_Edge( baseN , linkedN , weight ) ) );
                    }
            }
			Console.WriteLine( " Data.Sorted_Edges.Count Time : " + ( stw.ElapsedMilliseconds / 1000 ).ToString() );
			stw.Reset();
			stw.Start();
			return CompList;
        }
    }
}
