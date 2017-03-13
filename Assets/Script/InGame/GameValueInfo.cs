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
    public const int Hp = 1000;
    public const int Mp = 200;
}
public class KnockBackData
{
    public const int Guard  = 100;
    //임시로 데미지의 5배로 설정.
    public const int Weak   = 100;
    public const int Middle = 250;
    public const int Strong = 400;
}
public class ProbabilityData
{
    public const int Critical = 5;
}