
using UnityEngine;
using System.Collections;


public class UserKeyData : MonoBehaviour { 
   
    public static UserKeyData ins = null;
    public KeyCode[] Champion;
    public KeyCode[] Challenger;


    public const int Scissors = 0;
    public const int Rock = 1;
    public const int Paper = 2;
    public const int Guard = 3;
    public const int Energy = 4;
    public const int Sp1 = 5;
    public const int Sp2 = 6;
    public const int Sp3 = 7;

    // Use this for initialization
    void Awake()
    {
        ins = this;
    }
    public KeyCode[] GetKeyData(string name)
    {
        switch (name)
        {
            case "Champion":
            case "Left":
                return Champion;

            case "Challanger":
            case "Right":
                return Challenger;
        }
        return null;
    }
    void Reset()
    {
        Debug.Log("123");
        Champion = new KeyCode[8];
        Challenger = new KeyCode[8];
        Champion[Scissors] = KeyCode.A;
        Champion[Rock] = KeyCode.S;
        Champion[Paper] = KeyCode.D;
        Champion[Guard] = KeyCode.F;
        Champion[Energy] = KeyCode.Q;
        Champion[Sp1] = KeyCode.W;
        Champion[Sp2] = KeyCode.E;
        Champion[Sp3] = KeyCode.R;


        Challenger[Scissors] = KeyCode.H;
        Challenger[Rock] = KeyCode.J;
        Challenger[Paper] = KeyCode.K;
        Challenger[Guard] = KeyCode.L;
        Challenger[Energy] = KeyCode.Y;
        Challenger[Sp1] = KeyCode.U;
        Challenger[Sp2] = KeyCode.I;
        Challenger[Sp3] = KeyCode.O;
    }
	
	// Update is called once per frame
	void Update () {
    }
    public void SettingKey(int Controller,int PlayUserNum)
    {
        if (PlayUserNum == 2)
        {
           // TwoPlayerSetting(Controller);
        }
        else
        {
           // OnePlayerSetting(Controller,GameData.ins.isOnline);
        }
    }
    void TwoPlayerSetting(int controller)//2인용
    {
        switch (controller)

        {
            case gameManager.CHAMPION:
                Champion[Scissors] = KeyCode.A;
                Champion[Rock] = KeyCode.S;
                Champion[Paper] = KeyCode.D;
                Champion[Guard] = KeyCode.F;
                Champion[Energy] = KeyCode.Q;
                Champion[Sp1] = KeyCode.W;
                Champion[Sp2] = KeyCode.E;
                Champion[Sp3] = KeyCode.R;

                break;
            case gameManager.CHALLANGER:

                Challenger[Scissors] = KeyCode.H;
                Challenger[Rock] = KeyCode.J;
                Challenger[Paper] = KeyCode.K;
                Challenger[Guard] = KeyCode.L;
                Challenger[Energy] = KeyCode.Y;
                Challenger[Sp1] = KeyCode.U;
                Challenger[Sp2] = KeyCode.I;
                Challenger[Sp3] = KeyCode.O;

                break;
        }


    }
    void OnePlayerSetting(int controller,bool isOnline)
    {
        switch (controller)

        {
            case gameManager.CHAMPION:
                Champion[Scissors] = KeyCode.A;
                Champion[Rock] = KeyCode.S;
                Champion[Paper] = KeyCode.D;
                Champion[Guard] = KeyCode.J;
                Champion[Energy] = KeyCode.K;
                Champion[Sp1] = KeyCode.W;
                Champion[Sp2] = KeyCode.E;
                Champion[Sp3] = KeyCode.R;

                break;
            case gameManager.CHALLANGER:

                Challenger[Scissors] = KeyCode.A;
                Challenger[Rock] = KeyCode.S;
                Challenger[Paper] = KeyCode.D;
                Challenger[Guard] = KeyCode.J;
                Challenger[Energy] = KeyCode.K;
                Challenger[Sp1] = KeyCode.W;
                Challenger[Sp2] = KeyCode.E;
                Challenger[Sp3] = KeyCode.R;

                break;
        }
    }
    void Reset(int controller)
    {
        switch (controller)

        {
            case gameManager.CHAMPION:
                for (int i = 0; i < 8; i++)
                {
                    Champion[i] = KeyCode.None;
                }

                break;
            case gameManager.CHALLANGER:
                for (int i = 0; i < 8; i++)
                {
                    Challenger[i] = KeyCode.None;
                }
                break;
        }
    }
}