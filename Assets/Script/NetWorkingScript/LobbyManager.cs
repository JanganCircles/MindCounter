using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkBehaviour{
    static public LobbyManager ins;

    public int UserNumber = -1;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer)
        {
            enabled = false;
            return;
        }
        else
        {
            ins = this;
        }
        if (isServer)
        {
            UserNumber = 0;
        }
        else
            UserNumber = 1;
    }
    void Enable()
    {
    }
	// Update is called once per frame
	void Update () {
    }
    

}
