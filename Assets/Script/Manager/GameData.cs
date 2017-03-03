using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameData : MonoBehaviour {
    public bool OnePlayer;          //혼자하냐
    public bool isOnline;           //온라인이냐
    public string TempSceneName;    //바꿀씬
    public static GameData ins { get; set; }//인스턴스
    private int[] PlayerCharacter = null;//현재 캐릭터
    public CharacterData.Charas[] Characters = null;//현재 캐릭터(에디터 보여주기용)
    // Use this for initialization
    void Reset()
    {
        PlayerCharacter = null;
    }
    void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(this);
            SceneInit(TempSceneName);//씬이름별 대응
        }
        else
        {
            //처음들어오는게 아님;
            SceneInit(TempSceneName);//대응은 해야지.
            Destroy(gameObject);//게임오브젝트 삭제
        }
    }
    public void SetPlayer(int Character, int index)
    {
        PlayerCharacter[index] = Character;
        //캐릭터 데이터를 엑셀에서 가져오게 하자(http://gigong.tistory.com/4) 무리다요
    }
    public void InitPlayer()//캐릭터초기화
    {
        for (int i = 0; i < 2; i++)
        {
            Characters[i] = (CharacterData.Charas)PlayerCharacter[i];//캐릭터 번호 저장
        }
    }
    public void PlayGame(ref int Red, ref int Blue)
    {
        //???
    }
    public void SceneInit(string str)
    {
        switch (str)
        {
            case "CharacterSelect"://캐릭터셀렉씬일때
                ins.PlayerCharacter = new int[2];

                break;
            case "Main"://메인씬일떄
                {
                    if (isOnline)
                    {
                    }
                    if (ins.PlayerCharacter == null)//캐릭터가 없으면?
                    {
                        ins.PlayerCharacter = new int[2];
                        ins.PlayerCharacter[0] = 0;
                        ins.PlayerCharacter[1] = 0;
                        //임의의값 설정
                    }
                    if (ins == this && Characters.Length == 2)//첫씬이 해당씬이고, 캐릭터가 있으면
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            ins.PlayerCharacter[i] = (int)(Characters[i]);
                        }
                    }
                }
                break;
            case "ItemSelect"://아이템셀렉씬일때

                //여기다 하면 돼.
                //*참고 ins. 으로 사용해야함.
                break;
            case "SelectScene"://온라인로비씬일때
                
                break;
        }
    }
    
}
