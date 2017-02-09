using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FuncData : MonoBehaviour {
}/*
    public delegate void HitFunc(int Damage,params float[] multiple);
    public delegate void IFunc(CharacterStatus Winer, CharacterStatus Loser);
    SelectState MyStasis;
    Dictionary<int, IFunc> WinEffect;
    float AnimationTime;
    public int[] Damage = {50,100,40,0,0,1,2,3 };
    // Use this for initialization
    void Reset()
    {
         int[] aba = { 50,100,40,0,0,1,2,3 };
        Damage = aba;
    }

    void Start () {
        MyStasis = GetComponent<SelectState>();
        WinEffect = new Dictionary<int, IFunc>();
        WinEffect.Add(KeyData.Rock, DefaultRock);
        WinEffect.Add(KeyData.Scissors, DefaultScissor);
        WinEffect.Add(KeyData.Paper, DefaultPaper);
        WinEffect.Add(KeyData.Guard, DefaultGuard);
    }
	
	// Update is called once per frame
	void Update () {
    }
    void DefaultRock(CharacterStatus win, CharacterStatus lose)
    {
        lose.Defence = true;
        Debug.Log(win.name+"의 압박승");
        lose.HpDown(Damage[MyStasis.TempKey]);
        gameManager.ins.PivotMove(win.Controller, 50);
        lose.Disable = false;
    }
    void DefaultScissor(CharacterStatus win, CharacterStatus lose)
    {
        Debug.Log(win.name + "의 화력승");
        lose.HpDown(Damage[MyStasis.TempKey]);
        gameManager.ins.PivotMove(win.Controller, 100);
        lose.DebuffReset();
        lose.Down = true;
    }
    void DefaultPaper(CharacterStatus win, CharacterStatus lose)
    {
        Debug.Log(win.name + "의 연속승");
        lose.HpDown(Damage[MyStasis.TempKey]);
        gameManager.ins.PivotMove(win.Controller, 40);
        lose.Disable = true;
    }
    void DefaultGuard(CharacterStatus win, CharacterStatus lose)
    {
        Debug.Log(win.name + "의 가드");
        int EnemyKey = lose.gameObject.GetComponent<SelectState>().TempKey;
        int Damage = lose.gameObject.GetComponent<FuncData>().Damage[EnemyKey];
        Debug.Log(Damage);
        Guard(Damage, win,null);
        lose.Disable = false;
    }
    void ActionWiner(object[] Players)
    {
        CharacterStatus Win = (CharacterStatus)Players[0];
        Win.DebuffReset();
        CharacterStatus Lose = (CharacterStatus)Players[1];
        SelectState WinnerStasis = (SelectState)Players[2];
        WinEffect[WinnerStasis.TempKey](Win, Lose);
    }
    void Guard(int Damage, CharacterStatus Mystat, params float[] multiple)
    {
        if (Mystat.WallDistance == 0)
        {
            Debug.Log(Damage);
            Debug.Log("벽까지 거리 ㅌ");
            Damage = Damage / 2;
            Debug.Log(Damage);
        }
        else
        {
            Damage = 0;
            gameManager.ins.PivotMove(Mystat.Controller, 50);
        }
        Mystat.HpDown(Damage);
    }
}
*/