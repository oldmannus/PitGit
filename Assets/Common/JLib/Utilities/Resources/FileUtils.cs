using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JLib.Utilities
{

    static class FileUtils
    {
        public static List<string> ReadLines(string fileName)
        {
            List<string> lines = new List<string>();
            try
            {
                string line;
                // Create a new StreamReader, tell it which file to read and what encoding the file
                // was saved as
                StreamReader theReader = new StreamReader(fileName, Encoding.Default);
                // Immediately clean up the reader after this block of code is done.
                // You generally use the "using" statement for potentially memory-intensive objects
                // instead of relying on garbage collection.
                // (Do not confuse this with the using directive for namespace at the 
                // beginning of a class!)
                using (theReader)
                {
                    // While there's lines left in the text file, do this:
                    do
                    {
                        line = theReader.ReadLine();

                        if (line != null)
                            lines.Add(line);
                    }
                    while (line != null);

                    // Done reading, close the reader and return true to broadcast success    
                    theReader.Close();
                    return lines;
                }
            }
            // If anything broke in the try block, we throw an exception with information
            // on what didn't work
            catch (Exception e)
            {
                Console.WriteLine("{0}\n", e.Message);
                return null;
            }
        }

        public static string[] ReadLinesFromAsset(string fileName)
        {
            TextAsset asset = Resources.Load(fileName) as TextAsset;
            if (asset == null)
            {
                Dbg.LogError("Can't find resource " + fileName);
                return null;
            }
            else
                return asset.text.Split("\n"[0]);
        }

        public static T ReadJsonObjectFromFile<T>(string filename) where T : class
        {
            if (File.Exists(filename) == false)
            {
                Dbg.Log("File does not exist  " + filename);
                return null;
            }

            T data = null;

            try
            {
                string json = System.IO.File.ReadAllText(filename);
                data = JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Dbg.Log("Failed to jsonify info array and to file. ReadJsonObjectFromFile failed " + e.Message);
                Debug.Assert(data == null);
            }
            return data;
        }


        public static bool WriteJsonObjectFromFile(string filename, object data)
        {
            try
            {
                string json = JsonUtility.ToJson(data);
                System.IO.File.WriteAllText(filename, json);
            }
            catch (Exception e)
            {
                Dbg.Log("Failed to jsonify info array and to file. WriteJsonObjectFromFile failed " + e.Message);
                return false;
            }
            return true;
        }
    }

}