using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console.Scripts
{

    public class DamageInfo
    {
        public Character Attacker;

        public DamageType DmgType;

        public Gongfa Gongfa;

        public int DmgAmount;

        public int Pierce;

        public float Ignore;

        public Dictionary<BodyArea, float> TgtAreaWeight;

        public Dictionary<BodyPartEnum, float> TgtGroupWeight;

        public Dictionary<BodyPartEnum, float> TgtPartMultiplier;

        public bool PenetrateOut = false;

        public DamageInfo(DamageInfo dInfo)
        {
            Attacker = dInfo.Attacker;
            DmgType = dInfo.DmgType;
            DmgAmount = dInfo.DmgAmount;
            Pierce = dInfo.Pierce;
            Ignore = dInfo.Ignore;

            TgtAreaWeight = dInfo.TgtAreaWeight;
            TgtGroupWeight = dInfo.TgtGroupWeight;
            TgtPartMultiplier = dInfo.TgtPartMultiplier;
        }

        public DamageInfo(Gongfa gf, int dmgAmount, DamageType dmgType)
        {
            Attacker = gf.Ch;
            DmgType = dmgType;
            DmgAmount = dmgAmount;
            Pierce = gf.Pierce;
            Ignore = gf.Ignore;

            TgtAreaWeight = gf.GongfaDef.TgtAreaWeight;
            TgtGroupWeight = gf.GongfaDef.TgtGroupWeight;
            TgtPartMultiplier = gf.GongfaDef.TgtPartProbMultiDict;
        }
    }
}
