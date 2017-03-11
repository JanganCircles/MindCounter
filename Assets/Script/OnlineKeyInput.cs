
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlineKeyInput : NetworkBehaviour
{
    [SyncVar]
    public int Select = 0;
    [SyncVar]
    public bool Ready = false;
    public bool KeyCheck;
    public int UserID;
	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
            UserID = LobbyManager.ins.UserNumber;
        else
            UserID = 1-LobbyManager.ins.UserNumber;
    }
	
	// Update is called once per frame
    [ClientCallback]
	void Update ()
    {
        if (gameManager.ins.TempStep == gameManager.STEP.START)
        {
            Ready = false;
            Select = gameManager.ins.UserSlot[UserID].selectedSlot;
        }
        if (gameManager.ins.TempStep == gameManager.STEP.KEYCHECK && KeyCheck == true)
        {
            if (!isLocalPlayer)
                return;
            StartCoroutine("TimeLimit");
        }
        if (gameManager.ins.TempStep == gameManager.STEP.KEYCHECK)
        {
            KeyCheck = false;
            if (!isLocalPlayer)
                return;
            if (!Ready && !KeyCheck)//선입력 방지용
            {
                if (gameManager.ins.UserSlot[UserID].KeyCheck())
                {
                    StopCoroutine("TimeLimit");
                    CmdSetKey(gameManager.ins.UserSlot[UserID].selectedSlot);
                }
            }
        }
        if (gameManager.ins.TempStep == gameManager.STEP.DECISION)
        {
            Ready = true;
            KeyCheck = true;
        }
    }
    [Command]
    void CmdSetKey( int Num)
    {
        Select = Num;
        Ready = true;
    }
    IEnumerator TimeLimit()
    {
        float TempTimes = 0.0f;
        float LimitTimes = 5.0f;
        while (LimitTimes > TempTimes)
        {
            UITextSet.UIList["WaitingTimer"] = (Mathf.Round(TempTimes / 0.01f) * .01).ToString();

            yield return null;
            TempTimes += Time.unscaledDeltaTime;
        }
        CmdSetKey(gameManager.ins.UserSlot[UserID].selectedSlot);
    }
}
