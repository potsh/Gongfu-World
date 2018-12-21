using System.Collections;
using System.Collections.Generic;

public class InjurySet
{

    public Character Ch;

    public List<Injury> Injuries = new List<Injury>();

    public float PainTotal
    {
        get
        {
            float totalPain = 0;
            foreach(var injury in Injuries)
            {
                totalPain += injury.Pain;
            }

            return totalPain;
        }
    }

    public float BleedRateTotal
    {
        get
        {
            float totalBleedRate = 0;
            foreach (var injury in Injuries)
            {
                totalBleedRate += injury.BleedRate;
            }

            return totalBleedRate;
        }
    }

    public InjurySet()
    {
    }

    public InjurySet(Character ch)
    {
        Ch = ch;
    }
}
