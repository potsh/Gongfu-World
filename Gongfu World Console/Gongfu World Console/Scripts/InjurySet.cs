using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class InjurySet
{
    [JsonIgnore]
    public Character Ch;

    public List<Injury> Injuries = new List<Injury>();

    [JsonIgnore]
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

    [JsonIgnore]
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
