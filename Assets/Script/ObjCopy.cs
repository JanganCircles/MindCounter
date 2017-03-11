using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjCopy : MonoBehaviour {
    public Vector2 Direction;
    public Vector2 Amount;
    GameObject GmObj;
	// Use this for initialization
	void Start () {
        Vector3 MotherPos = transform.position;
        Vector3 TempPos = Vector3.zero;
        Vector3 Scale = transform.localScale;
        for (int i = 0; i < Amount.y; i++)
        {
            for (int j = 0; j < Amount.x; j++)
            {
                TempPos.x = MotherPos.x + Scale.x * j;
                TempPos.z = MotherPos.z + Scale.z * i;
                Instantiate(GmObj, MotherPos + TempPos,Quaternion.identity);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
