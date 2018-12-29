using System.Collections;
using System.Collections.Generic;
using Gongfu_World_Console;
using Newtonsoft.Json;

public class CharHealth
{
    [JsonIgnore]
    public Character Ch;

    public InjurySet InjurySet;

    public float MaxHp = CharacterDef.BaseHp;

    public float Hp = CharacterDef.BaseHp;

    public bool Downed = false;

    public bool Dead = false;

    public bool Awake = true;

    public bool InPainShock => InjurySet.PainTotal >= 0.8;

    public CharHealth()
    {
    }

    public CharHealth(Character ch)
    {
        Ch = ch;
        InjurySet = new InjurySet(ch);
    }

}
