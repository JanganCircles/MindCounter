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
    public const int Provoke = 4;
    public const int Sp1 = 5;
    public const int Sp2 = 6;
    public const int Sp3 = 7;

    // Use this for initialization
    void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
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
        Champion[Provoke] = KeyCode.Q;
        Champion[Sp1] = KeyCode.W;
        Champion[Sp2] = KeyCode.E;
        Champion[Sp3] = KeyCode.R;


        Challenger[Scissors] = KeyCode.H;
        Challenger[Rock] = KeyCode.J;
        Challenger[Paper] = KeyCode.K;
        Challenger[Guard] = KeyCode.L;
        Challenger[Provoke] = KeyCode.Y;
        Challenger[Sp1] = KeyCode.U;
        Challenger[Sp2] = KeyCode.I;
        Challenger[Sp3] = KeyCode.O;
    }
	
	// Update is called once per frame
	void Update () {
    }
}