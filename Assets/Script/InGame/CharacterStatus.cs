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
    

    public const int ROCK   = 0;
    public const int SCISSOR = 1;
    public const int PAPER  = 2;
    void Reset()
    {
        Cost = (MaxCost = CharacterData.Mp) / 2;
        Guard = false;
        HP = MaxHP = CharacterData.Hp;
        WallDistance = 600;
    }
    public void HpDown_Debuff(int Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            HP = 1;
        }
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
