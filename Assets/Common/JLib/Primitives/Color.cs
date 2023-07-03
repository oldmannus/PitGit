using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLib.Utilities
{
    [Serializable]
    public class RGBColor
    {
        public byte R;
        public byte G;
        public byte B;

        public RGBColor( byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public RGBColor() { }

        public static RGBColor Random()
        {
            return Create((float) Rng.RandomDouble(), (float) Rng.RandomDouble(), (float) Rng.RandomDouble());
        }

        public static RGBColor Create(float r, float g, float b)
        {
            return new RGBColor() { R = (byte)((255) * r), G = (byte)((255) * g), B = (byte)((255) * b) };
        }
        public static RGBColor Create(byte r, byte g, byte b)
        {
            return new RGBColor() { R = r, G = g, B = b };
        }


        public static RGBColor operator *(RGBColor c, float f)
        {
            return new RGBColor((byte)Math.Max(0, Math.Min(255, c.R * f)),
                            (byte)Math.Max(0, Math.Min(255, c.G * f)),
                            (byte)Math.Max(0, Math.Min(255, c.B * f)));


        }
    }
}
