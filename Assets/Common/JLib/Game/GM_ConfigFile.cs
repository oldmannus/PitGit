using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using JLib.Utilities;

namespace JLib.Game
{

    public interface IConfigFile  
    {
        void Reload();
        bool Get(string configKey, out string value, string defaultValue = "");
        bool Get(string configKey, out Int32 value, Int32 defaultValue = 0);
        bool Get(string configKey, out bool value, bool defaultValue = false);
    }


    public class GM_ConfigFile : MonoBehaviour, IConfigFile
    {
        List<string> _fileNames = new List<string>();
        Dictionary<string, string> _configData = new Dictionary<string, string>();

        // -----------------------------------------------------------------------------------------------------------
        public virtual void OnInitialize()
        // -----------------------------------------------------------------------------------------------------------
        {
            LoadAll();
        }

        // -----------------------------------------------------------------------------------------------------------
        public void Reload()
        // -----------------------------------------------------------------------------------------------------------
        {
            LoadAll();
        }


        // -----------------------------------------------------------------------------------------------------------
        void LoadAll()
        // -----------------------------------------------------------------------------------------------------------
        {
            _configData.Clear();
            for (int i = 0; i < _fileNames.Count; i++)
            {
                LoadConfigurationDataFromFile(_fileNames[i]);
            }
        }


        // -----------------------------------------------------------------------------------------------------------
        /// Note that this always sticks a good value into "value", either what was found or the default
        /// Returns true if it was a value found in table
        public bool Get(string configKey, out string value, string defaultValue = "")
        // -----------------------------------------------------------------------------------------------------------
        {
            if (_configData.TryGetValue(configKey.ToLower(), out value))
                return true;

            Dbg.Log("ConfigFile failed to find key " + configKey + " adding key and value |" + defaultValue + "|");
            _configData.Add(configKey, defaultValue);
            value = defaultValue;
            return false;
        }


        // -----------------------------------------------------------------------------------------------------------
        // note that get always returns a valid value, either the default or what was there. 
        // the bool returns if it was found in table
        public bool Get(string configKey, out Int32 value, Int32 defaultValue = 0)
        // -----------------------------------------------------------------------------------------------------------
        {
            string stringVal;
            bool wasFound = Get(configKey, out stringVal, defaultValue.ToString());
            if (wasFound)
            {
                Int32 parsedValue;
                if (Int32.TryParse(stringVal, out parsedValue))
                {
                    value = parsedValue;
                    return true;
                }
            }

            value = defaultValue;
            return false;
        }

        // -----------------------------------------------------------------------------------------------------------
        // note that get always returns a valid value, either the default or what was there. 
        // the bool returns if it was found in table
        public bool Get(string configKey, out bool value, bool defaultValue = false)
        // -----------------------------------------------------------------------------------------------------------
        {
            string stringVal;
            bool wasFound = Get(configKey, out stringVal, defaultValue.ToString());
            if (wasFound)
            {
                bool parsedValue;
                if (bool.TryParse(stringVal, out parsedValue))
                {
                    value = parsedValue;
                    return true;
                }
            }

            value = defaultValue;
            return false;
        }



        // -----------------------------------------------------------------------------------------------------------
        public bool ContainsKey(string configKey)
        // -----------------------------------------------------------------------------------------------------------
        {
            return _configData.ContainsKey(configKey.ToLower());
        }

        // -----------------------------------------------------------------------------------------------------------
        void LoadConfigurationDataFromFile(string fileName)
        // -----------------------------------------------------------------------------------------------------------
        {
            if (File.Exists(fileName))
            {
                Dbg.Log("Reading configuration data from " + fileName);

                string currentLine;
                StringBuilder logConfigData = new StringBuilder();

                using (StreamReader reader = new StreamReader(fileName))
                {
                    while ((currentLine = reader.ReadLine()) != null)
                    {
                        currentLine = currentLine.Trim();

                        if (!string.IsNullOrEmpty(currentLine) && string.Compare(currentLine, 0, "#", 0, 1) != 0)
                        {
                            string configKey;
                            string configValue;

                            if (ExtractConfigDataFromLine(currentLine, out configKey, out configValue))
                            {
                                logConfigData.Append(string.Format("{0} = {1}\n", configKey, configValue));
                                _configData[configKey] = configValue;
                            }
                            else
                            {
                                Dbg.LogWarning(string.Format("Unable to parse configuration line: '{0}'", currentLine));
                            }
                        }
                    }

                    if (logConfigData.Length > 0)
                    {
                        logConfigData.Insert(0, "Configuration values:\n");
                        Dbg.Log(logConfigData);
                    }
                }
            }
            else
            {
                Dbg.Log(string.Format("Config file '{0}' not found.", fileName));
            }
        }

        // -----------------------------------------------------------------------------------------------------------
        bool ExtractConfigDataFromLine(string line, out string configKey, out string configValue)
        // -----------------------------------------------------------------------------------------------------------
        {
            string[] lineItems = line.Split('=');

            if (lineItems.Length == 2)
            {
                configKey = lineItems[0].Trim().ToLower();
                configValue = lineItems[1].Trim();
                return true;
            }
            else
            {
                configKey = "";
                configValue = "";
                return false;
            }
        }

    }
}
