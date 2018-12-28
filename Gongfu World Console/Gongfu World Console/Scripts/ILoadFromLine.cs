using System;

namespace Gongfu_World_Console.Scripts
{
    public interface ILoadFromLine
    {
        object ParseString(string str, Type t);
    }
}