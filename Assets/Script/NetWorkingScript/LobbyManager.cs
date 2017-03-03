using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkBehaviour{

    static int stServer_UserNumber;
    [SyncVar]
    int Server_UserNumber;

    public int UserNumber = -1;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            enabled = false;
            return;
        }
        if (isServer)
        {
            Server_UserNumber = 0;
            Debug.Log("서버임");
            UserNumber = 0;
        }
        else
        {
            CmdPlusUserNumber();
        }
        UserNumber = Server_UserNumber;
    }
	// Update is called once per frame
	void Update () {
        Debug.Log(stServer_UserNumber);
    }

    [Command]
    void CmdPlusUserNumber()
    {
        Debug.Log("호출됨");
        stServer_UserNumber++;
        Server_UserNumber = stServer_UserNumber;
    }

}
