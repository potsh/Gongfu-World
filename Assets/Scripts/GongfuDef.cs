using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GongfuDef
{
    public static enum GongfuTypeEnum
    {
        内功,
        身法,
        绝技,
        拳掌
    }


    private string _name;

    private int _gongfuType; //内功 身法 绝技 拳掌...

    private float _baseDamage;

    private float _baseSpeed;

    private float _baseClever;

    private float _basePower;

    private float _baseEnegyCost;

    private float _efficiency;


    public float CalcEfficiency()
    {
        return 1.0f;
    }

}
