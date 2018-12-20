using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private string _name;

    public CharHealth Health;

    public CharGongfa Gongfa;

    public Dictionary<int, int> AptitudeDict; //资质

    public CharPrimaryAttr PrimaryAttr;

    public CharEnegy EnegyHandle;



}
