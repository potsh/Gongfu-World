//using UnityEngine;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


// This class uses Reflection and Linq so it's not the fastest thing in the
// world; however I only use it in development builds where we want to allow
// game data to be easily tweaked so this isn't an issue; I would recommend
// you do the same.
namespace Gongfu_World_Console.Scripts
{
    public class CsvUtil<TDef> where TDef : new()
    {
        private static TDef _loadObj = new TDef();
        private static string _header = "";
        private static Dictionary<string, int> _fieldNameDict = null;
        private static List<string> _fieldTypeList = null;
        private static Dictionary<string, FieldInfo> _fieldDictOfT = new Dictionary<string, FieldInfo>();
        private static Dictionary<string, PropertyInfo> _propertyDictOfT = new Dictionary<string, PropertyInfo>();
        private static string _tableName = null;
        private static int _keyColNo = -1;
        private static string _keyName = "";
        private static string _fileFullPathToLoad= null;
        private static bool _strict = true;
        private static Encoding _encoding = Encoding.Default;
        private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
            {NullValueHandling = NullValueHandling.Ignore};

        // CSV分隔符
        private static char[] _quotedChars = new char[] { ',' , '"', '\n' };


        public static object LoadObjectsToDict<T>(string fileName, bool strict = true) where T : new()
        {

            Dictionary<object, T> dict = new Dictionary<object, T>();

            List<T> list = LoadObjects<T>(fileName, strict);

            if (list != null)
            {
                foreach (var obj in list)
                {
                    dict.Add(_fieldDictOfT[_keyName].GetValue(obj), obj);
                }

                return dict;
            }


            return null;
        }

        // Load a CSV into a list of struct/classes from a file where each line = 1 object
        // First line of the CSV must be a header containing property names
        // Can optionally include any other columns headed with #foo, which are ignored
        // E.g. you can include a #Description column to provide notes which are ignored
        // This method throws file exceptions if file is not found
        // Field names are matched case-insensitive for convenience
        // @param fileName File to load
        // @param strict If true, log errors if a line doesn't have enough
        //   fields as per the header. If false, ignores and just fills what it can
        public static List<T> LoadObjects<T>(string fileName, bool strict = true) where T : new()
        {
            _fileFullPathToLoad = fileName;
            _encoding = EncodingType.GetType(fileName);

            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var rdr = new StreamReader(stream))                 
                {
                    TextReader reader = Init<T>(rdr, strict);
                    return reader == null ? null : LoadObjects<T>(reader);
                }
            }
        }

        //解析文件头，获取必要信息
        private static TextReader Init<T>(TextReader rdr, bool strict = true)
        {
            GetFieldsAndProperties<T>();
            _strict = strict;

            while (true) //处理表头，前进到数据区
            {
                string line;
                if ((line = rdr.ReadLine()) != null)
                {
                    _header = _header + line + Environment.NewLine;

                    IEnumerable<string> fields = EnumerateCsvLine(line);
                    var fieldList = fields.ToList();

                    for (int i = 0; i < fieldList.Count(); i++) //去一遍字段内部的双重引号
                    {
                        fieldList[i] = fieldList[i].Replace("\"\"", "\"");
                    }

                    if (fieldList.First() == "关键字")
                    {
                        for (int i = 1; i < fieldList.Count(); i++)
                        {
                            if (fieldList[i] == "Y")
                            {
                                _keyColNo = i;
                                break;
                            }
                        }
                    }
                    else if (fieldList.First() == "变量名称")
                    {
                        _fieldNameDict = ParseHeader(fieldList);
                    }
                    else if (fieldList.First() == "变量类型")
                    {
                        for (int i = 1; i < fieldList.Count(); i++)
                        {
                            fieldList[i] = fieldList[i].Trim();
                            _fieldTypeList = fieldList;
                        }
                    }
                    else if (fieldList.First() == "BEGIN")
                    {
                        _tableName = fieldList[1];

                        if (_keyColNo != -1)
                        {
                            foreach (var pair in _fieldNameDict)
                            {
                                if (pair.Value == _keyColNo)
                                {
                                    _keyName = pair.Key;
                                    break;
                                }
                            }
                        }

                        return rdr;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private static void GetFieldsAndProperties<T>()
        {
            FieldInfo[] fi = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo[] pi = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var f in fi)
            {
                _fieldDictOfT[f.Name] = f;
            }

            foreach (var p in pi)
            {
                _propertyDictOfT[p.Name] = p;
            }
        }

        // Load a CSV into a list of struct/classes from a stream where each line = 1 object
        // First line of the CSV must be a header containing property names
        // Can optionally include any other columns headed with #foo, which are ignored
        // E.g. you can include a #Description column to provide notes which are ignored
        // Field names are matched case-insensitive for convenience
        // @param rdr Input reader
        // @param strict If true, log errors if a line doesn't have enough
        //   fields as per the header. If false, ignores and just fills what it can
        private static List<T> LoadObjects<T>(TextReader rdr) where T : new()
        {
            var ret = new List<T>();

            string line;
            bool isValueType = typeof(T).IsValueType;

            while ((line = rdr.ReadLine()) != null)
            {
                var obj = new T();
                // box manually to avoid issues with structs
                T boxed = obj;
                if (ParseLineToObject(line, destObject: ref boxed))
                {
                    // unbox value types
                    if (isValueType)
                        obj = boxed;
                    ret.Add(obj);
                }
            }
            return ret;
        }

        // Save multiple objects to a CSV file
        // Writes a header line with field names, followed by one line per
        // object with each field value in each column
        // This method throws exceptions if unable to write
        public static void SaveObjects<T>(IEnumerable<T> objs, string filename)
        {
            using (var stream = File.Open(filename, FileMode.Create))
            {
                using (var wtr = new StreamWriter(stream, _encoding))
                {
                    WriteHeader(_fieldDictOfT.Values, wtr);
                    SaveObjects(objs, wtr);
                }
            }
        }

        // Save multiple objects to a CSV stream
        // Writes a header line with field names, followed by one line per
        // object with each field value in each column
        // This method throws exceptions if unable to write
        public static void SaveObjects<T>(IEnumerable<T> objs, TextWriter w)
        {
            bool firstLine = true;
            foreach (T obj in objs)
            {
                // Good CSV files don't have a trailing newline so only add here
                if (firstLine)
                    firstLine = false;
                else
                    w.Write(Environment.NewLine);

                WriteObjectToLine(obj, w);
            }
        }

        private static void WriteHeader(IEnumerable<FieldInfo> fi, TextWriter w)
        {
            bool firstCol = true;
            foreach (FieldInfo f in fi)
            {
                // Good CSV files don't have a trailing comma so only add here
                if (firstCol)
                    firstCol = false;
                else
                    w.Write(",");

                w.Write(f.Name);
            }
            w.Write(Environment.NewLine);
        }

        private static void WriteObjectToLine<T>(T obj, TextWriter w)
        {
            bool firstCol = true;
            foreach (FieldInfo f in _fieldDictOfT.Values)
            {
                // Good CSV files don't have a trailing comma so only add here
                if (firstCol)
                    firstCol = false;
                else
                    w.Write(",");

                string val = "";

                bool isDefault = false;
                if ( typeof(ValueType).IsAssignableFrom(f.FieldType) )
                {
                    if ( f.GetValue(obj) == null || f.GetValue(obj).Equals(Activator.CreateInstance(f.FieldType)) ) 
                    {
                        isDefault = true;
                    }                    
                }
                else
                {
                    if (f.GetValue(obj) == null)
                    {
                        isDefault = true;
                    }
                }

                if ( !isDefault )
                {
                    if (typeof(ILoadFromString).IsAssignableFrom(f.FieldType))
                        val = ((ILoadFromString)f.GetValue(obj)).ToString();
                    else if (f.FieldType == typeof(string))
                        val = (string)f.GetValue(obj);
                    else if (typeof(Enum).IsAssignableFrom(f.FieldType))
                        val = f.GetValue(obj).ToString();
                    else
                        val = JsonConvert.SerializeObject(f.GetValue(obj), f.FieldType, _jsonSerializerSettings);
                }

                if (f.FieldType == typeof(float) || f.FieldType == typeof(double))
                    val = TrimFloat(val);

                // Double double-quote
                if (val.IndexOf('"') != -1)
                {
                    val = val.Replace("\"", "\"\"");
                }
                // Quote if necessary
                if (val.IndexOfAny(_quotedChars) != -1)
                {
                    val = $"\"{val}\"";
                }
                w.Write(val);
            }
        }

        // Parse the header line and return a mapping of field names to column
        // indexes. Columns which have a '#' prefix are ignored.
        private static Dictionary<string, int> ParseHeader(string header)
        {
            var headers = new Dictionary<string, int>();
            int n = 0;
            foreach (string field in EnumerateCsvLine(header))
            {
                var trimmed = field.Trim();
                if ( !trimmed.StartsWith("#") || trimmed != "" )
                {
                    trimmed = RemoveSpaces(trimmed);
                    headers[trimmed] = n;
                }
                ++n;
            }
            return headers;
        }

        // Parse the header line and return a mapping of field names to column
        // indexes. Columns which have a '#' prefix are ignored.
        private static Dictionary<string, int> ParseHeader(IEnumerable<string> header)
        {
            var headers = new Dictionary<string, int>();
            int n = 1;
            header = header.Skip(1);
            foreach (string field in header)
            {
                var trimmed = field.Trim();
                if (!trimmed.StartsWith("#") || trimmed != "")
                {
                    trimmed = RemoveSpaces(trimmed);
                    headers[trimmed] = n;
                }
                ++n;
            }
            return headers;
        }


        // Parse an object line based on the header, return true if any fields matched
        private static bool ParseLineToObject<T>(string line, ref T destObject)
        {

            string[] values = EnumerateCsvLine(line).ToArray();
            bool setAny = false;

            for (int i = 0; i < values.Length; i++)
                values[i] = values[i].Replace("\"\"", "\"");

            foreach (string field in _fieldNameDict.Keys)
            {
                int index = _fieldNameDict[field];
                if (index < values.Length)
                {
                    string val = values[index];
                    setAny = SetField(field, val, destObject) || setAny;
                }
                else if (_strict)
                {
                    /*Debug.LogWarning*/Console.WriteLine(string.Format("CsvUtil: error parsing line '{0}': not enough fields", line));
                }
            }
            return setAny;
        }

        private static object StringToObject(string val, Type t)
        {
            if (val == "")
            {
                return null;
            }
            else
            {
                object typedVal = null;
                if (typeof(ILoadFromString).IsAssignableFrom(t))
                {
                    typedVal = Activator.CreateInstance(t);
                    typedVal = ((ILoadFromString)typedVal).StringToObject(val);
                }
                else if (t == typeof(string))
                {
                    typedVal = val;
                }
                else if (typeof(Enum).IsAssignableFrom(t))
                {   
                    typedVal = TypeDescriptor.GetConverter(t).ConvertFromInvariantString(val);
                }
                else
                {
                    typedVal = JsonConvert.DeserializeObject(val, t, _jsonSerializerSettings);
                }

                return typedVal;
            }
        }

        private static bool SetField(string colName, string val, object destObject)
        {
            if (_fieldDictOfT.ContainsKey(colName))
            {
                var typedVal = StringToObject(val, _fieldDictOfT[colName].FieldType);                
                _fieldDictOfT[colName].SetValue(destObject, typedVal);
                return true;
            }
            else if (_propertyDictOfT.ContainsKey(colName))
            {

                var typedVal = StringToObject(val, _propertyDictOfT[colName].PropertyType);                              
                _propertyDictOfT[colName].SetValue(destObject, typedVal, null);
                return true;
            }

            return false;
        }

        public static object ParseString(string strValue, Type t)
        {
            if (strValue == "")
            {
                if (t.IsValueType)
                    return default(ValueType);
                else
                {
                    return null;
                }
            }

            if (typeof(ILoadFromString).IsAssignableFrom(t)) //是自定义子类型，则调用类型自身的字符串解析函数
            {
                ILoadFromString ret = (ILoadFromString)Activator.CreateInstance(t);
                return ret.StringToObject(strValue);
            }

            var cv = TypeDescriptor.GetConverter(t);
            if ( !(cv is CollectionConverter) )
            {
                return cv.ConvertFromInvariantString(strValue);
            }
            else
            {
                return ((ILoadFromLine)_loadObj).ParseString(strValue, t);
            }
        }

        public static Dictionary<TKey, TValue> ParseString<TKey, TValue>(string strValue)
        {
            if (strValue == "")
            {
                return null;
            }
            else
            {
                return StringToDictionary<TKey, TValue>(strValue);
            }
        }

        public static List<T> ParseString<T>(string strValue)
        {
            if (strValue == "")
            { 
                return null;
            }
            else
            {            
                return StringToList<T>(strValue);
            }
        }

        private static IEnumerable<string> EnumerateCsvLine(string line)
        {
            // Regex taken from http://wiki.unity3d.com/index.php?title=CSVReader
            foreach (Match m in Regex.Matches(line,
                @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                RegexOptions.ExplicitCapture))
            {
                yield return m.Groups[1].Value;
            }
        }

        private static string RemoveSpaces(string strValue)
        {
            return Regex.Replace(strValue, @"\s", string.Empty);
        }

        public static Dictionary<TKey, TValue> StringToDictionary<TKey, TValue>(string value)
        {

            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();

            string[] dicStrs = value.Split(',');
            foreach (string str in dicStrs)
            {
                string[] strs = str.Split(':');

                dic.Add((TKey)ParseString(strs[0], typeof(TKey)), (TValue)ParseString(strs[1], typeof(TValue)));
            }

            return dic;
        }

        public static List<T> StringToList<T>(string value)
        {

            List<T> list = new List<T>();

            string[] dicStrs = value.Split(';');
            foreach (string str in dicStrs)
            {
                list.Add((T)ParseString(str, typeof(T)));
            }

            return list;
        }

        private static string TrimFloat(string str)
        {

            if (str.IndexOf('.') == -1)
            {
                return str;
            }

            int i = str.Length - 1;
            for ( ; i >= 0; i--)
            {
                if (str[i] != '0')
                {
                    if (str[i] == '.')
                    {
                        i--;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }           

            return str.Substring(0, i+1);
        }

        private static bool CreateOrgForCheck(string org)
        {
            using (var stream = File.Open(_fileFullPathToLoad, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var rdr = new StreamReader(stream))
                {
                    while (true) //处理表头，前进到数据区
                    {
                        string line;
                        if ((line = rdr.ReadLine()) != null)
                        {
                            if (line.StartsWith("BEGIN"))
                            {                               
                                using (var streamOrg = File.Open(org, FileMode.Create, FileAccess.Write,
                                    FileShare.ReadWrite))
                                {
                                    using (var writer = new StreamWriter(streamOrg, _encoding))
                                    {
                                        WriteHeader(_fieldDictOfT.Values, writer);
                                        bool firstLine = true;
                                        while ((line = rdr.ReadLine()) != null)
                                        {
                                            if (firstLine)
                                                firstLine = false;
                                            else
                                                writer.Write(Environment.NewLine);

                                            // box manually to avoid issues with structs
                                            string[] values = EnumerateCsvLine(line).ToArray();

                                            bool firstCol = true;
                                            foreach (FieldInfo f in _fieldDictOfT.Values)
                                            {
                                                // Good CSV files don't have a trailing comma so only add here
                                                if (firstCol)
                                                    firstCol = false;
                                                else
                                                    writer.Write(",");
//                                                if (f.Name == "ReqAptitude")
//                                                {
//                                                    ;
//                                                }
                                                string val = values[_fieldNameDict[f.Name]];

                                                // Quote if necessary
                                                if (val.IndexOfAny(_quotedChars) != -1)
                                                {
                                                    val = $"\"{val}\"";
                                                }

                                                writer.Write(val);
                                            }
                                        }
                                        //rdrOrg.Write(rdr.ReadToEnd());
                                        return true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public static bool CompareFile(string f1, string f2)
        {

            //计算第一个文件的哈希值
            var hash = System.Security.Cryptography.HashAlgorithm.Create();
            var stream_1 = new FileStream(f1, FileMode.Open);
            byte[] hashByte_1 = hash.ComputeHash(stream_1);
            stream_1.Close();

            //计算第二个文件的哈希值
            var stream_2 = new System.IO.FileStream(f2, System.IO.FileMode.Open);
            byte[] hashByte_2 = hash.ComputeHash(stream_2);
            stream_2.Close();


            //比较两个哈希值
            if (BitConverter.ToString(hashByte_1) == BitConverter.ToString(hashByte_2))
            {
                //Console.WriteLine("比较以下两个文件：");
                //Console.WriteLine(f1);
                //Console.WriteLine(f2);
                //Console.WriteLine("√√√√√√√相等√√√√√√√");
                //Console.WriteLine();
                return true;
            }
            else
            {
                Console.WriteLine("比较以下两个文件：");
                Console.WriteLine(f1);
                Console.WriteLine(f2);
                Console.WriteLine("×××××××不相等×××××××");
                Console.WriteLine();
                return false;
            }


        }

        public static bool CheckCorrectness<T>(IEnumerable<T> objs)
        {
            string name = Path.GetFileNameWithoutExtension(_fileFullPathToLoad);
            string extension = Path.GetExtension(_fileFullPathToLoad);
            string path = Path.GetDirectoryName(_fileFullPathToLoad);

            string org = path + "/" + name + "_org" + extension;
            string exported = path + "/" + name + "_exported" + extension;

            CreateOrgForCheck(org);
            SaveObjects(objs, exported);

            return CompareFile(org, exported);
        }
    }
}
