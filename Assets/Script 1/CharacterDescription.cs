using UnityEngine;
using System.Collections;

public class CharacterDescription {
    public string[] CharacterStr;
    public string[] CharacterName;
    public CharacterDescription()
    {
        CharacterName = new string[9];
        CharacterStr = new string[9];
        for (int i = 0; i < 9; i++)
        {
            switch (i)
            {
                case 0:
                    CharacterName[i] = "제임스";
                    CharacterStr[i] = "국적 : 미국\n크기 : 소형\n타입 : 방어\n특징 : 스텟 밸런스";
                    break;
                case 1:
                    CharacterName[i] = "엘로이";
                    CharacterStr[i] = "국적 : 호주\n크기 : 소형\n타입 : 스피드\n특징 : 고속이동";
                    break;
                case 2:
                    CharacterName[i] = "솜브라";
                    CharacterStr[i] = "국적 : 멕시코\n크기 : 소형\n타입 : 테크니컬\n특징 : 고속연사";
                    break;
                case 3:
                    CharacterName[i] = "쿠에쿠";
                    CharacterStr[i] = "국적 : 이집트\n크기 : 중형\n타입 : 방어\n특징 : 범위공격";
                    break;
                case 4:
                    CharacterName[i] = "박요원";
                    CharacterStr[i] = "국적 : 한국\n크기 : 중형\n타입 : 스피드\n특징 : 높은 아이템 효율";
                    break;
                case 5:
                    CharacterName[i] = "Beta-Go";
                    CharacterStr[i] = "국적 : 구글\n크기 : 중형\n타입 : 테크니컬\n특징 : 빠른 미니언 레벨업";
                    break;
                case 6:
                    CharacterName[i] = "루카스";
                    CharacterStr[i] = "국적 : 독일\n크기 : 대형\n타입 : 방어\n특징 : 체력";
                    break;
                case 7:
                    CharacterName[i] = "페이 신";
                    CharacterStr[i] = "국적 : 중국\n크기 : 대형\n타입 : 스피드\n특징 : 강력한 미니언";
                    break;
                case 8:
                    CharacterName[i] = "다니엘";
                    CharacterStr[i] = "국적 : 영국\n크기 : 대형\n타입 : 테크니컬\n특징 : 강력한 한방";
                    break;
            }
        }
    }



}
