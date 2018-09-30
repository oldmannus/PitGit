using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace JLib.Utilities
{

    [Serializable]
    public class TableObjectBuilder
    {
    }
    public abstract class TableObject<IdType>
    {
        public abstract bool BuildFrom(TableObjectBuilder b);
        public IdType Id;
    }


    public abstract class TableObjectManager<Tbuilder, T, IdType>
        where T : TableObject<IdType>, new()
        where Tbuilder : class, new()
    {
        Dictionary<IdType, T> _table = new Dictionary<IdType, T>();
        List<IdType> _keys = new List<IdType>();        // faster for iteration, or for picking randomly


        public T GetRandom()
        {
            if (_keys == null || _keys.Count == 0)
            {
                Dbg.Log("No valid keys loaded!");
                return null;
            }
            return Get(Rng.RandomListElement<IdType>(_keys));
        }


        public T Get(IdType id)
        {
            T bo;
            if (_table.TryGetValue(id, out bo))
                return bo;
            return null;
        }


        public virtual void LoadData(string assetname)
        {
            List<Dictionary<string, object>> data = CSVReader.Read(UnityEngine.Resources.Load(assetname) as TextAsset);
            if (data == null)
            {
                Dbg.LogError("TableObject.LoadData failed because data was null");
                return;
            }


            for (int i = 0; i < data.Count; i++)
            {
                object newObj = MakeObject();
                AddValues(newObj, data[i]);
                OnMadeObject(newObj);
            }
        }

        protected virtual object MakeObject()
        {
            return new Tbuilder();
        }

        protected virtual void OnMadeObject(object obj)
        {
            T newThing = new T();
            if (newThing.BuildFrom(obj as TableObjectBuilder))
            {
                _table.Add(newThing.Id, newThing);
                _keys.Add(newThing.Id);
            }
        }

        public static object Convert(System.Type T, string input)
        {
            var converter = TypeDescriptor.GetConverter(T);
            if (converter != null)
            {
                //Cast ConvertFromString(string text) : object to (T)
                return converter.ConvertFromString(input);
            }
            return default(T);
        }

        static void AddValues(object obj, Dictionary<string, object> data)
        {
            object sourceObject;
            foreach (FieldInfo info in obj.GetType().GetFields())
            {
                if (data.TryGetValue(info.Name, out sourceObject))
                {
                    if (Dbg.Assert(sourceObject != null, "Null in table : " + info.Name))
                        continue;

                    if (sourceObject is string) { AddValueAsString(obj, info, sourceObject); }
                    else if (info.FieldType == typeof(byte)) { info.SetValue(obj, (byte)sourceObject); }
                    else if (info.FieldType == typeof(short)) { info.SetValue(obj, (short)sourceObject); }
                    else if (info.FieldType == typeof(ushort)) { info.SetValue(obj, (ushort)sourceObject); }
                    else if (info.FieldType == typeof(int)) { info.SetValue(obj, (int)sourceObject); }
                    else if (info.FieldType == typeof(uint)) { info.SetValue(obj, (uint)sourceObject); }
                    else if (info.FieldType == typeof(long)) { info.SetValue(obj, (long)sourceObject); }
                    else if (info.FieldType == typeof(float)) { info.SetValue(obj, (float)sourceObject); }
                    else if (info.FieldType == typeof(double)) { info.SetValue(obj, (double)sourceObject); }
                    else if (info.FieldType == typeof(decimal)) { info.SetValue(obj, (decimal)sourceObject); }
                    else
                    {
                        Dbg.Assert(false, "Unknown field type in table object! " + info.FieldType.ToString());
                    }

                }
                else
                {
                    Debug.LogWarning("Failed to fill field " + info.Name);
                }
            }
        }

        private static void AddValueAsNumber(object obj, FieldInfo info, object sourceObject)
        {
        }



        private static void AddValueAsString(object obj, FieldInfo info, object sourceObject)
        {
            if (obj == null)
                obj = "";

            if (info.FieldType == typeof(string))
            {
                info.SetValue(obj, sourceObject as string);
            }
            else
            {
                // sometimes data comes in looking like a string but we want it as an int or whatever, let's do that
                object convertedObject = Convert(info.FieldType, sourceObject as string);
                info.SetValue(obj, convertedObject);
            }
        }
    }

#if ENABLE_MONO



#else
    public static class ExcelObjectReader
    {
        public delegate object CreateObjectFactoryDelegate();       // this object might be tossed if we fail to read in
        public delegate void OnCreationDelegate(object o);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void ReadFromExcelFile(string filename, string worksheetName, CreateObjectFactoryDelegate createFunc, OnCreationDelegate onReadFunc)
        {
            Debug.Assert(System.IO.File.Exists(filename));

            // NOTE: If this stops working, make sure that 32 bit OLEDB is set up and the app is set to 32 bit
            //            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", filename);
            //            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; data source={0}; Extended Properties=Excel 12.0 Xml;HDR=YES;", filename);
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; data source={0}; Extended Properties=Excel 12.0", filename);

            string selectString = string.Format("SELECT * FROM [{0}$]", worksheetName);

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectString, connection))
                {
                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    if (ds.Tables.Count == 0)
                    {
                        Debug.Log("Missing data table!");
                        return;
                    }
                    DataTable dt = ds.Tables[0];

                    for (int rowNdx = 0; rowNdx < dt.Rows.Count; rowNdx++)
                    {
                        object newObj = createFunc();

                        for (int colNdx = 0; colNdx < dt.Columns.Count; colNdx++)
                        {
                            AddValue(newObj, dt.Columns[colNdx], dt.Rows[rowNdx][dt.Columns[colNdx]]);
                        }
                        onReadFunc(newObj);
                    }
                }
            }
        }

        static void AddValue(object obj, DataColumn col, object sourceObject)
        {
            if (sourceObject == null || sourceObject.GetType() == typeof(DBNull))
                return;


            string fieldName = col.ToString();

            Type objType = obj.GetType();

            FieldInfo info = objType.GetField(fieldName);

            if (info == null)
            {
                Debug.Log(string.Format("Type {0} doesn't have field {1}", objType.ToString(), fieldName));
                return;
            }

            if (info.FieldType == typeof(int))
            {
                {
                    int? d = sourceObject as int?;
                    if (d != null)
                    {
                        info.SetValue(obj, d);
                        return;
                    }
                }
                {
                    float? d = sourceObject as float?;
                    if (d != null)
                    {
                        info.SetValue(obj, (int)d);
                        return;
                    }
                }
                {
                    double? d = sourceObject as double?;
                    if (d != null)
                    {
                        info.SetValue(obj, (int)d);
                        return;
                    }
                }
            }

            info.SetValue(obj, sourceObject);
        }
    }
#endif

    }


