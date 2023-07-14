using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Pit.Utilities
{
    public static class MathExt
    {
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

        public static float LerpTime(float startVal, float endVal, float startTime, float endTime)
        {
            return Mathf.Lerp(startVal, endVal, (Time.time - startTime) / (endTime - startTime));
        }

        public static float Normalize(float value, float min, float max)
        {
            Debug.Assert(value >= min && value <= max);
            return (value - min) / (max-min);
        }


        // returns ndx
        public static int PickRandomFromUnnormalizedTable(float[] table, int numSamples, float roll)
        {
//###            Debug.Log(roll.ToString());
            roll *= numSamples;
//###            Debug.Log("B" + roll.ToString());
            for (int i = 0; i < table.Length; i++)
            {
                if (roll < table[i])
                {
//###                    Debug.Log("returning " + i.ToString());
                    return i;
                }
                roll -= table[i];
            }

            Debug.Assert(false);
            return table.Length - 1;
        }
    }
}
