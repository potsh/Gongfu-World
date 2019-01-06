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

        public float PhyDamage => GongfaDef.BasePhyDamage * Efficiency;

        public float EngDamage => GongfaDef.BaseEngDamage * Efficiency;

        public float Power => GongfaDef.BasePower* Efficiency;

        public float Speed => GongfaDef.BaseSpeed * Efficiency;

        public float Clever => GongfaDef.BaseClever * Efficiency;
    }
}
