using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;



namespace Pit.Utilities
{
    public class GroupNameBuilder
    {
        string[] NounAdjTable;      //  Mala is Jain. The Jain Temple Ffofof.  
        string[] GroupTable;        // Jainism, Christianity


        public void Init(string filename)
        {
            string[] baseNames = FileUtils.ReadLinesFromAsset(filename);
            if (baseNames == null)
                return;

            NounAdjTable = new string[baseNames.Length];
            GroupTable = new string[baseNames.Length];

            char[] delimiters = { ':' };
            for (int i = 0; i < baseNames.Length; i++)
            {
                string[] line = baseNames[i].Split(delimiters);
                NounAdjTable[i] = line[0];
                GroupTable[i] = line[1];

            }

            // clean up capitalization
            CleanCase(NounAdjTable);
            CleanCase(GroupTable);
        }



        public void RandomName(out string noun, out string group)
        {
            int entry = Rng.RandomInt(NounAdjTable.Length);
            noun = NounAdjTable[entry];
            group = GroupTable[entry];
        }


        void CleanCase(string[] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                string lower = table[i].ToLower();
                table[i] = Char.ToUpper(lower[0]) + lower.Substring(1);
            }
        }
    }
}
