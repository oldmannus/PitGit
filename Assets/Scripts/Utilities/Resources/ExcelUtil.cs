//using System;
//using System.Data;
//using System.Data.OleDb;
//using System.IO;
//


//namespace Pit.Utilities
//{
//    static class ExcelUtil
//    {
//        public class TableID
//        {
//            public string FileName = "";
//            public string SheetName = "";
//            public string TableName = "";
//            public string IdColumn = "";
//            public string ValueColumn = "";


//            const string Provider = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=";
//            const string Properties = ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";

//            public string ConnectionString { get { return Provider + FileName + Properties; } }
//        }

//        //public static DataTable ReadExcelTable(TableID id)
//        //{
//        //    DataTable dt = new DataTable();
//        //    using (OleDbConnection conn = new OleDbConnection(id.ConnectionString))
//        //    {
//        //        using (var sourceCmd = new OleDbCommand(id.TableName, conn) { CommandType = CommandType.TableDirect })
//        //        {
//        //            using (var sourceDA = new OleDbDataAdapter(sourceCmd))
//        //            {
//        //                sourceDA.Fill(dt);
//        //            }
//        //        }
//        //    }
//        //    return dt;
//        //}

//        public  static DataTable ReadExcelSheet(string sheetName, string path)
//        {
//            if (System.IO.File.Exists(path) == false)
//            {
//                Dbg.LogError("File does not exist: " + path);
//                return null;
//            }

//            using (OleDbConnection conn = new OleDbConnection())
//            {
//                DataTable dt = new DataTable();
//                string Import_FileName = path;
//                string fileExtension = Path.GetExtension(Import_FileName);
//                if (fileExtension == ".xls")
//                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
//                if (fileExtension == ".xlsx")
//                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
//                using (OleDbCommand comm = new OleDbCommand())
//                {
//                    comm.CommandText = "Select * from [" + sheetName + "$]";

//                    comm.Connection = conn;

//                    using (OleDbDataAdapter da = new OleDbDataAdapter())
//                    {
//                        da.SelectCommand = comm;
//                        da.Fill(dt);
//                        return dt;
//                    }
//                }
//            }
//        }


//        // -----------------------------------------------------------------------------------------------------------
//        public static DataSet ReadExcelFile(string fileName)
//        // -----------------------------------------------------------------------------------------------------------
//        {
//            DataSet data = new DataSet();

//            using (OleDbConnection conn = new OleDbConnection())
//            {
//                string connectionString  = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";

//                foreach (string sheetName in GetExcelSheetNames(connectionString))
//                {
//                    using (OleDbConnection con = new OleDbConnection(connectionString))
//                    {
//                        DataTable dataTable = new DataTable();
//                        string query = string.Format("SELECT * FROM [{0}]", sheetName);
//                        con.Open();
//                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, con))
//                        {
//                            adapter.Fill(dataTable);
//                            data.Tables.Add(dataTable);
//                        }
//                    }
//                }
//            }

//            return data;
//        }

//        // -----------------------------------------------------------------------------------------------------------
//        static string[] GetExcelSheetNames(string connectionString)
//        // -----------------------------------------------------------------------------------------------------------
//        {
//            OleDbConnection con = null;
//            DataTable dt = null;
//            con = new OleDbConnection(connectionString);
//            con.Open();
//            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

//            if (dt == null)
//            {
//                return null;
//            }

//            String[] excelSheetNames = new String[dt.Rows.Count];
//            int i = 0;

//            foreach (DataRow row in dt.Rows)
//            {
//                excelSheetNames[i] = row["TABLE_NAME"].ToString();
//                i++;
//            }

//            return excelSheetNames;
//        }
//    }
//}
