using Emgu.CV;
using Emgu.CV.Structure;
using GrpahBasedImageSegementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeedyCoding;

namespace ForTest_GraphBasedSegmetation
{
	class Program
	{
		[STAThread]
		static void Main( string [ ] args )
		{
			byte[][] testdata = null;
			OpenFileDialog ofd = new OpenFileDialog() ;


			if ( ofd.ShowDialog() == DialogResult.OK )
			{
				testdata = new Image<Gray , byte>( ofd.FileName ).Data.ToJagged().Select( f => f.Select( s => s [ 0 ] ).ToArray() ).ToArray();

			}

			//byte[][] testdata = new byte[8][]
			//{
			//     new byte[] { 1 , 2 ,3 ,100, 102 }
			//    ,new byte[] { 2, 3 , 1 , 103 , 101 }
			//    ,new byte[] { 4, 2, 1, 100, 102 }
			//    ,new byte[] { 4, 2, 1, 100, 102 }
			//    ,new byte[] { 80, 2, 1, 100, 102 }
			//    ,new byte[] { 60, 2, 1, 100, 102 }
			//    ,new byte[] { 77, 2, 1, 100, 102 }
			//    ,new byte[] { 4, 2, 1, 100, 102 }
			//};



			GBS gg = new GBS();
			var result = gg.Processing(testdata , 80);
			var data = result.Coloring().ToMat();
			Image<Rgb,byte> output = new Image<Rgb, byte>(data);
			output.Save( @"C:\00_TestTemp\GBS.bmp" );
			Console.WriteLine("Done");
			Console.ReadLine();
		}
	
	}
}
