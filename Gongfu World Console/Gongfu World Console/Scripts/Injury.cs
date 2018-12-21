using System.Collections;
using System.Collections.Generic;

public class Injury
{

    public float Hp;

    private float _painPerHp;

    private float _bleedPerHp;

    public bool CanMerge;

    public BodyPart Part;

    public int Source; //todo

    public float Pain => Hp * _painPerHp;

    public float BleedRate => Hp * _bleedPerHp;

}
