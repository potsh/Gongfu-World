using System.Collections;
using System.Collections.Generic;
using Gongfu_World_Console.Scripts;

public class Injury
{

    public float Hp;

    private float _painPerHp = 1.0f;

    private float _bleedPerHp = 0f; 

    public bool CanMerge;

    public BodyPart Part;

    public int Source; //todo

    public float Pain => Hp * _painPerHp;

    public float BleedRate => Hp * _bleedPerHp;

}
