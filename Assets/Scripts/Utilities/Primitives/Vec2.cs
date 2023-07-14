using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit.Utilities
{
    public struct IVec2
    {
        public int x;
        public int y;
        public IVec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static IVec2 operator -(IVec2 v1, IVec2 v2)
        {
            IVec2 t;
            t.x = v1.x - v2.x;
            t.y = v1.y - v2.y;
            return t;
        }

        public static IVec2 operator +(IVec2 v1, IVec2 v2)
        {
            IVec2 t;
            t.x = v1.x + v2.x;
            t.y = v1.y + v2.y;
            return t;
        }

        public static IVec2 operator *(IVec2 v, int scalar)
        {
            IVec2 newV;
            newV.x = v.x * scalar;
            newV.y = v.y * scalar;
            return newV;
        }

        public static IVec2 operator *(IVec2 v, long scalar)
        {
            IVec2 newV;
            newV.x = (int)(v.x * scalar);
            newV.y = (int)(v.y * scalar);
            return newV;
        }

        public override string ToString()
        {
            return x + "," + y;
        }




        public void SetX(int x) { this.x = x; }
        public void SetY(int y) { this.y = y; }

        public bool IsZero()
        {
            return (Math.Abs(x) < 0.00001f) && (Math.Abs(y) < 0.00001f);
        }

        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }
    }



    public struct FVec2
    {
        public float X;
        public float Y;
        FVec2(float x, float y)
        {
            X = x;
            Y = y;
        }


        public void SetX(float x) { X = x; }
        public void SetY(float y) { Y = y; }

        public static FVec2 operator -(FVec2 v1, FVec2 v2)
        {
            FVec2 t;
            t.X =  v1.X - v2.X;
            t.Y = v1.Y - v2.Y;
            return t;
        }

        public static FVec2 operator +(FVec2 v1, FVec2 v2)
        {
            FVec2 t;
            t.X = v1.X + v2.X;
            t.Y = v1.Y + v2.Y;
            return t;
        }

        public static FVec2 operator *(FVec2 v, float scalar)
        {
            FVec2 newV;
            newV.X = v.X * scalar;
            newV.Y = v.Y * scalar;
            return newV;
        }


        public bool IsZero()
        {
            return (Math.Abs(X) < 0.00001f) && (Math.Abs(Y) < 0.00001f);
        }

        public float Length()
        {
            return (float)Math.Sqrt(X*X + Y*Y);
        }

        public void Normalize()
        {
            float l = Length();
            X /= l;
            Y /= l;
        }

       }

}
