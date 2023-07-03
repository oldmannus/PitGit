
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;


namespace JLib.Utilities
{
    public static class FileUtils
    {
        public static List<string> ReadLines(string fileName)
        {
            List<string> lines = new List<string>();
            try
            {
                using (StreamReader theReader = new StreamReader(fileName, Encoding.Default))
                {
                    string line;
                    do
                    {
                        line = theReader.ReadLine();

                        if (line != null)
                            lines.Add(line);
                    }
                    while (line != null);

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

        public delegate string TextAssetResourceLoader(string filename);

        static string DefaultAssetResourceLoader(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                return sr.ReadToEnd();
            }
        }

        static TextAssetResourceLoader _defaultResourceAssetLoader = DefaultAssetResourceLoader;

        public static void SetTextAssetResourceLoader(TextAssetResourceLoader v)
        {
            _defaultResourceAssetLoader = v;
        }


        public static string ReadFromAsset(string fileName)
        {
            string asset = _defaultResourceAssetLoader(fileName);
            if (asset == null)
            {
                Dbg.LogError("Can't find resource " + fileName);
                return null;
            }

            return asset;
        }

        public static string[] ReadLinesFromAsset(string fileName)
        {
            string asset = _defaultResourceAssetLoader(fileName);
            if (asset == null)
            {
                Dbg.LogError("Can't find resource " + fileName);
                return null;
            }
            else
                return asset.Split("\n"[0]);
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
                data = JsonConvert.DeserializeObject<T>(json);
                
            }
            catch (Exception e)
            {
                Dbg.Log("Failed to jsonify info array and to file. ReadJsonObjectFromFile failed " + e.Message);
                Dbg.Assert(data == null);
            }
            return data;
        }


        public static bool WriteJsonObjectFromFile(string filename, object data)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data);
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