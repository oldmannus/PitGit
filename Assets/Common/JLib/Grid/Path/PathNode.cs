using System;
using System.Collections.Generic;
using System.Text;
using JLib.Utilities;

namespace JLib.Grid
{
    public struct PathNode
    {
        public float F;
        public float G;
        public int H;  // f = gone + heuristic
        public IVec3 Pos;
        public IVec3 ParentPos;
    }
}
