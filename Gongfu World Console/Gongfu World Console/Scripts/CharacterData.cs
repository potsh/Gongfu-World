﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gongfu_World_Console
{
    public class CharacterData
    {
        public string Name;

        public int Hp;

        public int Energy;

        public int 内功;
        public int 身法;
        public int 绝技;
        public int 拳掌;
        public int 腿法;
        public int 剑法;
        public int 刀法;

        public int Strength;
        public int Dexterity;
        public int Constitution;
        public int Vitality;
        public int Comprehension;
        public int Willpower;

        public Dictionary<string, int> Learned内功;
        public Dictionary<string, int> Learned身法;
        public Dictionary<string, int> Learned绝技;
        public Dictionary<string, int> Learned拳掌;
        public Dictionary<string, int> Learned腿法;
        public Dictionary<string, int> Learned剑法;
        public Dictionary<string, int> Learned刀法;

        public int Armor;


    }
}
