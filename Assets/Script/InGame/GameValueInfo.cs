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
    public const int Critical = 5;
}
public class MenuItemIndex
{
    const int POTIONLENGTH = 5;
    const int ITEMLENGTH = 8;
    const int TIERLENGTH = 13;
    public enum RESULTTYPE { POTION,EQULPMENT}
    public static bool GetItemCodeToIndex(out int Result, RESULTTYPE type, Item.ITEMCODE code)
    {
        int typelength = type == RESULTTYPE.POTION ? POTIONLENGTH : ITEMLENGTH;
        Result = -1;
        int Tier = (int)code / TIERLENGTH;
        int index = (int)code % TIERLENGTH;
        switch (type)
        {
            case RESULTTYPE.POTION:
                if (index >= typelength)
                    return false;
                break;
            case RESULTTYPE.EQULPMENT:
                index -= POTIONLENGTH;
                if (index < 0)
                    return false;
                break;
        }
        Result = index + Tier * typelength;
        return true;
        //21번 2티어 완드 , 인덱스는 11번나와야함.
        //인덱스 = 6번
    }
    public static bool GetItemIndexToCode(out Item.ITEMCODE Result, RESULTTYPE type, int index)
    {
		Result = Item.ITEMCODE.armor_L;
        int typelength = type == RESULTTYPE.POTION ? POTIONLENGTH : ITEMLENGTH;
        
        if (index >= typelength * 3) return false;
        int Tier = index / typelength;
        switch (type)
        {
            case RESULTTYPE.POTION:
                Result = (Item.ITEMCODE)(index + Tier * ITEMLENGTH);
                break;
            case RESULTTYPE.EQULPMENT:
                Result = (Item.ITEMCODE)(index + (Tier + 1 ) * POTIONLENGTH);
                break;
        }
        return true;
    }

}