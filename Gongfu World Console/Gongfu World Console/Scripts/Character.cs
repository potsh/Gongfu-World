using System;
using System.Collections;
using System.Collections.Generic;
using Gongfu_World_Console;
using Gongfu_World_Console.Scripts;

public class Character
{
    public string Name;

    public CharPrimaryAttr PrimaryAttr;

    public CharHealth Health;

    public CharEnegy Enegy;

    public CharGongfa Gongfa;

    public Dictionary<GongfaTypeEnum, int> AptitudeDict; //资质




    private void Init(string name)
    {
        Name = name;
        PrimaryAttr = new CharPrimaryAttr(this);
        Health = new CharHealth(this);
        Enegy = new CharEnegy(this);
        Gongfa = new CharGongfa(this);
        AptitudeDict = new Dictionary<GongfaTypeEnum, int>();
    }

    public Character()
    {
    }

    public Character(string name)
    {
        Init(name);
    }

    public Character (CharacterData cd)
    {
        Init(cd.Name);

        //Health.MaxHp = Health.Hp = cd.Hp;
        Enegy.MaxEnegy = Enegy.Enegy = cd.Enegy;

        AptitudeDict.Add(GongfaTypeEnum.内功, cd.内功);
        AptitudeDict.Add(GongfaTypeEnum.身法, cd.身法);
        AptitudeDict.Add(GongfaTypeEnum.绝技, cd.绝技);
        AptitudeDict.Add(GongfaTypeEnum.拳掌, cd.拳掌);
        AptitudeDict.Add(GongfaTypeEnum.腿法, cd.腿法);

        PrimaryAttr.Strength = cd.Strength;
        PrimaryAttr.Dexterity = cd.Dexterity;
        PrimaryAttr.Constitution = cd.Constitution;
        PrimaryAttr.Comprehension = cd.Comprehension;
        PrimaryAttr.Willpower = cd.Willpower;

        Gongfa.LoadGongfaFromData(cd);
    }

}
