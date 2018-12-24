using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;


public class CharEnegy
{
    [JsonIgnore]
    public Character Ch;

    public int MaxEnegy = 0;
    public int Enegy = 0;

    public int MaxFlowEnegy = 0;
    public int FlowEnegy = 0;
    public int StrengthAdd = 0;
    public int EnegyFlowRate = 0;

    public int MaxProtectEnegy = 0;
    public int ProtectEnegy = 0;
    public int ConstitutionAdd = 0;

    public int MaxMovingEnegy = 0;
    public int MovingEnegy = 0;
    public int DexterityAdd = 0;

    public CharEnegy()
    {
    }
    public CharEnegy(Character ch)
    {
        Ch = ch;
    }
}

