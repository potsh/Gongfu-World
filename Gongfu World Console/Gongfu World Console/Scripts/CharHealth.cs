using System.Collections;
using System.Collections.Generic;
using Gongfu_World_Console;
using Gongfu_World_Console.Scripts;
using Newtonsoft.Json;

public class CharHealth
{
    [JsonIgnore]
    public Character Ch;

    public Body Body;

    public InjurySet InjurySet;

    public int MaxHp => (int)(CharacterDef.BaseHp * HealthScale);

    public int Hp;

    public bool Downed = false;

    public bool Dead = false;

    public bool Awake = true;

    public bool InPainShock => InjurySet.PainTotal >= 0.8;

    public float HealthScale => 0.04f * Ch.PrimaryAttr.Constitution + 1;

    public float EnergyProtectRate => (float) (Ch.Energy.ProtectEnergy) / MaxHp;

    public void Init(Character ch)
    {
        Ch = ch;
        Hp = MaxHp;
        InjurySet = new InjurySet(ch);
        Body = new Body(ch);       
    }

    //从csv导入数据的后处理
    public void PostLoadData()
    {
        Body.Ch = Ch;
        if (Hp == default(int))
        {
            Hp = MaxHp;
        }
        Body.PostLoadData();
    }

    public CharHealth()
    {
    }

    public CharHealth(Character ch)
    {
        Init(ch);
    }

}
