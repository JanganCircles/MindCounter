using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour {
    public static GameStarter ins;
    public bool Run = false;
	// Use this for initialization
    void Awake(){
        ins = this;
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Run)
        {
            GameObject[] gm0 = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] gm1 = GameObject.FindGameObjectsWithTag("Spawner");
            GameObject[] gm2 = GameObject.FindGameObjectsWithTag("Shop");
            for (int i = 0; i < gm0.Length; i++)
            {
                gm0[i].SendMessage("Run");
            }
            for (int i = 0; i < gm1.Length; i++)
            {
                gm1[i].SendMessage("Run");
            }
            for (int i = 0; i < gm2.Length; i++)
            {
                gm2[i].SendMessage("Run");
            }
            Destroy(this);
        }
	}
}
