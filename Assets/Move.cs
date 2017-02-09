using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int abc = 72;
        int bca = abc;
	}
	
	// Update is called once per frame
	void Update () {


        transform.Translate(Vector3.up * Time.deltaTime * 8f);
        transform.Translate(Vector3.up * Time.deltaTime * 44f);

	}
}
