using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameData : MonoBehaviour {
    public bool OnePlayer;          //혼자하냐
    public bool isOnline;           //온라인이냐
    public string TempSceneName;    //바꿀씬
    public GameObject BackGroundEffect;

    public static GameData ins { get; set; }//인스턴스
    private int[] PlayerCharacter = null;//현재 캐릭터
    public Item.ITEMCODE[] PotionCode = null;
    public Item.ITEMCODE[] EquipmentCode = null;
    public MapSelect.MAPSTASIS MapData;
    public CharacterStatusSetup.Charas[] Characters = null;//현재 캐릭터(에디터 보여주기용)
    // Use this for initialization
    void Reset()
    {
        PlayerCharacter = null;
    }
    void OnEnable()
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
    }
    public void InitPlayer()//캐릭터초기화
    {
        for (int i = 0; i < 2; i++)
        {
            Characters[i] = (CharacterStatusSetup.Charas)PlayerCharacter[i];//캐릭터 번호 저장
        }
    }
    public void PlayGame(ref int Red, ref int Blue)
    {
        //???
    }
    public void AllCameraClose()
    {
        Debug.Log(ins.BackGroundEffect);
        Debug.Log(Camera.allCameras.Length);
        if(Camera.main)
            Camera.main.gameObject.SetActive(false);
        ins.BackGroundEffect.transform.FindChild("Camera").gameObject.SetActive(false);


    }
    public void SceneInit(string str)
    {
        switch (str)
        {
            case "MainManu":
                {
                    if (BackGroundEffect == null)
                    {
                    }
                    else
                    {
                        ins.BackGroundEffect.SetActive(true);
                        Debug.Log(ins.BackGroundEffect);
                        AllCameraClose();
                        ins.BackGroundEffect.GetComponent<MenuSceneMove>().cam.gameObject.SetActive(true);
                        Debug.Log(BackGroundEffect);
                    }
                }
                break;
            case "CharacterSelect"://캐릭터셀렉씬일때
                if (ins.BackGroundEffect == null)
                {

                    //BackGroundEffect = GameObject.Find("SpaceEffect");
                }
                else
                {
                    ins.BackGroundEffect.SetActive(true);
                    AllCameraClose();
                    ins.BackGroundEffect.GetComponent<MenuSceneMove>().cam.gameObject.SetActive(true);
                }
                ins.PotionCode = new Item.ITEMCODE[2];
                ins.EquipmentCode = new Item.ITEMCODE[2];
                ins.PlayerCharacter = new int[2];

                break;
            case "Main"://메인씬일떄
                {
                    if (ins.BackGroundEffect)
                    {
                            ins.BackGroundEffect.SetActive(false);
                            GameObject maincamera = Camera.main.gameObject;
                            AllCameraClose();
                            maincamera.SetActive(true);
                    }
                    GameObject gm = null;
                    gm = GameObject.Find("NetWorkLobby");
                    if (gm != null)
                    {
                        Debug.Log("온라인임");
                        ins.isOnline = true;
                        ins.OnePlayer = true;
                    }
                    else
                    { 
                        Debug.Log("오프라인임");
                        ins.isOnline = false;
                        ins.OnePlayer = false;
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
