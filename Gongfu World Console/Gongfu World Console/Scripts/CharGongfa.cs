using System.Collections;
using System.Collections.Generic;
using Gongfu_World_Console.Scripts;
using Newtonsoft.Json;

public class CharGongfa
{
    [JsonIgnore]
    public Character Ch;

    public Dictionary<GongfaTypeEnum, Dictionary<string, GongfaDef>> GongfaDict = new Dictionary<GongfaTypeEnum, Dictionary<string, GongfaDef>>();

    public CharGongfa()
    {
    }
    public CharGongfa(Character ch)
    {
        Ch = ch;
    }

}
