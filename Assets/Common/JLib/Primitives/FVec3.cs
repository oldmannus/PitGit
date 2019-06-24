using System;
using System.Collections.Generic;
using System.Text;

namespace JLib.Utilities
{
    public struct FVec3
    {
        public float x, y, z;

        public static FVec3 zero { get { return _zero; } }
        
        static FVec3 _zero = new FVec3(0.0f, 0.0f, 0.0f);

        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + ")";
        }

        public FVec3(float px, float py, float pz)
        {
            x = px;
            y = py;
            z = pz;
        }



        public void Set(float px, float py, float pz)
        {
            x = px;
            y = py;
            z = pz;
        }

        public float Length
        {
            get { return (float)Math.Sqrt(x * x + y * y + z * z); }
        }

        public float LengthSqrd
        {
            get { return (x * x + y * y + z * z); }
        }

        public void Normalize()
        {
            float l = Length;
            x /= l;
            y /= l;
            z /= l;
        }
        public static FVec3 Normalize(FVec3 n)
        {
            float l = n.Length;
            n.x /= l;
            n.y /= l;
            n.z /= l;

            return n;
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


        public static FVec3 operator /(FVec3 a, FVec3 b)
        {
            FVec3 newVec;
            newVec.x = a.x / b.x;
            newVec.y = a.y / b.y;
            newVec.z = a.z / b.z;
            return newVec;
        }

        public static FVec3 operator +(FVec3 a, FVec3 b)
        {
            FVec3 newVec;
            newVec.x = a.x + b.x;
            newVec.y = a.y + b.y;
            newVec.z = a.z + b.z;
            return newVec;
        }
        public static FVec3 operator -(FVec3 b)
        {
            FVec3 newVec;
            newVec.x = -b.x;
            newVec.y = -b.y;
            newVec.z = -b.z;
            return newVec;
        }
        public static FVec3 operator -(FVec3 a, FVec3 b)
        {
            FVec3 newVec;
            newVec.x = a.x - b.x;
            newVec.y = a.y - b.y;
            newVec.z = a.z - b.z;
            return newVec;
        }
        public static FVec3 operator *(float d, FVec3 a)
        {
            FVec3 newVec;
            newVec.x = a.x * d;
            newVec.y = a.y * d;
            newVec.z = a.z * d;
            return newVec;
        }

        public static FVec3 operator *(FVec3 a, float d)
        {
            FVec3 newVec;
            newVec.x = (float)(a.x * d);
            newVec.y = (float)(a.y * d);
            newVec.z = (float)(a.z * d);
            return newVec;
        }


        public static FVec3 operator /(FVec3 a, float d)
        {
            FVec3 newVec;
            newVec.x = (float)(a.x / d);
            newVec.y = (float)(a.y / d);
            newVec.z = (float)(a.z / d);
            return newVec;
        }

        public static FVec3 operator /(FVec3 a, IVec3 d)
        {
            FVec3 newVec;
            newVec.x = (float)(a.x / d.x);
            newVec.y = (float)(a.y / d.y);
            newVec.z = (float)(a.z / d.z);
            return newVec;
        }


        public static bool operator ==(FVec3 lhs, FVec3 rhs)
        {
            return lhs.x == rhs.x &&
                   lhs.y == rhs.y &&
                   lhs.z == rhs.z;
        }
        public static bool operator !=(FVec3 lhs, FVec3 rhs)
        {
            return lhs.x != rhs.x ||
                    lhs.y != rhs.y ||
                    lhs.z != rhs.z;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return ((FVec3)obj) == this;
        }

        public override int GetHashCode()
        {
            return (int)((x * 97 - y * 71 + z * 37) * 10000);
        }
    }

}
