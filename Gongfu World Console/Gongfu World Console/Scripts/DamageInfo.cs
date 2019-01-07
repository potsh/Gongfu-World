using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console.Scripts
{

    public class DamageInfo
    {
        public DamageType DmgType;

        public int DmgAmount;

        public int Pierce;

        public float Ignore;

        public Dictionary<BodyArea, float> TgtAreaWeight;

        public Dictionary<BodyPartEnum, float> TgtGroupWeight;

        public Dictionary<BodyPartEnum, float> TgtPartMultiplier;

    }
}
