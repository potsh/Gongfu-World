using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gongfu_World_Console.Scripts;
using Newtonsoft.Json;

namespace Gongfu_World_Console
{
    public class Gongfa
    {
        [JsonIgnore]
        public Character Ch;

        [JsonIgnore]
        public GongfaDef GongfaDef;

        public int Exp;

        public float LearnProgress => Math.Min( (float)Exp / GongfaDef.Exp, 1.0f );

        public int EnergyCost => GongfaDef.BaseEnergyCost;

        public int ActionCost => GongfaDef.BaseActionCost;

        public float Efficiency => Math.Min(LearnProgress * GongfaDef.CalcEfficiency(Ch), GongfaDef.MaxEfficiency);

        public int PowerTotal => Power + Ch.PrimaryAttr.Strength;
        public float DmgMultiplier => 1 + PowerTotal / 100.0f;

        public int PierceDamage => (int)(GongfaDef.BasePierceDamage * Efficiency * DmgMultiplier);
        public int CutDamage => (int)(GongfaDef.BaseCutDamage * Efficiency * DmgMultiplier);
        public int BluntDamage => (int)(GongfaDef.BaseBluntDamage * Efficiency * DmgMultiplier);
        public int EngDamage => (int)(GongfaDef.BaseEngDamage * Efficiency * DmgMultiplier);

        public int Pierce => (int)(GongfaDef.Pierce * Efficiency * DmgMultiplier);
        public float Ignore => GongfaDef.Ignore;

        public int Power => (int)(GongfaDef.BasePower * Efficiency);
        public int Speed => (int)(GongfaDef.BaseSpeed * Efficiency);
        public int Clever => (int)(GongfaDef.BaseClever * Efficiency);
    }
}
