using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Gongfu_World_Console;
using Gongfu_World_Console.Scripts;
using Newtonsoft.Json;

public class CharGongfa
{
    [JsonIgnore]
    public Character Ch;

    public Dictionary<GongfaTypeEnum, Dictionary<string, Gongfa>> GongfaDict = new Dictionary<GongfaTypeEnum, Dictionary<string, Gongfa>>();

    public CharGongfa()
    {
        intGongfaDict();
    }
    public CharGongfa(Character ch)
    {
        Ch = ch;
        intGongfaDict();
    }

    private void intGongfaDict()
    {
        foreach(GongfaTypeEnum e in Enum.GetValues(typeof(GongfaTypeEnum)))
        {
            GongfaDict.Add(e, new Dictionary<string, Gongfa>());
        }
    }

    public void LoadGongfaFromData(GongfaTypeEnum gongfaType, Dictionary<string, int> learnedGongfa)
    {
        if(learnedGongfa != null)
        {
            foreach(var lgf in learnedGongfa)
            {
                Gongfa gongfa = new Gongfa();
                gongfa.Ch = this.Ch;
                if(!Data.GongfaDefData.ContainsKey(lgf.Key))
                {
                    Console.WriteLine("ERROR: Data.GongfaDefData not contains '{0}', in {1}", lgf.Key, (new StackFrame()).GetMethod().Name);
                    return;
                }
                gongfa.GongfaDef = Data.GongfaDefData[lgf.Key];
                gongfa.Exp = lgf.Value;

                GongfaDict[gongfaType].Add(lgf.Key, gongfa);
            }
        }
    }

    public void LoadGongfaFromData(CharacterData chData)
    {
        FieldInfo[] fi = typeof(CharacterData).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (GongfaTypeEnum eValue in Enum.GetValues(typeof(GongfaTypeEnum)))
        {
            string eName = Enum.GetName(typeof(GongfaTypeEnum), eValue);
            string fName = "learned" + eName;
            foreach (var f in fi)
            {
                if (f.Name == fName)
                {
                    LoadGongfaFromData(eValue, (Dictionary<string, int>)f.GetValue(chData));
                }
            }
            
        }
    }
}
