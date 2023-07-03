using System;
using System.Collections.Generic;
using System.Text;

namespace JLib.Grid
{
    public class Path
    {
        public List<PathNode> Nodes = new List<PathNode>();

        public bool IsEqual(Path other)
        {
            if (other == null || other.Nodes == null)
                return false;

            if (other.Nodes.Count != Nodes.Count)
            {
                return false;
            }

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Pos != other.Nodes[i].Pos)
                    return false;
            }

            return true;

        }

        public void Add(PathNode n) { Nodes.Add(n); }
        public void Clear() { Nodes.Clear(); }
        public int Count { get { return Nodes.Count; } }
        public void RemoveAt(int i) { Nodes.RemoveAt(i); }
        public PathNode this[int key]
        {
            get
            {
                return Nodes[key];
            }
            set
            {
                Nodes[key] = value;
            }
        }
    }
}
