using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GongfaDef
{

    private string _name;

    private GongfaTypeEnum _gongfaType;

    private int _level;

    private float _baseEnegyCost;

    private int _actionCost;

    private float _maxEfficiency;

    private float _basePhyDamage;

    private float _baseEngDamage;

    private float _basePower;

    private float _baseSpeed;

    private float _baseClever;



    public float CalcEfficiency()
    {
        return 1.0f;
    }

}
