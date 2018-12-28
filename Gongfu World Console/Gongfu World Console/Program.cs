using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gongfu_World_Console.Scripts;
using Newtonsoft.Json;
//using Excel;

namespace Gongfu_World_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = "D:/__Study/Projects/Unity3d/Gongfu-World/Assets/";
            string dataPath = rootPath + "Saves/";
            string gongfaDefPath = @"D:\__Study\Projects\Unity3d\Gongfu-World\Assets\Data\csv\功法数据表.csv";

            Character ch = new Character("testPlayer".ToString());
            ch.AptitudeDict.Add(GongfaTypeEnum.拳掌, 45);
            ch.AptitudeDict.Add(GongfaTypeEnum.内功, 55);


            //Console.WriteLine(typeof(Dictionary<GongfaTypeEnum, int>));


            Console.WriteLine("Total csv file converted from excel: {0}", ExcelConvert.AllExcelToCsv());
            Console.WriteLine("Data loaded successfully: {0}", Data.LoadData());
            Console.WriteLine("All csv file imported correctly: {0}", Data.CheckData());




            //List<GongfaDef> gongfaDefList = CsvUtil<GongfaDef>.LoadObjects<GongfaDef>(gongfaDefPath);
            //var gongfaDict= CsvUtil<GongfaDef>.LoadObjectsToDict<GongfaDef>(gongfaDefPath);

            //CsvUtil<GongfaDef>.CheckCorrectness(((Dictionary<object, GongfaDef>)gongfaDict).Values);

            //Dictionary<int, int> dic = new Dictionary<int, int>{{1,2}, {3,4}};
            //Dictionary<int, int> dic = new Dictionary<int, int>();
            //dic = (Dictionary<int, int>)CsvUtil.ParseString(@"{[1,5],[2,5],[3,6],[4,8]}", typeof(Dictionary<int, int>));
            //            FieldInfo[] fi = typeof(Character).GetFields();
            //            foreach (var f in fi)
            //            {
            //                if (f.Name == "AptitudeDict" )
            //                    foreach (var pair in ch.AptitudeDict)
            //                    {
            //                        Console.WriteLine(pair);
            //                    }
            //            }


            //            string jsonData = JsonConvert.SerializeObject(ch.AptitudeDict, Formatting.Indented);
//            JsonSerializerSettings jsSeting =  new JsonSerializerSettings
//                {
//                NullValueHandling = NullValueHandling.Ignore, TypeNameHandling = TypeNameHandling.None,
//                DefaultValueHandling = DefaultValueHandling.Ignore               
//            };
//            string jsonData = JsonConvert.SerializeObject(gongfaDict, Formatting.Indented, jsSeting);
//            Console.WriteLine(jsonData);

            //File.WriteAllText(dataPath + ch.Name + ".json", jsonData);

            Console.WriteLine();


//            Character ch2 = JsonConvert.DeserializeObject<Character>(jsonData);
//            string jsonData2 = JsonConvert.SerializeObject(ch, Formatting.Indented);
//            Console.WriteLine(jsonData2);

            Console.ReadLine();
        }
    }

   
}