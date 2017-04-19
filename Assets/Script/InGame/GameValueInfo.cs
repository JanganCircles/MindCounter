using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageData {
    public const int Weak    = 40;
    public const int Middle  = 80;
    public const int Strong  = 150;
}
public class CostData
{
    //감소
    public const int Weak = 20;
    public const int Middle = 40;
    public const int Strong = 60;
    public const int Guard = 10;
    //회복
    public const int EnergyRecovery = 50;

}
public class CharacterData
{
    public const int Hp = 1800;
    public const int Mp = 200;
}
public class KnockBackData
{
    public const int Guard  = 100;
    //임시로 데미지의 5배로 설정.
    public const int Weak   = DamageData.Weak * 2;
    public const int Middle = DamageData.Middle * 2;
    public const int Strong = DamageData.Strong * 2;
}
public class ProbabilityData
{
    public const int Critical = 100;
}