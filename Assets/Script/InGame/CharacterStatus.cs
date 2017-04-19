using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {

    public int Controller;
    public int MaxHP;               //최대체력
    public int HP;                  //현재체력
    public bool Guard;              //가드함ㅋ
    public float WallDistance;      //벽까지의 거리

    //Rock = 압박, Scissor = 화력 , Paper = 연속
    public int AttackType;          //공격타입
    public int MaxCost;             //최대 코스트
    public int Cost;                //현재 코스트


    public bool usingPotion;        //현재턴에 포션을 사용했는가.
    public bool isSuperArmor;       //슈아인가(넉백안됨)
    public const int ROCK   = 0;
    public const int SCISSOR = 1;
    public const int PAPER  = 2;

    private bool isYellowCanSee;
    void Reset()
    {
        isYellowCanSee = false;
        Cost = (MaxCost = CharacterData.Mp) / 2;
        Guard = false;
        SetMaxHP(false, CharacterData.Hp);
        WallDistance = 600;
    }
    void Start()
    {
        Vector2 Hps = new Vector2();
        Hps.x = HP;
        Hps.y = MaxHP;
        UIProgressBar.SetData((Controller == 0 ? "Champion" : "Challanger") + "HP", Hps);
        usingPotion = false;
    }
    public void TurnStart()
    {
        usingPotion = false;
        isSuperArmor = false;
    }
    public void HpDown_Debuff(int Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            HP = 1;
        }
        HPProgressBar();
    }
    public void HpDown(int Damage)
    {
        HP -= Damage;
        HPProgressBar();

    }
    public void SetMaxHP(bool isGameRunning,int _MaxHP)
    {
        if (isGameRunning)
        {
            float PerHp = HP / (float)MaxHP;
            MaxHP += _MaxHP;
            HP = (int)((float)MaxHP *PerHp);
        }
        else
        {
            HP = MaxHP = _MaxHP;
        }
        HPProgressBar();
    }
    public void CostPlus(int Number)
    {
        Cost += Number;
        if (MaxCost < Cost)
            Cost = MaxCost;
    }

    public int Enemy()
    {
        return (Controller + 1) % 2;
    }
    public void HPProgressBar()
    {
        StartCoroutine(HPDownAnimation());
    }
    public IEnumerator HPDownAnimation()
    {
        string ProgressBarName = (Controller == 0 ? "Champion" : "Challanger") + "HP";
        Vector2 Hps = new Vector2();
        Vector2 YellowHps = new Vector2();
        YellowHps = UIProgressBar.GetData(ProgressBarName);
        Hps.x = HP;
        Hps.y = MaxHP;
        UIProgressBar.SetData(ProgressBarName, Hps);
        ProgressBarName += "Y";
        if (isYellowCanSee)
            yield break;
        isYellowCanSee = true;
        while (YellowHps.x > HP)
        {
            YellowHps.x-=0.5f;
            UIProgressBar.SetData(ProgressBarName, YellowHps);
            yield return null;
        }
        isYellowCanSee = false;
    }
}
