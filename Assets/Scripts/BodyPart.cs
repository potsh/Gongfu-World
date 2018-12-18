using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public BodyPartDef BodyPartDef;

    public float Hp;

    public float Enegy;
    public float MaxEnegy;

    public float Coverage;

    public BodyPart Parent;

    public bool IsDestroyed => Hp == 0;







    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
