using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gongfu_World_Console.Scripts
{
    public class CharSecondaryAttr
    {
        [JsonIgnore]
        public Character ch;

        //迅疾、精妙
        public int Quickness => ch.PrimaryAttr.Dexterity;
        public int Cunning => ch.PrimaryAttr.Dexterity;

        //闪避、拆招
        public int Dodge => ch.PrimaryAttr.Dexterity;
        public int Crack => ch.PrimaryAttr.Dexterity;

        //格挡、卸力
        public int Block;
        public int Unloading;

        //力道
        public int Power => ch.PrimaryAttr.Strength;
    }
}
