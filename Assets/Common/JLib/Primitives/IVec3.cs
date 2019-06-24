using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace JLib.Utilities
{

    public struct IVec3
    {
        public int x, y, z;

        public static IVec3 One { get { return _one; } }

        static IVec3 _one = new IVec3(1, 1, 1);

        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + ")";
        }

        public IVec3(int px, int py, int pz)
        {
            x = px;
            y = py;
            z = pz;
        }



        public void Set(int px, int py, int pz)
        {
            x = px;
            y = py;
            z = pz;
        }

        [JsonIgnore]
        public float Length
        {
            get { return (float)Math.Sqrt(x * x + y * y + z * z); }
        }

        [JsonIgnore]
        public float LengthSqrd
        {
            get { return (x * x + y * y + z * z); }
        }

        public static IVec3 operator +(IVec3 a, IVec3 b)
        {
            IVec3 newVec;
            newVec.x = a.x + b.x;
            newVec.y = a.y + b.y;
            newVec.z = a.z + b.z;
            return newVec;
        }
        public static IVec3 operator -(IVec3 b)
        {
            IVec3 newVec;
            newVec.x = -b.x;
            newVec.y = -b.y;
            newVec.z = -b.z;
            return newVec;
        }
        public static IVec3 operator -(IVec3 a, IVec3 b)
        {
            IVec3 newVec;
            newVec.x = a.x - b.x;
            newVec.y = a.y - b.y;
            newVec.z = a.z - b.z;
            return newVec;
        }
        public static IVec3 operator *(int d, IVec3 a)
        {
            IVec3 newVec;
            newVec.x = a.x * d;
            newVec.y = a.y * d;
            newVec.z = a.z * d;
            return newVec;
        }

        public static FVec3 operator *(FVec3 f, IVec3 a)
        {
            return new FVec3(f.x * a.x, f.y * a.y, f.z * a.z);
        }

        public static FVec3 operator *(IVec3 a, FVec3 f)
        {
            return new FVec3(f.x * a.x, f.y * a.y, f.z * a.z);

        }

        public IVec3(FVec3 v)
        {
            x = (int)v.x;
            y = (int)v.y;
            z = (int)v.z;
        }
        /// <summary>
        /// Multiply by float and truncate
        /// </summary>
        /// <param name="d"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static IVec3 operator *(float d, IVec3 a)
        {
            IVec3 newVec;
            newVec.x = (int)(a.x * d);
            newVec.y = (int)(a.y * d);
            newVec.z = (int)(a.z * d);
            return newVec;
        }

        public static IVec3 operator *(IVec3 a, float d)
        {
            IVec3 newVec;
            newVec.x = (int)(a.x * d);
            newVec.y = (int)(a.y * d);
            newVec.z = (int)(a.z * d);
            return newVec;
        }

        // divide and truncate
        public static IVec3 operator /(IVec3 a, float d)
        {
            IVec3 newVec;
            newVec.x = (int)(a.x * d);
            newVec.y = (int)(a.y * d);
            newVec.z = (int)(a.z * d);
            return newVec;
        }

        public static bool operator ==(IVec3 lhs, IVec3 rhs)
        {
            return lhs.x == rhs.x &&
                   lhs.y == rhs.y &&
                   lhs.z == rhs.z;
        }
        public static bool operator !=(IVec3 lhs, IVec3 rhs)
        {
            return lhs.x != rhs.x ||
                    lhs.y != rhs.y ||
                    lhs.z != rhs.z;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return ((IVec3)obj) == this;
        }

        public override int GetHashCode()
        {
            return x * 97 - y * 71 + z * 37;
        }

        /////// <summary>
        /// Clamps values from 0 to the given. 
        /// </summary>
        /// <param name="limits"></param>
        public static IVec3 Clamp0To(IVec3 vec3, IVec3 limits)
        {
            vec3.x = MathExt.Clamp(vec3.x, 0, limits.x);
            vec3.y = MathExt.Clamp(vec3.y, 0, limits.y);
            vec3.z = MathExt.Clamp(vec3.z, 0, limits.z);
            return vec3;
        }
    }

}
