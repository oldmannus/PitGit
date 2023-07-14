using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit.Utilities
{
    [Serializable]
    public class DColor
    {
        public byte R;
        public byte G;
        public byte B;

        public DColor( byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public DColor() { }

        public static DColor Random()
        {
            return Create((float) Rng.RandomDouble(), (float) Rng.RandomDouble(), (float) Rng.RandomDouble());
        }

        public static DColor Create(float r, float g, float b)
        {
            return new DColor() { R = (byte)((255) * r), G = (byte)((255) * g), B = (byte)((255) * b) };
        }
        public static DColor Create(byte r, byte g, byte b)
        {
            return new DColor() { R = r, G = g, B = b };
        }


        public static DColor operator *(DColor c, float f)
        {
            return new DColor((byte)Math.Max(0, Math.Min(255, c.R * f)),
                            (byte)Math.Max(0, Math.Min(255, c.G * f)),
                            (byte)Math.Max(0, Math.Min(255, c.B * f)));


        }
    }
}
