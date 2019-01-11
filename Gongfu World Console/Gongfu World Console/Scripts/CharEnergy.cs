using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;


public class CharEnergy
{
    [JsonIgnore]
    public Character Ch;

    public int MaxEnergy; //内力总量

    public int UnAllocatedEnergy => MaxEnergy - StrengthAddCost - DexterityAddCost - ConstitutionAddCost - VitalityAddCost; //未分配内力点数

    public int MaxFlowEnergy => Ch.PrimaryAttr.Vitality * 20;     //内息总量
    public int EnergyRecoverSpeed => Ch.PrimaryAttr.Vitality / 5; //内息回复

    public int EnergyFlowSpeed => Ch.PrimaryAttr.Vitality;       //活力真气回复速度
    public int MaxFlowingEnergy => Ch.PrimaryAttr.Vitality * 5;  //活力真气总量
    public int FlowingEnergy;                                    //当前活力真气值
    public int VitalityAddCost => CalcEnergyCost(VitalityAdd);   //根骨加点消耗的内力值
    public int VitalityAdd => CalcStatAdd(MaxEnergy / 2);        //根骨加点


    public int MaxProtectEnergy => ConstitutionAddCost;                 //护体真气总量
    public int ProtectEnergy;                                           //当前护体真气值
    public int ConstitutionAddCost => CalcEnergyCost(ConstitutionAdd);  //体质加点消耗的内力值
    public int ConstitutionAdd => CalcStatAdd(MaxEnergy / 4);           //体质加点

    public int StrengthAddCost => CalcEnergyCost(StrengthAdd);
    public int StrengthAdd => CalcStatAdd(MaxEnergy / 4) * 0; //TODO

    public int DexterityAddCost => CalcEnergyCost(DexterityAdd);
    public int DexterityAdd => CalcStatAdd(MaxEnergy / 4);


    //通过消耗的内力计算基础属性加点数
    private int CalcStatAdd(int e)
    {
        return (int)(0.5 * (Math.Sqrt(8.0 * e + 1) - 1));
    }

    //通过基础属性加点数计算消耗的内力
    private int CalcEnergyCost(int stat)
    {
        return (1 + stat) * stat / 2;
    }

    public void Init()
    {
        ProtectEnergy = MaxProtectEnergy;
        FlowingEnergy = MaxFlowingEnergy;
    }

    public CharEnergy()
    {
    }
    public CharEnergy(Character ch)
    {
        Ch = ch;
    }

    public void PostLoadData()
    {
        Init();
    }
}

