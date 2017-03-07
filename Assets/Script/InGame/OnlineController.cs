using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnlineController : NetworkBehaviour, InputController {
    

    public int UserCtrl;
    public bool RunEffect;
    GameObject[] objs;
    // Use this for initialization

    void Awake ()
    {
        UserCtrl = LobbyManager.ins.UserNumber ;
    }
	
    public bool CheckingRunEffect()
    {
        return RunEffect;
    }
    public void CheckingKey()//이거씀
    {
        Debug.Log("키입력받을준비");
        RunEffect = false;
        StartCoroutine(IECheckingKey());
    }
    IEnumerator IECheckingKey()
    {
        OnlineKeyInput ChamsInput = null;
        OnlineKeyInput ChallInput = null;
         objs = GameObject.FindGameObjectsWithTag("OnlinePlayer");
            OnlineKeyInput inputs = objs[0].GetComponent<OnlineKeyInput>();
        Debug.Log("inputs.UserID : " + inputs.UserID);
        if (inputs.UserID == gameManager.CHAMPION)
        {
            Debug.Log("챔피언임");
            ChamsInput = inputs;
            ChallInput = objs[1].GetComponent<OnlineKeyInput>();
        }
        else
        {
            ChallInput = inputs;
            ChamsInput = objs[1].GetComponent<OnlineKeyInput>();
        }
        while (!ChamsInput.Ready || !ChallInput.Ready)
        {
            yield return null;
        }

        gameManager.ins.UserSlot[gameManager.CHAMPION].selectedSlot = ChamsInput.Select;
        gameManager.ins.UserSlot[gameManager.CHALLANGER].selectedSlot = ChallInput.Select;
        Debug.Log("챔프선택" + ChamsInput.Select);
        Debug.Log("챌져선택" + ChallInput.Select);

        for (int i = 0; i < 2; i++)
        {
            gameManager.ins.UserSlot[i].RunActive();
        }
        RunEffect = true;
    }

}
public interface InputController
{
    void CheckingKey();
    bool CheckingRunEffect();
}