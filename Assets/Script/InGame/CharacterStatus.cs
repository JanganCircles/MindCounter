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

    public int SpecialPower;        //특수사용하는데 필요한 자원

    public const int ROCK   = 0;
    public const int SCISSOR = 1;
    public const int PAPER  = 2;
    void Reset()
    {
        Cost = 7;
        MaxCost = 15;
        SpecialPower = 500;
        Guard = false;
        HP = MaxHP = 20;
        WallDistance = 600;
    }
    public void HpDown(int Damage)
    {
        HP -= Damage;
        
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
}
