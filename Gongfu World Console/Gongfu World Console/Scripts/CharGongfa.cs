using System.Collections;
using System.Collections.Generic;
using Gongfu_World_Console.Scripts;
using Newtonsoft.Json;

public class CharGongfa
{
    [JsonIgnore]
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
