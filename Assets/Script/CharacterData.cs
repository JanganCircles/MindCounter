using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusSetup: MonoBehaviour {
    public static CharacterStatusSetup ins;

    const int StreetFighter = 0; //권투맨
    const int ShieldWorrier = 1; //실드맨
    const int FencingMaster = 2; //펜싱맨
    const int BatMan = 3;        //빠따맨
    const int Golem = 4;         //골렘
    const int GauntletsMan = 5;  //바이
    const int Cat = 6;           //고양이
    const int Random = 7;        //랜덤
    const int Assassin = 8;      //암살자
        
    const int dibidibidip= 10;   //외계인 넌그냥..

    public enum Charas : int
    {
        StreetFighter = 0,
        ShieldWorrier = 1,
        FencingMaster = 2,
        BatMan = 3,
        Golem = 4,
        GauntletsMan = 5,
        Cat = 6,
        Random = 7,
        Assassin = 8,
        dibidibidip = 10
    }
    public static void SetupStat(int Index,int CharacterNum)
    {
        CharacterStatus Stat = gameManager.ins.UserStatus[Index];
        switch (CharacterNum)
        {
            case StreetFighter:
                {
                }
                break;
            case ShieldWorrier:
                {
                }
                break;
            case FencingMaster:
                {
                }
                break;
            case BatMan:
                {
                }
                break;
            case GauntletsMan:
                {
                }
                break;
            case Golem:
                {
                    Stat.SetMaxHP(false, 1500);
                }
                break;
            case Cat:
                {
                }
                break;
            case Assassin:
                {
                }
                break;
            case dibidibidip:
                {
                    //보류
                }
                break;
        }
    }
}
