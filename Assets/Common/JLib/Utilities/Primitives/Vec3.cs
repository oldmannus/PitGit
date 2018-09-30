using System;
using System.Collections.Generic;
using System.Text;

namespace JLib.Utilities
{
    public struct IVec3
    {
        public int x;
        public int y;
        public int z;


        public static IVec3 Make(int x, int y, int z)
        {
            IVec3 tmp;
            tmp.x = x;
            tmp.y = y;
            tmp.z = z;
            return tmp;
        }

        public IVec3(int px, int py)
        {
            x = px;
            y = py;
            z = 0;
        }

        public IVec3(int px, int py, int pz)
        {
            x = px;
            y = py;
            z = pz;
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

        public void Set(int px, int py, int pz)
        {
            x = px;
            y = py;
            z = pz;
        }



        public FVec3 ToFVec3()
        {
            return new FVec3(x, y, z);
        }

        //public static bool operator ==(IVector3 lhs, IVector3 rhs)
        //{
        //    return lhs.x == rhs.x &&
        //           lhs.y == rhs.y &&
        //           lhs.z == rhs.z;
        //}
        //public static bool operator !=(IVector3 lhs, IVector3 rhs)
        //{
        //    return lhs.x != rhs.x ||
        //            lhs.y != rhs.y ||
        //            lhs.z != rhs.z;
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj == null || GetType() != obj.GetType())
        //        return false;

        //    return ((IVector3)obj) == this;
        //}

        //public override int GetHashCode();
        //{
        //    return 
        //}

    }



    public struct FVec3
    {
        public float x;
        public float y;
        public float z;

        public static FVec3 zero { get { return new FVec3(0.0f, 0.0f, 0.0f); } }

        public FVec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static FVec3 operator -(FVec3 v1, FVec3 v2)
        {
            FVec3 t;
            t.x = v1.x - v2.x;
            t.y = v1.y - v2.y;
            t.z = v1.z - v2.z;
            return t;
        }

        public static FVec3 operator +(FVec3 v1, FVec3 v2)
        {
            FVec3 t;
            t.x = v1.x + v2.x;
            t.y = v1.y + v2.y;
            t.z = v1.z + v2.z;
            return t;
        }

        public static FVec3 operator *(FVec3 v, float scalar)
        {
            FVec3 newV;
            newV.x = v.x * scalar;
            newV.y = v.y * scalar;
            newV.z = v.z * scalar;
            return newV;
        }

        public static FVec3 operator /(FVec3 v, float scalar)
        {
            FVec3 newV;
            newV.x = v.x / scalar;
            newV.y = v.y / scalar;
            newV.z = v.z / scalar;
            return newV;
        }

        public bool IsZero()
        {
            return (Math.Abs(x) < 0.00001f) && (Math.Abs(y) < 0.00001f) && (Math.Abs(z) < 0.00001f);
        }

        public float Length 
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y + z * z);
            }
        }

        public void Normalize()
        {
            float l = Length;
            x /= l;
            y /= l;
            z /= l;
        }

        public void FromIVec3(IVec3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public float Dot(FVec3 B)
        {
            return x * B.x + y * B.y + z * B.z;
        }
    }
}
