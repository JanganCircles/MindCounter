using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjCopy : MonoBehaviour {
    public Vector2 Direction;
    public Vector2 Amount;
    public Vector2 Interval;
    public float RotateInterval;
    public GameObject GmObj;
    // Use this for initialization
    void Start () {
        Vector3 MotherPos = transform.position;
        Vector3 TempPos = Vector3.zero;
        Vector3 Scale = transform.localScale;
        for (int i = 0; i < Amount.y; i++)
        {
            for (int j = 0; j < Amount.x; j++)
            {
                TempPos.x = MotherPos.x + (Interval.x * j);
                TempPos.z = MotherPos.z + (Interval.y * i);
                Instantiate(GmObj, MotherPos + TempPos,Quaternion.identity).SendMessage("SetupStartTime", RotateInterval* j);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
