using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JLib.Utilities
{
    // this is done by a set of first and last names. The sets of names are paired by nationality
    // so we have a set of german first and last names, and we randomly select from each list
    // ### add gender later
    // ### faster with arrays
    public class NameGenerator
    {
        public class Nationality
        {
            public List<string> mFirstNames;
            public List<string> mLastNames;

            public Nationality()
            {
                mFirstNames = new List<string>();
                mLastNames = new List<string>();

            }
        }

        List<Nationality> mNationalities = new List<Nationality>();



        public string GenerateName()
        {
            // pick a nationality
            int nationalityNdx = Rng.RandomInt(mNationalities.Count());
            Nationality n = mNationalities.ElementAt(nationalityNdx);

            int numFirstOfThatNationality = n.mFirstNames.Count();
            int numLastOfThatNationality = n.mLastNames.Count();

            int firstNameNdx = Rng.RandomInt(numFirstOfThatNationality);
            int lastNameNdx = Rng.RandomInt(numLastOfThatNationality);


            return n.mFirstNames.ElementAt(firstNameNdx) + ' ' + n.mLastNames.ElementAt(lastNameNdx);
        }

        private void LoadStringFile(string filename, List<string> stuff)
        {
            System.IO.StreamReader r= new StreamReader(filename);
            string line;
            while (true)
            {
                line = r.ReadLine();
                if (line == null)
                    return;
                stuff.Add(line);
            }

        }

        public void LoadTables()
        {
            System.IO.StreamReader masterFileReader = new StreamReader("Names/MasterTable.txt");
        
            string masterFileReaderLine;
            while (true)
            {
                masterFileReaderLine = masterFileReader.ReadLine();
                if (masterFileReaderLine == null)
                    break;
                
                // each line says something like "german", and we add "_first.txt" and "_last.txt" to find
                // all the language files
                Nationality nat = new Nationality();

                string firstNameFile = "Names/" + masterFileReaderLine + "_first.txt";
                string secondNameFile = "Names/" + masterFileReaderLine + "_last.txt";
                LoadStringFile(firstNameFile, nat.mFirstNames);
                LoadStringFile(secondNameFile, nat.mLastNames);

                mNationalities.Add(nat);

            }        
        }
    }
}
