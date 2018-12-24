using System;
using System.Collections;
using System.Collections.Generic;

public class GongfaDef
{

    public string Name;

    public GongfaTypeEnum GongfaType;

    public int Level;

    public float BaseEnegyCost;

    public int ActionCost;

    public float MaxEfficiency;

    public float BasePhyDamage;

    public float BaseEngDamage;

    public float BasePower;

    public float BaseSpeed;

    public float BaseClever;

    

    public float CalcEfficiency()
    {
        return 1.0f;
    }

}
