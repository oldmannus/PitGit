using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using JLib.Utilities;


namespace JLib.Utilities
{

    // this is meant to be overloaded and used as a 
    // baseclass for things that use xml-based databases

    public class XMLDatabaseEntry
    {
        public virtual void FinishedLoading() { }
    }


    public class XMLDatabase<T> where T: XMLDatabaseEntry, new()
    {
        public List<T> Entries { get; private set; }


        // ------------------------------------------------------------------
        // This scans the given node to fill out the fields of whatever type
        // we're constructing ourselves from
        // ------------------------------------------------------------------
        public virtual void AddNode(XmlNode node) 
        {
            T newT = new T();

            Type type = newT.GetType();
            System.Reflection.FieldInfo [] fields = type.GetFields(System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public);
            for (int i = 0; i < fields.Count(); i++)
            {
                FieldInfo field = fields[i];
                XmlNode xmlNode;
                
                string fieldName = field.Name;
                xmlNode = node[fieldName]; 
                
                if (xmlNode == null)
                {  
                    Dbg.Log("Field missing in xml: " + field.Name);
                    continue;
                }

                string fieldTypeName = field.FieldType.ToString();
                if (fieldTypeName == "System.String")
                {
                    field.SetValue(newT, xmlNode.InnerText);
                }
                else if (fieldTypeName == "System.Single")
                {
                    float val = (float)Convert.ToDouble(xmlNode.InnerText);
                    field.SetValue(newT, val);
                }
                else if (fieldTypeName == "System.Int32")
                {
                    int val = Convert.ToInt32(xmlNode.InnerText);
                    field.SetValue(newT, val);
                }
                else if (fieldTypeName == "System.UInt32")
                {
                    uint val = Convert.ToUInt32(xmlNode.InnerText);
                    field.SetValue(newT, val);
                }
                else
                {
                    Dbg.Log("Database reading unknown field type: " + fieldTypeName);
                }
            
            }

            newT.FinishedLoading();
            Entries.Add(newT);
        }
   


        public void Load(string filename)
        {
            Entries = new List<T>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            // we run through the nodes till we find one that goes with this screen
            XmlNode root = xmlDoc.FirstChild;
            if (root == null)
            {
                Dbg.Log("Failed to load document " + filename);
                return;
            }

            // step one is to to run over all of the Conditions
            root = root.NextSibling;  // points to conditions


            // these should all be states at the top level
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                XmlNode node = root.ChildNodes[i];
                AddNode(node);
            }
        }
    }
}
