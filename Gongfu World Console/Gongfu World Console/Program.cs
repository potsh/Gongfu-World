using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            //string rootPath = "D:/__Study/Projects/Unity3d/Gongfu-World/Assets/";
            //string dataPath = rootPath + "Saves/";
            //string gongfaDefPath = @"D:\__Study\Projects\Unity3d\Gongfu-World\Assets\Data\csv\功法数据表.csv";

            //Character ch = new Character("testPlayer".ToString());
            //ch.AptitudeDict.Add(GongfaTypeEnum.拳掌, 45);
            //ch.AptitudeDict.Add(GongfaTypeEnum.内功, 55);


            //Console.WriteLine(typeof(Dictionary<GongfaTypeEnum, int>));
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Total csv file converted from excel: {0}", ExcelConvert.AllExcelToCsv());
            Console.WriteLine("Data loaded successfully: {0}", Data.LoadData());
            Console.WriteLine("All csv file imported correctly: {0}", Data.CheckData());

            CharacterData ch1 = Data.CharacterTableData.Values.ToArray()[0];
            CharacterData ch2 = Data.CharacterTableData.Values.ToArray()[1];
            CharacterData ch3 = Data.CharacterTableData.Values.ToArray()[2];
            CharacterData ch4 = Data.CharacterTableData.Values.ToArray()[3];

            //DebugTool.Debug_DeathMatch_Statistics(ch1, ch2, 1000, true);

            DebugTool.Debug_ArmorTest_Statistics(ch3, ch4, 0, 120, 1000);

            //DebugTool.Debug_ArmorTest_Once(ch1, ch2, 35, 1, true);

            //Data.DebugPrintPartCoverage();




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



//            JsonSerializerSettings jsSeting =  new JsonSerializerSettings
//                {
//                NullValueHandling = NullValueHandling.Ignore, TypeNameHandling = TypeNameHandling.None,
//                DefaultValueHandling = DefaultValueHandling.Ignore               
//            };
//            string jsonData = JsonConvert.SerializeObject(Data.BodyPartDefData[BodyPartEnum.Body], Formatting.Indented, jsSeting);
//            Console.WriteLine(jsonData);
            
//            Console.WriteLine("############################################################################");
//            jsonData = JsonConvert.SerializeObject(ch2, Formatting.Indented, jsSeting);
//            Console.WriteLine(jsonData);

            //File.WriteAllText(dataPath + ch.Name + ".json", jsonData);

            Console.WriteLine();


            stopwatch.Stop();
            Console.WriteLine($"\n\nTotal time elapsed: {stopwatch.Elapsed.TotalMilliseconds / 1000.0}");

            //            Character ch2 = JsonConvert.DeserializeObject<Character>(jsonData);
            //            string jsonData2 = JsonConvert.SerializeObject(ch, Formatting.Indented);
            //            Console.WriteLine(jsonData2);

            Console.ReadLine();
        }
    }

   
}