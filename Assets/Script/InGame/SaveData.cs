using UnityEngine;
using System.Collections;

public class SaveData : MonoBehaviour {

    public static SaveData ins = null;
    public float[] RockPersent;
    public float[] ScissorPersent;
    public float[] PaperPersent;
    public float[] StillAttack;
    public int[] CriticalHit;
    public int[] GuardPersent;
    public int[] SevenCombo;

    void Awake()
    {
        if (ins == null) ins = this;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
