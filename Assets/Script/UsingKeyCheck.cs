using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingKeyCheck : MonoBehaviour {
    int Index;

    public GameObject Guard;
    public GameObject Weak;
    public GameObject Middle;
    public GameObject Strong;
    int[] Costs;
    GameObject[] gm;
	// Use this for initialization
	void Start () {
        Costs = new int[] { CostData.Guard,CostData.Weak,CostData.Middle,CostData.Strong};
        gm = new GameObject[] { Guard, Weak, Middle, Strong};
        Index = name[0] == 'B' ? 0 : 1;

    }

    // Update is called once per frame
    void Update() {
        if (gameManager.ins.TempStep == gameManager.STEP.KEYCHECK)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Costs[i] <= gameManager.ins.UserStatus[Index].Cost)
                {
                    gm[i].SetActive(true);
                }
                else
                {
                    gm[i].SetActive(false);
                }
            }
        }
    }
}
