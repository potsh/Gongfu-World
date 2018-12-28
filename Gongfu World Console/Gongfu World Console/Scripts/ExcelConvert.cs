using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;

namespace Gongfu_World_Console.Scripts
{
    public class ExcelConvert
    {

        private static Dictionary<string, string> _convertedCsvDict = new Dictionary<string, string>();

        //获取一个Excel Sheet
        private static DataTable GetTable(IExcelDataReader excelReader)
        {
            DataTable dt = new DataTable();
            dt.TableName = excelReader.Name;

            bool isInit = false;
            string[] ItemArray = null;
            int rowsNum = 0;
            while (excelReader.Read())
            {
                rowsNum++;
                if (!isInit) 
                {
                    isInit = true;
                    for (int i = 0; i < excelReader.FieldCount + 1; i++) //dt开头插入空行, 标记总列数
                    {
                        dt.Columns.Add("", typeof(string));
                    }

                    ItemArray = new string[excelReader.FieldCount];
                }

                /*if (excelReader.IsDBNull(0)) //如果第0列为空，则跳过该行
            {
                continue;
            }*/

                for (int i = 0; i < excelReader.FieldCount; i++)
                {
                    string value = excelReader.IsDBNull(i) ? "" : excelReader[i].ToString();
                    ItemArray[i] = value;
                }

                dt.Rows.Add(ItemArray);
            }

            return dt;
        }

        //获取Excel文件中的所有Sheet
        public static DataSet GetDataSet(string path)
        {
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //DataSet ds = excelReader.AsDataSet();//excel有空时会报错

            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Choose one of either 1 or 2:

                // 1. Use the reader methods
                /*do
            {
                while (reader.Read())
                {
                    // reader.GetDouble(0);
                }
            } while (reader.NextResult());*/

                // 2. Use the AsDataSet extension method
                return reader.AsDataSet();

                // The result of each spreadsheet is in result.Tables
            }       

            /*DataSet ds = new DataSet();
        do
        {
            DataTable dt = GetTable(excelReader);
            ds.Merge(dt);
        } while (excelReader.NextResult());

        excelReader.Close();
        excelReader.Dispose();
        stream.Close();
        stream.Dispose();
        return ds;*/
        }

        //获取Excel文件中的第一个Sheet
        public static DataTable GetFirstDataTable(string path)
        {
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataTable dt = GetTable(excelReader);

            excelReader.Close();
            excelReader.Dispose();
            stream.Close();
            stream.Dispose();
            return dt;
        }

        public static void DataTableToCsv(DataTable dt, string path)
        {
            var fi = new FileInfo(path);
            if (fi.Directory != null && !fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            var sw = new StreamWriter(fs, Encoding.UTF8);
            var data = "";
            //写出列名称
            /*for (var i = 0; i < dt.Columns.Count; i++)
        {
            data += dt.Columns[i].ColumnName;
            if (i < dt.Columns.Count - 1)
            {
                data += ",";
            }
        }*/

            //sw.WriteLine(data);
            //写出各行数据
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    var str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\""); //替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(',') || str.Contains('"')
                                          || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                    {
                        str = $"\"{str}\"";
                    }

                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }

                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
        }

        private static bool CheckValidExcelData(DataTable dt)
        {
            if (dt.Columns.Count <= 1)
            {
                return false;
            }

            int start = Math.Min(2, dt.Rows.Count);
            int end = Math.Min(20, dt.Rows.Count);

            while (start <= end)
            {
                if (dt.Rows[start][0].ToString() == "BEGIN")
                {
                    return true;
                }

                start++;
            }
            return false;
        }

        public static int AllExcelToCsv()
        {
            int count = 0;

            DirectoryInfo excelRootInfo = new DirectoryInfo(Find.DataExcelPath);

            foreach (FileInfo f in excelRootInfo.GetFiles())
            {
                if ((f.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    continue;
                }

                try
                {
                    count += ExcelToCsv(f.FullName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return count;
        }

        public static int ExcelToCsv(string path)
        {
            DataSet ds = GetDataSet(path);
            FileInfo f = new FileInfo(path);

            ds.DataSetName = f.Name;

            int count = 0;

            foreach (DataTable dt in ds.Tables)
            {
                if (CheckValidExcelData(dt))
                {
                    string sheetName = dt.TableName;

                    if (_convertedCsvDict.ContainsKey(sheetName))
                    {
                        string error =
                            $"ERROR: csv name conflict! csv '{sheetName}' both exist in excel '{_convertedCsvDict[sheetName]}' and '{dt.DataSet.DataSetName}'";

                        throw new Exception(error);
                    }

                    _convertedCsvDict.Add(sheetName, dt.DataSet.DataSetName);

                    DataTableToCsv(dt, Find.DataCsvPath + sheetName + ".csv");
                    count++;
                }
            }

            return count;
        }
    }
}

