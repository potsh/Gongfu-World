using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Gongfu_World_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = "D:/__Study/Projects/Unity3d/Gongfu-World/Assets/";
            string dataPath = rootPath + "Data/";

            Character ch = new Character("testPlayer".ToString());

            XmlSerializer serializer = new XmlSerializer(ch.GetType());
            TextWriter writer = new StreamWriter(dataPath + ch.Name + ".xml");
            serializer.Serialize(writer, ch);
            writer.Close();


            //XmlDocument xml = new XmlDocument();
            //xml.Load(dataPath + "CharacterPlayer.xml");

           

        }
    }
}
