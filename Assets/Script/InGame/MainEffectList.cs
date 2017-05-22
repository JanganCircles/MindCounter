using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEffectList : MonoBehaviour {
    public string StrongHit;
    public string MiddleHit;
    public string WeakHit;
    public string GuardHit;
    public string Energy;
    public static MainEffectList ins;
    // Use this for initialization
    void Start () {
        ins = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
