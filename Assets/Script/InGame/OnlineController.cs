using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlineController : NetworkBehaviour {
    public int UserCtrl;
    public bool isOk;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator CheckingKey()
    {
        yield return null;
    }
}
