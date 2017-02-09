using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {

    public int Controller;
    public int Life;                //체력
    public int MaxHP;               //최대체력
    public int HP;                  //현재체력
    public bool Down;               //다운 여부
    public bool Disable;            //무력화 여부
    public bool Defence;            //방어상태 여부
    public bool Guard;              //가드함ㅋ
    public float WallDistance;      //벽까지의 거리

    public int[] RSPTempCount;      //RSP남은갯수
    //Rock = 압박, Scissor = 화력 , Paper = 연속
    public int AttackType;          //공격타입
    public int[] RSPMaxCount;       //RSP최대갯수

    public int SpecialPower;        //특수사용하는데 필요한 자원

    public const int ROCK   = 0;
    public const int SCISSOR = 1;
    public const int PAPER  = 2;
    void Reset()
    {
        Life = 1;
        SpecialPower = 500;
           RSPTempCount = new int[3];
        RSPMaxCount = new int[3];
        for (int i = 0; i < 3; i++)
        {
            RSPTempCount[i] = 3;
            RSPMaxCount[i] = 3;
        }
        Guard = false;
        Down = false;
        Disable = false;
        HP = MaxHP = 800;
        WallDistance = 600;
    }
    public void RSPSet(int Rock, int Scissors, int Paper)
    {
        RSPTempCount[ROCK] = RSPMaxCount[ROCK] = Rock;
        RSPTempCount[SCISSOR] = RSPMaxCount[SCISSOR] = Scissors;
        RSPTempCount[PAPER] = RSPMaxCount[PAPER] = Paper;
    }
    public void RSPPlus(int Num)
    {
        RSPTempCount[Num]++;
        if (RSPTempCount[Num] > RSPMaxCount[Num])
        {
            RSPTempCount[Num] = RSPMaxCount[Num];
        }
    }
    public void HpDown(int Damage)
    {
        HP -= Damage;
        
    }
    public int Enemy()
    {
        return (Controller + 1) % 2;
    }
    public void CheckDie()
    {
        if (HP < 0)
        {
            Life--;
            if (Life > 0)
                HP = MaxHP;
        }
    }
    public void DebuffReset()
    {
        Disable = false;
        Down = false;
        Defence = false;
    }
}
