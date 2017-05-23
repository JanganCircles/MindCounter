using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneMove : MonoBehaviour {

    private static GameObject gm =null;
    public Camera cam;
    // Use this for initialization
    void Awake()
    {
        if (gm == null)
        {
            gm = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
