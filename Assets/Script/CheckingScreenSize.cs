using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckingScreenSize : MonoBehaviour {

    Text t;
    public bool isWidth;
	// Use this for initialization
	void Awake() {
        t = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isWidth)
            t.text = "Width = " + Screen.width.ToString();
        else
            t.text = "Height = " + Screen.height.ToString();     
       // t.fontSize = Screen.width;
	}

}
