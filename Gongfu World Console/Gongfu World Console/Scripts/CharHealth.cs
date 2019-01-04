using System.Collections;
using System.Collections.Generic;
using Gongfu_World_Console;
using Gongfu_World_Console.Scripts;
using Newtonsoft.Json;

public class CharHealth
{
    [JsonIgnore]
    public Character Ch;

    public Dictionary<BodyPartEnum, BodyPart> BodyPartDict;

    public InjurySet InjurySet;

    public float MaxHp => CharacterDef.BaseHp * HealthScale;

    public float Hp;

    public bool Downed = false;

    public bool Dead = false;

    public bool Awake = true;

    public bool InPainShock => InjurySet.PainTotal >= 0.8;

    public float HealthScale => 0.02f * Ch.PrimaryAttr.Constitution + 1;

    public CharHealth()
    {
    }

    public CharHealth(Character ch)
    {
        Ch = ch;
        InjurySet = new InjurySet(ch);
    }

}
