    using System;
using System.Collections.Generic;
using System.Text;
using JLib.Utilities;

namespace JLib.Grid
{
    [Flags]
    public enum DirectionFlag
    {
        PosX = 1 << 0,
        PosY = 1 << 1,
        PosZ = 1 << 2,

        NegX = 1 << 3,
        NegY = 1 << 4,
        NegZ = 1 << 5,

        Diagonal = 1 << 6,

        YAxis = PosY | NegY,
         

        Full = PosX | PosY | PosZ | NegX | NegY | NegZ,

        FlatYUpDiagonal = PosX | PosZ | NegX | NegZ | Diagonal
    }

    public class Offsets3d
    {
        static int[] noOffset = { 0 };
        static int[] negOffset = { 0, -1 };
        static int[] posOffset = { 1, 0 };
        static int[] posNegOffset = { 1, 0, -1 };

        public static bool IsDiagonal(IVec3 v)
        {
            bool x = v.x != 0;
            bool y = v.y != 0;
            bool z = v.z != 0;

            return x && y || x && z || y && z;
        }


        static int[] GetAxisOffset(bool canPos, bool canNeg)
        {
            if (canPos)
            {
                return canNeg ? posNegOffset : posOffset;
            }
            else if (canNeg)
            {
                return negOffset;
            }
            else
            {
                return noOffset;
            }
        }

        static Dictionary<DirectionFlag, IVec3[]> _cache = new Dictionary<DirectionFlag, IVec3[]>();

        public static DirectionFlag OffsetToDirection(IVec3 vec)
        {
            DirectionFlag flag = 0;
            if (vec.x > 0)
                flag |= DirectionFlag.PosX;
            else if (vec.x < 0)
                flag |= DirectionFlag.NegX;

            if (vec.y > 0)
                flag |= DirectionFlag.PosY;
            else if (vec.y < 0)
                flag |= DirectionFlag.NegY;

            if (vec.z > 0)
                flag |= DirectionFlag.PosZ;
            else if (vec.z < 0)
                flag |= DirectionFlag.NegZ;

            return flag;

        }      

  


        public static IVec3[] GetOffsets(DirectionFlag flags)
        {
            IVec3[] val;
            if (_cache.TryGetValue(flags, out val))
            {
                return val;
            }
            IVec3[] result;

            if ((flags & DirectionFlag.Diagonal) == DirectionFlag.Diagonal)
            {
                int[] xOffsets = GetAxisOffset((flags & DirectionFlag.PosX) == DirectionFlag.PosX, (flags & DirectionFlag.NegX) == DirectionFlag.NegX);
                int[] yOffsets = GetAxisOffset((flags & DirectionFlag.PosY) == DirectionFlag.PosY, (flags & DirectionFlag.NegY) == DirectionFlag.NegY);
                int[] zOffsets = GetAxisOffset((flags & DirectionFlag.PosZ) == DirectionFlag.PosZ, (flags & DirectionFlag.NegZ) == DirectionFlag.NegZ);

                result = new IVec3[xOffsets.Length * yOffsets.Length * zOffsets.Length - 1];
                int n = 0;
                for (int x = 0; x < xOffsets.Length; x++)
                    for (int y = 0; y < yOffsets.Length; y++)
                        for (int z = 0; z < zOffsets.Length; z++)
                        {
                            int xv = xOffsets[x];
                            int yv = yOffsets[y];
                            int zv = zOffsets[z];

                            if (xv != 0 || yv != 0 || zv != 0)
                            {
                                result[n++] = new IVec3(xv, yv, zv);
                            }
                        }
            }
            else
            {
                // no diagonals
                List<IVec3> offsets = new List<IVec3>();
                if ((flags & DirectionFlag.PosX) == DirectionFlag.PosX)
                    offsets.Add(new IVec3(1, 0, 0));
                if ((flags & DirectionFlag.NegX) == DirectionFlag.NegX)
                    offsets.Add(new IVec3(-1, 0, 0));

                if ((flags & DirectionFlag.PosY) == DirectionFlag.PosY)
                    offsets.Add(new IVec3(0, 1, 0));
                if ((flags & DirectionFlag.NegY) == DirectionFlag.NegY)
                    offsets.Add(new IVec3(0, -1, 0));

                if ((flags & DirectionFlag.PosZ) == DirectionFlag.PosZ)
                    offsets.Add(new IVec3(0, 0, 1));
                if ((flags & DirectionFlag.NegZ) == DirectionFlag.NegZ)
                    offsets.Add(new IVec3(0, 0, -1));

                result = offsets.ToArray();
            }
            _cache.Add(flags, result);
            return result;
        }
    }
}
