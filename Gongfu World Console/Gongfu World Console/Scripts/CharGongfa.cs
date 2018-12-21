using System.Collections;
using System.Collections.Generic;

public class CharGongfa
{
    public Character Ch;

    public Dictionary<GongfaTypeEnum, List<GongfaDef>> GongfaDict = new Dictionary<GongfaTypeEnum, List<GongfaDef>>();

    public CharGongfa()
    {
    }
    public CharGongfa(Character ch)
    {
        Ch = ch;
    }
}
