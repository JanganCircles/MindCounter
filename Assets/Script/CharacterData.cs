using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusSetup: MonoBehaviour {
    public static CharacterStatusSetup ins;

    const int StreetFighter = 0; //권투맨
    const int ShieldWorrier = 1; //실드맨
    const int FencingMaster = 2; //펜싱맨

    const int BatMan = 3;        //빠따맨
    const int Golliath = 4;      //헐크
    const int GauntletsMan = 5;  //바이

    const int Golem = 6;         //빅-골렘
    const int Cat = 7;           //떼껄룩
    const int Monk = 8;          //수도사

    const int Assassin = 9;      //암살자 공간상 안들어감
    const int dibidibidip= 10;   //외계인 넌그냥..

    public enum Charas : int
    {
        StreetFighter = 0,
        ShieldWorrier = 1,
        FencingMaster = 2,
        BatMan = 3,
        Golliath = 4,
        GauntletsMan = 5,
        Golem = 6,
        Cat = 7,
        Monk = 8,
        Assassin = 9,
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
            case Golliath:
                {
                    Stat.SetMaxHP(false, 2000);
                }
                break;
            case GauntletsMan:
                {
                }
                break;
            case Golem:
                {
                }
                break;
            case Cat:
                {
                }
                break;
            case Monk:
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
