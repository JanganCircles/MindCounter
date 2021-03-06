﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineController : MonoBehaviour, InputController
{
    public bool RunEffect = false;
	public Transform[] SyncBar;
    public float[] KeyCatchTime  = { 0,0};
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
    public float[] GetCatchTime()
    {
        return KeyCatchTime;
    }
    IEnumerator IECheckingKey()
    {
        RunEffect = false;
        const float WaitTime = 4f;
        float Num = 0;
        bool[] CheckingOK = new bool[2];
        for (int i = 0; i < 2; i++)
        {
            KeyCatchTime[i] = 5f;
            CheckingOK[i] = false;
        }
        while (Num < WaitTime && !(CheckingOK[gameManager.CHAMPION] && CheckingOK[gameManager.CHALLANGER]))
        {
            for (int i = 0; i < 2; i++)
            {
                if (!CheckingOK[i])
                {
                    CheckingOK[i] = gameManager.ins.UserSlot[i].KeyCheck();//두 플레이어 키 체크

                    if (CheckingOK[i] && gameManager.ins.UserSlot[i].RunActive())
                    {

						EffectManager.ins.EffectRun (SyncBar[i].position, Vector3.one, i == 0 ? "LeftBar" : "RightBar",0.33f, true);
                        KeyCatchTime[i] = Num;
                    }
                    else
                        CheckingOK[i] = false;

                }
            }
            Num += Time.unscaledDeltaTime;
            UITextSet.UIList["WaitingTimer"] = (Mathf.Round(Num / 0.01f) * .01).ToString();
            yield return null;                           //체력

        }
        RunEffect = true;
    }
}