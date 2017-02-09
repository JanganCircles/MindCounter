using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {

    public int Controller;
    public int Life;
    public int MaxHP;
    public int HP;
    public bool Down;
    public bool Disable;
    public bool Defence;
    public bool Guard;
    public float WallDistance;      //벽까지의 거리

    public int[] RSPTempCount;      //RSP남은갯수
    public int AttackType;          //공격타입
    public int[] RSPMaxCount;       //RSP최대갯수

    public int SpecialPower;        //특수사용하는데 필요한 자원

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
    private void CheckDie()
    {
        if (HP <= 0)
        {

        }
    }
    public void DebuffReset()
    {
        Disable = false;
        Down = false;
        Defence = false;
    }
}
