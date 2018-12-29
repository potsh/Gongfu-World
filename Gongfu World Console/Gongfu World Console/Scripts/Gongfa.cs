using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gongfu_World_Console.Scripts;

namespace Gongfu_World_Console
{
    public class Gongfa
    {
        private Character ch;

        public GongfaDef gongfaDef;

        public int Exp;

        public float LearnProgress = 1.0f;

        public int EnegyCost => gongfaDef.BaseEnegyCost;

        public int ActionCost => gongfaDef.BaseActionCost;

        public float Efficiency => Math.Min(LearnProgress, gongfaDef.CalcEfficiency(ch));

        public float PhyDamage => gongfaDef.BasePhyDamage * Efficiency;

        public float EngDamage => gongfaDef.BaseEngDamage * Efficiency;

        public float Power => gongfaDef.BasePower* Efficiency;

        public float Speed => gongfaDef.BaseSpeed * Efficiency;

        public float Clever => gongfaDef.BaseClever * Efficiency;
    }
}
