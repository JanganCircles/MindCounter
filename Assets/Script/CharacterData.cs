using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour {
    public static CharacterData ins;

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
                    Stat.RSPSet(5, 2, 3);
                }
                break;
            case ShieldWorrier:
                {
                    Stat.RSPSet(4, 3, 3);
                }
                break;
            case FencingMaster:
                {
                    Stat.RSPSet(4, 2, 4);
                }
                break;
            case BatMan:
                {
                    Stat.RSPSet(6, 2, 2);
                }
                break;
            case Golliath:
                {
                    Stat.RSPSet(3, 5, 2);
                }
                break;
            case GauntletsMan:
                {
                    Stat.RSPSet(2, 5, 3);
                }
                break;
            case Golem:
                {
                    Stat.RSPSet(3, 4, 3);
                }
                break;
            case Cat:
                {
                    Stat.RSPSet(2, 2, 6);
                    Stat.HP = Stat.MaxHP = 250;
                }
                break;
            case Monk:
                {
                    Stat.RSPSet(5, 3, 2);
                }
                break;
            case Assassin:
                {
                    Stat.RSPSet(2, 4, 4);
                }
                break;
            case dibidibidip:
                {
                    //보류
                    Stat.RSPSet(4, 3, 3);
                }
                break;
        }
    }
}
