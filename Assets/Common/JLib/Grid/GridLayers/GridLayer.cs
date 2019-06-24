using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using JLib.Utilities;

namespace JLib.Grid
{

    public interface IGridLayer
    {
        void Initialize(IVec3 size, Grid parent);

        UInt64 Id { get; set; }
    }

    [Serializable]
    public class GridLayer<T> : IGridLayer
    {
        public UInt64 Id { get { return _id; } set { _id = value; } }
        public T[,,] Layer;
        protected Grid _grid;
        UInt64 _id = 0;

        public virtual void Initialize(IVec3 size, Grid parent)
        {
            _grid = parent;
            Layer = new T[size.x, size.y, size.z];
        }

        public virtual void Set(IVec3 pos, T value)
        {
            Layer[pos.x, pos.y, pos.z] = value;
        }

        public virtual T Get(IVec3 pos)
        {
            return Layer[pos.x, pos.y, pos.z];
        }

        public virtual void Dump()
        {
            string line;
            for (int z = 0; z < Layer.GetLength(2); z++)
            {
                Debug.Print(string.Format("*********** Z {0} **************** \n", z));
                for (int x = 0; x < Layer.GetLength(0); x++)
                {
                    line = "";
                    for (int y = 0; y < Layer.GetLength(1); y++)
                    {
                        line += " " + Layer[x, y, z];
                    }
                    Debug.Print(line + "\n");
                }
            }
        }
    }
}
