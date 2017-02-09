using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveData : MonoBehaviour {

    public static SaveData ins = null;
    public const int Try = 0;
    public const int Success = 2;
    private float[] RockData;
    private float[] ScissorData;
    private float[] PaperData;
    private float[] StillAttack;
    private float[] CriticalHit;
    private float[] GuardPersent;
    private int Turn;
    public Dictionary<TYPE, float[]> Data;
    public enum TYPE
    {
        ROCK        ,
        SCISSOR     ,
        PAPER       ,
        STILL       ,
        CRITICAL    ,
        GUARD       
    }
    void Awake()
    {
        Debug.Log("저장데이터");
        if (ins == null) ins = this;
        Data = new Dictionary<TYPE, float[]>();

        RockData = new float[4];
        ScissorData = new float[4];
        PaperData = new float[4];
        CriticalHit = new float[2];
        GuardPersent = new float[4];
        StillAttack = new float[2];
        Data.Add(TYPE.ROCK      , RockData);
        Data.Add(TYPE.SCISSOR   , ScissorData);
        Data.Add(TYPE.PAPER     , PaperData);
        Data.Add(TYPE.STILL     , StillAttack);
        Data.Add(TYPE.CRITICAL  , CriticalHit);
        Data.Add(TYPE.GUARD     , GuardPersent);
    }
	// Use this for initialization
	void Start () {

    }
    public void AddData(TYPE Type, int Controller, int Info, int Value)
    {
        Data[Type][Controller + Info] += Value;
    }
    public void AddData(TYPE Type, int Controller, int Value)
    {
        AddData(Type, Controller, 0, Value);
    }
	// Update is called once per frame
	void Update () {
        Turn = gameManager.ins.Turn;

    }
    public void ShowDebug()
    {
        int Champ = gameManager.CHAMPION;
        int Chall = gameManager.CHALLANGER;

        Debug.Log("챔피언 압박 " + GetPersent(RockData[Champ + Try] ,RockData[Champ + Success]) + "% " + RockData[Champ + Success] + "/" + RockData[Champ + Try]);
        Debug.Log("챔피언 화력 " + GetPersent(ScissorData[Champ + Try] , ScissorData[Champ + Success]) + "% " + ScissorData[Champ + Success] + "/" + ScissorData[Champ + Try]);
        Debug.Log("챔피언 연속 " + GetPersent(PaperData[Champ + Try] , PaperData[Champ + Success]) + "% " + PaperData[Champ + Success] + "/" + PaperData[Champ + Try]);
        Debug.Log("챔피언 공격턴유지율 " + GetPersent(Turn,StillAttack[Champ]) + "% " + StillAttack[Champ] + "/" + Turn);
        Debug.Log("챔피언 가드율 " + GetPersent(GuardPersent[Champ + Try], GuardPersent[Champ + Success]) + "% " + GuardPersent[Champ + Success] + "/" + GuardPersent[Champ + Try]);
        Debug.Log("챔피언 크리티컬횟수 " + CriticalHit[Champ] + "회");
        Debug.Log("-----------");
        Debug.Log("챌린저 압박 " + GetPersent(RockData[Chall + Try] , RockData[Chall + Success]) + "% " + RockData[Chall + Success] + "/" + RockData[Chall + Try]);
        Debug.Log("챌린저 화력 " + GetPersent(ScissorData[Chall + Try] , ScissorData[Chall + Success]) + "% " + ScissorData[Chall + Success] + "/" + ScissorData[Chall + Try]);
        Debug.Log("챌린저 연속 " + GetPersent(PaperData[Chall + Try] , PaperData[Chall + Success]) + "% " + PaperData[Chall + Success] + "/" + PaperData[Chall + Try]);
        Debug.Log("챌린저 공격턴유지율 " + GetPersent(Turn,StillAttack[Chall]) + "% " + StillAttack[Chall] + "/" + Turn);
        Debug.Log("챌린저 가드율 " + GetPersent(GuardPersent[Chall + Try] , GuardPersent[Chall + Success]) + "% " + GuardPersent[Chall + Success] + "/" + GuardPersent[Chall + Try]);
        Debug.Log("챌린저 크리티컬횟수 " + CriticalHit[Chall] + "회");
    }
    public int GetPersent(float Max,float Value)
    {
        if (Max == 0)
            return 0;
        float result = Value / Max;
        result *= 100;
        return (int)result;
    }
}
