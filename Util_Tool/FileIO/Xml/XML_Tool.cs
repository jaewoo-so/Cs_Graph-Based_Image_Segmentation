using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Util_Tool.FileIO.Xml
{
    public class XML_Tool
    {
        TestData data = new TestData();

        public void test_Setting_Data() {
            data.a = 1;
            data.b = 0.8;
            data.aArray = new dynamic[] { 1 , 2 , 34 };
            data.bList = new List<dynamic>();
            data.bList.Add( 5.6 );
            data.bList.Add( 5.3 );
            data.bList.Add( 2.6 );
            data.bList.Add( 1.6 );
        }


        public void Wirtefile() {
            using ( StreamWriter wr = new StreamWriter( @"C:\Users\idiol\Desktop\Local Project\Util_Tool\TestFolder\test.xml" ) ) {
                XmlSerializer xs = new XmlSerializer(typeof(TestData));
                xs.Serialize( wr , data );

            }

        }

        public TestData Readfile() {
            using (StreamReader sr = new StreamReader( @"C:\Users\idiol\Desktop\Local Project\Util_Tool\TestFolder\test.xml" ) ) {
                XmlSerializer xs = new XmlSerializer(typeof(TestData));
                TestData output = (TestData) xs.Deserialize(sr);
                return output;
            }
        }
    }
}
