using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace JLib.Utilities
{
    public static class MathExt
    {
        public static T Clamp<T>(T aValue, T aMin, T aMax) where T : IComparable<T>
        {
            var _Result = aValue;
            if (aValue.CompareTo(aMax) > 0)
                _Result = aMax;
            else if (aValue.CompareTo(aMin) < 0)
                _Result = aMin;
            return _Result;
        }

        public static float Lerp(float v1, float v2, float amount)
        {
            return v1 + amount * (v2 - v1);
        }
        public static double Lerp(double v1, double v2, float amount)
        {
            return v1 + amount * (v2 - v1);
        }
        public static int Lerp(int v1, int v2, float amount)
        {
            return (int)(v1 + amount * (v2 - v1));
        }

        public static float LerpTime(float startVal, float endVal, float curTime, float startTime, float endTime)
        {
            return startVal + (startVal - endVal) * ((curTime - startTime) / (endTime - startTime));
        }

        public static float Normalize(float value, float min, float max)
        {
            Dbg.Assert(value >= min && value <= max);
            return (value - min) / (max-min);
        }


        // returns ndx
        public static int PickRandomFromUnnormalizedTable(float[] table, int numSamples, float roll)
        {
//###            Dbg.Log(roll.ToString());
            roll *= numSamples;
//###            Dbg.Log("B" + roll.ToString());
            for (int i = 0; i < table.Length; i++)
            {
                if (roll < table[i])
                {
//###                    Dbg.Log("returning " + i.ToString());
                    return i;
                }
                roll -= table[i];
            }

            Dbg.Assert(false);
            return table.Length - 1;
        }
    }
}
