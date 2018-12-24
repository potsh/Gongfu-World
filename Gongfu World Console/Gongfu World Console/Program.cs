using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gongfu_World_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = "D:/__Study/Projects/Unity3d/Gongfu-World/Assets/";
            string dataPath = rootPath + "Saves/";

            Character ch = new Character("testPlayer".ToString());
            ch.AptitudeDict.Add(GongfaTypeEnum.拳掌, 45);
            ch.AptitudeDict.Add(GongfaTypeEnum.内功, 55);

            string jsonData = JsonConvert.SerializeObject(ch, Formatting.Indented);
            Console.WriteLine(jsonData);

            File.WriteAllText(dataPath + ch.Name + ".json", jsonData);

            Console.WriteLine();


            Character ch2 = JsonConvert.DeserializeObject<Character>(jsonData);
            string jsonData2 = JsonConvert.SerializeObject(ch2, Formatting.Indented);
            Console.WriteLine(jsonData2);

            Console.ReadLine();
        }
    }
}
