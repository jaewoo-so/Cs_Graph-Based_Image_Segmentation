using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ValueTuple;

namespace GrpahBasedSegementation
{
    public class BaseDataClass
    {
        public byte[][] Source;
        public List<NDR_Edge>[][] Edges;
        public List<NDR_Edge> Sorted_Edges;
        public int k_value = 6;
        public List<Component> Segments = new List<Component>();
        public Dictionary< Tuple<int,int,int,int> , double > EdgeTable; // Tuple< y1,x1 , y2,x2 , w> 

        public BaseDataClass(byte[][] src , int k)
        {
            Source = src;
            k_value = k;
            EdgeTable = new Dictionary<Tuple<int , int , int , int> , double>();
        }

        public void SetEdge(List<NDR_Edge>[][] edges )
        {
            Edges = edges.Select( rows => rows.ToArray() ).ToArray();
            Sorted_Edges = new List<NDR_Edge>( Edges.SortEdgebyW() );
            
        }
    }

    public class Component : IEnumerable<NDR_Edge>
    {
        public List<Tuple<int,int>> Nodes;
        public List<NDR_Edge> EdgeList;

        public double Intra; //{ get { return EdgeList.Select( x => x.W).Max(); } }
        public double Size { get { return (double)Nodes.Count ; } }

        public Component()
        {
            Nodes = new List<Tuple<int , int>>();
            EdgeList = new List<NDR_Edge>();
        }

        public void Set_Initial(Tuple<int,int> basenodenode)
        {
            Nodes.Add( basenodenode );
            EdgeList.Add( new NDR_Edge( basenodenode ) );
            Intra = 0;
        }
      

        public IEnumerator<NDR_Edge> GetEnumerator ()
        {
            return EdgeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return this.GetEnumerator();
        }
    }

    public class DR_Edge
    {
        public Tuple<int,int> BaseNode;
        public Tuple<int,int> LinkedNode;
        public int            W;

        public DR_Edge () // For Beysian N
        { }

        public DR_Edge(
            Tuple<int , int> baseNode,
            Tuple<int , int> linkedNode,
            int w)
        {
            BaseNode   = baseNode;
            LinkedNode = linkedNode;
            W          = w;
        }

        public void SetData (
            Tuple<int , int> baseNode ,
            Tuple<int , int> linkedNode ,
            int w)
        {
            BaseNode = baseNode;
            LinkedNode = linkedNode;
            W = w;
        }
    }

    public class NDR_Edge // For Markov N
    {
        public Tuple<int,int> BaseNode;
        public Tuple<int,int> LinkedNode;
        public int            W;

        public NDR_Edge ()
        { }

        public NDR_Edge (Tuple<int,int> basenode)
        {
            BaseNode = basenode;
            W = 0;
        }

        public NDR_Edge (
            Tuple<int , int> baseNode ,
            Tuple<int , int> linkedNode ,
            int w)
        {
            BaseNode = baseNode;
            LinkedNode = linkedNode;
            W = w;
        }

        public void SetData (
            Tuple<int , int> baseNode ,
            Tuple<int , int> linkedNode ,
            int w)
        {
            BaseNode = baseNode;
            LinkedNode = linkedNode;
            W = w;
        }
    }
}
