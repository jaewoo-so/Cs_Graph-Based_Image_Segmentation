using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util_Tool.FileIO.Csv  
{
    public enum ArrayType { Vector , Matrix }

    public static class TypeConvert {
        public static T ChangeType<T>( this object obj )
        {
            return ( T ) Convert.ChangeType( obj , typeof( T ) );

        }
    }

    public class Csv<Tout>
    {
        public Func<string , Tout[][]> LoadCsv( char delimiter , int headerLine = 0 , bool headerIgnore = true )
        {
            return new Func<string , Tout[][]>( ( filePath ) =>
            {
                Tout[][] output;
                using ( FileStream fs = new FileStream( filePath , FileMode.Open ) )
                {
                    using ( StreamReader sr = new StreamReader( fs , Encoding.UTF8 , false ) )
                    {
                        StringBuilder sb = new StringBuilder();
                        List<Tout[]> stlist = new List<Tout[]>();

                        if ( !headerIgnore )
                        {
                            for ( int i = 0 ; i < headerLine ; i++ )
                            {
                                sr.ReadLine();
                            }
                        }

                        while ( !sr.EndOfStream )
                        {
                            string readline = sr.ReadLine();
                            // Must not be empty.
                            if ( !string.IsNullOrEmpty( readline ) )
                            {
                                string[] value = readline.Trim().Split(delimiter);
                                var vlist = value.Where(x=> !string.IsNullOrEmpty(x)).ToArray<string>();
                                Tout[] dv = vlist.Select(s =>  s.ChangeType<Tout>() ).ToArray();
                                stlist.Add( dv );
                            };
                        }
                        output = stlist.ToArray();
                    }
                }
                return output;
            });
        }

        public Action<string,Tout[][],Tout[]> WriteCsv( char delimiter , ArrayType type ) {
            return new Action<string,Tout[][],Tout[]>(( filePath , data , header ) => {
                var csv = new StringBuilder();
                StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
                sw.Dispose();
            } );
        }

        public Func<Tout[][] , Tout[][]> SelectColData(int[] selectIdx) {
            return new Func<Tout[][] , Tout[][]>((data) => {
                Tout[][] selectedData = new Tout[data.GetLength(0)][];
                for ( int i = 0 ; i < data.GetLength( 0 ) ; i++ )
                {
                    Tout[] selectedcol = new Tout[selectIdx.GetLength(0)];
                    for ( int j = 0 ; j < selectIdx.GetLength( 0 ) ; j++ )
                    {
                        selectedcol[j] = data[i][j];
                    }
                    selectedData[i] = selectedcol;
                }
                return selectedData;
            } );


        }

       

    }
}
