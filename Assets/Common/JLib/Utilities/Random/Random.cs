using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLib.Utilities
{
    public class Rng
    {
        static System.Random _random = new System.Random();

        public static double RandomDouble()             {    return _random.NextDouble(); }
        public static float  RandomFloat()              {    return (float)_random.NextDouble();   }
        public static float  RandomInt()                {    return _random.Next();        }
        public static int    RandomInt(int max)         {    return _random.Next(max);     }
        public static int    RandomInt(int min, int max){    return _random.Next(max-min) + min; }

        public static float  RandomFloat(float min, float max) { return ((max - min) * RandomFloat()) + min; }

        public static T      RandomArrayElement<T>(T[] data)
        {
            Dbg.Assert(data != null && data.Length > 0);
            return data[RandomInt(data.Length)];
        }
        public static T RandomListElement<T>(List<T> data)
        {
            Dbg.Assert(data != null && data.Count > 0);
            return data[RandomInt(data.Count)];
        }

        public static Color RandomColor()
        {
            return new Color(RandomFloat(), RandomFloat(), RandomFloat());
        }

        public static double RandomGaussian(float mean, float stdDev)
        {
            double u1 = RandomDouble(); //these are uniform(0,1) random doubles
            double u2 = RandomDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

            return mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        }
    }
}
