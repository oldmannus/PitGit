using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JLib.Utilities
{
    public class PersonNameBuilder
    {
        string[] LastNameTable;
        string[] FemaleFirstNameTable;
        string[] MaleFirstNameTable;

        public void Init(string maleFirstNameFile = "malefirstnames", 
                         string femaleFirstNameFile = "femalefirstnames", 
                         string lastNameFile = "lastnames")
        {
            MaleFirstNameTable = FileUtils.ReadLinesFromAsset(maleFirstNameFile);
            FemaleFirstNameTable = FileUtils.ReadLinesFromAsset(femaleFirstNameFile);
            LastNameTable = FileUtils.ReadLinesFromAsset(lastNameFile);


            // clean up capitalization
            if (MaleFirstNameTable != null)                Clean(MaleFirstNameTable);
            if (FemaleFirstNameTable != null)              Clean(FemaleFirstNameTable);
            if (LastNameTable != null)                     Clean(LastNameTable);
        }

        public string RandomFemaleFirstName() { return FemaleFirstNameTable[Rng.RandomInt(FemaleFirstNameTable.Length)]; }
        public string RandomMaleFirstName() { return MaleFirstNameTable[Rng.RandomInt(MaleFirstNameTable.Length)]; }
        public string RandomLastName() { return LastNameTable[Rng.RandomInt(LastNameTable.Length)]; }

        

        void Clean( string[] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                if (string.IsNullOrEmpty(table[i]))
                {
                    Debug.LogError("blank entry in table");
                    Debug.Assert(false);
                }
                string lower = table[i].ToLower();
                table[i] = Char.ToUpper(lower[0]) + lower.Substring(1);

                table[i].Replace("\r\n", string.Empty);
            }
        }
    }
}
