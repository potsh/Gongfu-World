using System;
using System.Collections;
using System.Collections.Generic;
using Gongfu_World_Console;
using Gongfu_World_Console.Scripts;

public class Character
{
    public string Name;

    public CharHealth Health;

    public CharEnegy Enegy;

    public CharGongfa Gongfa;

    public Dictionary<GongfaTypeEnum, int> AptitudeDict; //资质

    public CharPrimaryAttr PrimaryAttr;

    public Character()
    {
    }

    public Character(string name)
    {
        Name = name;
        Health = new CharHealth(this);
        Gongfa = new CharGongfa(this);
        AptitudeDict = new Dictionary<GongfaTypeEnum, int>();
        Enegy = new CharEnegy(this);
        PrimaryAttr = new CharPrimaryAttr(this);
    }

    public static Character CreateCharacterFromData(CharacterData cd)
    {
        Character ch = new Character(cd.Name);

        ch.Health.MaxHp = ch.Health.Hp = cd.Hp;
        ch.Enegy.MaxEnegy = ch.Enegy.Enegy = cd.Enegy;

        ch.AptitudeDict.Add(GongfaTypeEnum.内功, cd.内功);
        ch.AptitudeDict.Add(GongfaTypeEnum.身法, cd.身法);
        ch.AptitudeDict.Add(GongfaTypeEnum.绝技, cd.绝技);
        ch.AptitudeDict.Add(GongfaTypeEnum.拳掌, cd.拳掌);
        ch.AptitudeDict.Add(GongfaTypeEnum.腿法, cd.腿法);

        ch.PrimaryAttr.Strength = cd.Strength;
        ch.PrimaryAttr.Dexterity = cd.Dexterity;
        ch.PrimaryAttr.Constitution = cd.Constitution;
        ch.PrimaryAttr.Comprehension = cd.Comprehension;
        ch.PrimaryAttr.Willpower = cd.Willpower;

        return ch;
    }

}
