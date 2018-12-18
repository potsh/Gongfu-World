using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharHealth
{
    private Character _character;

    private InjurySet _injurySet = new InjurySet();

    private Capacities _capacities = new Capacities();

    public bool Downed;

    public bool Dead;

    public bool Awake;

    public bool InPainShock => _injurySet.PainTotal >= 0.8;


}
