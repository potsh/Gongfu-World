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
    /*class Program
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
    }*/

    class Program
    {
        static void OtherClassMethod()
        {
            Console.WriteLine("Delegate an other class's method");
        }

        static void Main(string[] args)
        {
            var test = new TestDelegate();
            test.delegateMethod = new TestDelegate.DelegateMethod(test.NonStaticMethod);
            test.delegateMethod += new TestDelegate.DelegateMethod(TestDelegate.StaticMethod);
            test.delegateMethod += Program.OtherClassMethod;
            test.RunDelegateMethods();

            Console.ReadLine();
        }
    }

    class TestDelegate
    {
        public delegate void DelegateMethod(); //声明了一个Delegate Type

        public DelegateMethod delegateMethod; //声明了一个Delegate对象

        public static void StaticMethod()
        {
            Console.WriteLine("Delegate a static method");
        }

        public void NonStaticMethod()
        {
            Console.WriteLine("Delegate a non-static method");
        }

        public void RunDelegateMethods()
        {
            if (delegateMethod != null)
            {
                Console.WriteLine("---------");
                delegateMethod.Invoke();

            }
        }
    }
}
