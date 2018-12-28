using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console.Scripts
{
    public interface ILoadFromString
    {
        object StringToObject(string str);

        string ToString();
    }
}


