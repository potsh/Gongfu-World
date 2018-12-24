using System;
using System.Collections;
using System.Collections.Generic;

public class Character
{
    public string Name;

    public CharHealth Health;

    public CharGongfa Gongfa;

    public Dictionary<GongfaTypeEnum, int> AptitudeDict; //资质

    public CharEnegy EnegyHandle;

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
        EnegyHandle = new CharEnegy(this);
        PrimaryAttr = new CharPrimaryAttr(this);
    }

}
