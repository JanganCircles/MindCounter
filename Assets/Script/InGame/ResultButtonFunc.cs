using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButtonFunc : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ReTry()
    {
        SceneManager.LoadScene("Main");
    }
    public void GotoSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
    public void ToMain()
    {
        SceneManager.LoadScene("MainManu");
    }
}
