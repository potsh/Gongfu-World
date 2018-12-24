using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CharHealth
{
    [JsonIgnore]
    public Character Ch;

    public InjurySet InjurySet;

    public float Hp = CharacterDef.BaseHp;

    public bool Downed = false;

    public bool Dead = false;

    public bool Awake = true;

    public CharHealth()
    {
    }

    public CharHealth(Character ch)
    {
        Ch = ch;
        InjurySet = new InjurySet(ch);
    }

    public bool InPainShock => InjurySet.PainTotal >= 0.8;


}
