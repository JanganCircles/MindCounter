using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotate : MonoBehaviour {
    public float StartTime = 0;
    public float RotationTime;
    public float StopAngle = 0;
    public Vector3 Axis = Vector3.up;
    public Transform Tr;
    // Use this for initialization
    void Reset()
    {
        Axis = Vector3.up;
    }
	void Start () {
        Tr = transform;
        StartCoroutine(Rotations());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetupStartTime(float T)
    {
        StartTime = T;
    }
    IEnumerator Rotations()
    {
        yield return new WaitForSeconds(StartTime+1f);
        float TempTime = 0.0f;
        while (TempTime < RotationTime)
        {
            float Angle = StopAngle / (RotationTime / Time.deltaTime);
            Tr.Rotate(Axis, Angle);
            TempTime += Time.deltaTime;
            yield return null;
        }
        Quaternion q = transform.rotation;
        q.eulerAngles = (Axis * StopAngle);
        transform.rotation = q;
    }
}
