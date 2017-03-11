using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineController : MonoBehaviour, InputController
{
    public bool RunEffect = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
    public bool CheckingRunEffect()
    {
        return RunEffect;
    }
    public void CheckingKey()//이거씀
    {
        RunEffect = false;
        StartCoroutine(IECheckingKey());
    }
    IEnumerator IECheckingKey()
    {
        RunEffect = false;
        const float WaitTime = 5f;
        float Num = 0;
        bool[] CheckingOK = new bool[2];
        for (int i = 0; i < 2; i++)
        {
            CheckingOK[i] = false;
        }
        while (Num < WaitTime && !(CheckingOK[gameManager.CHAMPION] && CheckingOK[gameManager.CHALLANGER]))
        {
            for (int i = 0; i < 2; i++)
            {
                if (!CheckingOK[i])
                {
                    CheckingOK[i] = gameManager.ins.UserSlot[i].KeyCheck();//두 플레이어 키 체크

                    if (CheckingOK[i])
                        gameManager.ins.UserSlot[i].RunActive();
                }
            }
            Num += Time.unscaledDeltaTime;
            UITextSet.UIList["WaitingTimer"] = (Mathf.Round(Num / 0.01f) * .01).ToString();
            yield return null;                           //체력

        }
        RunEffect = true;
    }
}