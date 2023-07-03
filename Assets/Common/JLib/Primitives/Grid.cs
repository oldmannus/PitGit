using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;


namespace JLib.Utilities
{
    [Serializable]
    public class Grid<T>
    {
        public delegate void IterateOverDelegate(int x, int y, object data);

        public int Size { get; private set; }
        protected T[,] _data;

        public T this[int x, int y]
        {
            get
            {
                return _data[x, y];
            }
            set
            {
                _data[x, y] = value;
            }
        }

        public Grid()
        {

        }


        public Grid(int size)
        {
            Size = size;
            _data = new T[size, size];
        }

        public virtual void Init(int gridSize)
        {
            Size = gridSize;
            _data = new T[gridSize, gridSize];
        }

        public virtual void Normalize() { }

        public void ForEachDo( IterateOverDelegate dlg, object data = null)
        {
            for (int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                dlg(x,y, data);

        }

    }




    [Serializable]
    public class FloatGrid : Grid<float>
    {

        public FloatGrid(int sz) : base(sz) {}

        public bool IsNormalized()
        {
            for (int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                    if (_data[x, y] < 0 || _data[x, y] > 1)
                        return false;


            return true;
        }


        public override void Normalize()
        {
            float minHt = float.MaxValue;
            float maxHt = float.MinValue;

            for (int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                {
                    if (_data[x, y] < minHt)
                        minHt = _data[x, y];
                    if (_data[x, y] > maxHt)
                        maxHt = _data[x, y];
                }


            Dbg.Assert(Math.Abs(minHt - maxHt) > float.Epsilon);
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    _data[x, y] = MathExt.Normalize(_data[x, y], minHt, maxHt);
                }
            }
        }
    }

    [Serializable]
    public class FlagGrid: Grid<UInt64>
    {
        public FlagGrid(int sz) : base(sz) { }

        public void SetFlag(int x, int y, int flag, bool b)
        {
            if (b)
                _data[x, y] |= (UInt64)((UInt64)1 << flag);
            else
                _data[x, y] &= (UInt64)~((UInt64)((UInt64)1 << flag));
        }

        public bool HasFlag(int x, int y, int flag)
        {
            return (_data[x, y] & ((UInt64)1 << (int)flag)) != 0;
        }
    }
}
