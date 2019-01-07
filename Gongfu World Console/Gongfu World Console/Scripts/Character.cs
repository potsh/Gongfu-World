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

    public CharEnergy Energy;

    public CharGongfa Gongfa;

    public Dictionary<GongfaTypeEnum, int> AptitudeDict; //资质



    private void Init(string name)
    {
        Name = name;
        Energy = new CharEnergy(this);
        PrimaryAttr = new CharPrimaryAttr(this);
        Health = new CharHealth();
        Health.Init(this);
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
        Energy.MaxEnergy = cd.Energy;

        AptitudeDict.Add(GongfaTypeEnum.内功, cd.内功);
        AptitudeDict.Add(GongfaTypeEnum.身法, cd.身法);
        AptitudeDict.Add(GongfaTypeEnum.绝技, cd.绝技);
        AptitudeDict.Add(GongfaTypeEnum.拳掌, cd.拳掌);
        AptitudeDict.Add(GongfaTypeEnum.腿法, cd.腿法);

        PrimaryAttr.BornStrength = cd.Strength;
        PrimaryAttr.BornDexterity = cd.Dexterity;
        PrimaryAttr.BornConstitution = cd.Constitution;
        PrimaryAttr.BornVitality = cd.Vitality;
        PrimaryAttr.BornComprehension = cd.Comprehension;
        PrimaryAttr.BornWillpower = cd.Willpower;

        Gongfa.LoadGongfaFromData(cd);

        Health.PostLoadData();
    }


    public void TakeDamage(DamageInfo dInfo)
    {
        switch (dInfo.DmgType)
        {
            case DamageType.Pierce:

                break;
            case DamageType.Cut:
                break;
            case DamageType.Blunt:
                break;
            case DamageType.Energy:
                break;
            default:
                break;
        }
    }

}
