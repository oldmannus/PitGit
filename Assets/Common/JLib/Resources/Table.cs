
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel;


namespace JLib.Utilities
{
    /// <summary>
    /// This is basically a 2d table read from an csv file. 
    /// This is simpler than the table builder, and only provides a set of rows and columns, 
    /// along with what column goes with what title
    /// </summary>
    public class Table
    {
        string _name;
        object[,] _data;
        Dictionary<string, int> _headerToIndex;
        Dictionary<int, string> _indexToHeader;


        public string Name
        {
            get
            {
                return _name;
            }
        }

        public object[,] GetData()
        {
            return _data;
        }

        public bool GetColumnNdxByName( string columnName, out int ndx)
        {
            return _headerToIndex.TryGetValue(columnName, out ndx);
        }

        public bool GetColumnNameByNdx(int ndx, out string columnName)
        {
            return _indexToHeader.TryGetValue(ndx, out columnName);
        }

        public void InitializeFromData(string tableName, string rawdata)
        {
            _name = tableName;

            _headerToIndex = new Dictionary<string, int>();
            _indexToHeader = new Dictionary<int, string>();

            // have to read in asset first
            List<Dictionary<string, object>> data = CSVReader.Read(rawdata);//(UnityEngine.Resources.Load(assetName) as TextAsset);

            // the list is the rows. the dictionary is the header/data pairs
            int numRows = data.Count;

            int numColumns = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (numColumns < data[i].Count)
                    numColumns = data[i].Count;
            }

            // now we should have enough info to allocate the data
            _data = new object[numColumns, numRows];
            int highestColumnAssigned = -1;
            for (int row = 0; row < data.Count; row++)
            {
                // process this row
                foreach (var column in data[row])
                {
                    int indexOfColumn = -1;
                    if (_headerToIndex.TryGetValue(column.Key, out indexOfColumn) == false)
                    {
                        // column doesn't exist, add it
                        highestColumnAssigned++;
                        _headerToIndex.Add(column.Key, highestColumnAssigned);
                        _indexToHeader.Add(highestColumnAssigned, column.Key);
                        indexOfColumn = highestColumnAssigned;
                    }

                    _data[indexOfColumn, row] = column.Value;
                }
            }
        }
    }
}
