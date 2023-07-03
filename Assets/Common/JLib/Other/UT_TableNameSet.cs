
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLib.Utilities
{
    public class UT_TableStringSet
    {
        protected string _resourceName;
        Dictionary<string, List<string>> _strings;

        public List<string> GetStrings(string key)
        {
            List<string> strs;
            if (_strings.TryGetValue(key, out strs) == false)
            {
                Dbg.LogWarning("Failed to find key in UT_TableStringSet : " + key);
                return null;
            }

            return strs;
        }

        public virtual void Initialize(string nameOfNameSet)
        {
            _strings = new Dictionary<string, List<string>>();

            _resourceName = nameOfNameSet;
            Table table = new Table();
            table.InitializeFromData("NameTable", FileUtils.ReadFromAsset(nameOfNameSet));

            object[,] data = table.GetData();
            if (data == null)
            {
                Dbg.LogError("Failed to load csv asset " + nameOfNameSet);
                return;
            }

            int numColumns = data.GetLength(0);
            int numRows = data.GetLength(1);

            for (int colNdx = 0; colNdx < numColumns; colNdx++)
            {
                List<string> newStrList = new List<string>();
                for (int rowNdx = 0; rowNdx < numRows; rowNdx++)
                {
                    string val = data[colNdx, rowNdx] as string;
                    if (val != null)
                    {
                        newStrList.Add(val);
                    }
                }

                
                string columnName;
                if (table.GetColumnNameByNdx(colNdx, out columnName))
                {
                    _strings.Add(columnName, newStrList);
                }
                else
                {
                    Dbg.Assert(false, "Invalid column index? Very weird!");
                }
                
            }
        }
    }
}
