using System.Collections;
using System.Collections.Generic;
using Gongfu_World_Console.Scripts;

public class BodyPart
{
    public BodyPartDef BodyPartDef;

    public float Hp;

    public float Enegy;
    public float MaxEnegy;

    public float Coverage;

    public BodyPart Parent;

    public bool IsDestroyed
    {
        get
        {
            return Hp == 0;
        }
    }
}
