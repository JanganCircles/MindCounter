using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextDuration : MonoBehaviour {
    public SelectsMenuCtrl Menu;
    private int index;
	// Use this for initialization
	void Start () {
        index  = name[0] == 'B' ? 0 : 1;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = Menu.Duration[index].ToString() + "/10";
	}
}
