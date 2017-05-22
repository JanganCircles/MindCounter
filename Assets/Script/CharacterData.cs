using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusSetup {
    public static CharacterStatusSetup ins;

    const int StreetFighter = 0; //권투맨
    const int Cat = 1;           //고양이
    const int GauntletsMan = 2;  //바이
    const int Assassin = 3;      //암살자
    const int Golem = 4;         //골렘
    const int ShieldWorrier = 5; //실드맨
    const int FencingMaster = 6; //펜싱맨
    const int BatMan = 7;        //빠따맨
    const int Random = 8;        //랜덤
        
    const int dibidibidip= 10;   //외계인 넌그냥..

    public enum Charas : int
    {
        StreetFighter = 0,
        Cat = 1,           
          GauntletsMan = 2,  
          Assassin = 3,      
          Golem = 4,         
          ShieldWorrier = 5, 
          FencingMaster = 6, 
          BatMan = 7,
        Random = 8,
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
