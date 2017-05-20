using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectsMenuCtrl : MonoBehaviour {



    public Sprite[] SpriteImg;              //캐릭터 이미지들
    public Image[] UIImage;              //UI사용되는 이미지
    SelectMenu[] TempMenu = new SelectMenu[2];
    public SelectMenu[] CharacterSelecter;
    public SelectMenu[] ItemSelecter;

    public float UpMenuY;
    public float DownMenuY;

    private int[] ItemCode = { -1, -1, -1, -1 };        // 선택한 아이템 인덱스
    private int[] CharacterCode = { -1, -1 };           // 선택한 캐릭터 인덱스
    public int[] SelectItem = { -1, -1, -1, -1 };

    private bool[] AllOK = { false,false};

    //0 = 1피, 1 = 2피
    public StasisLevel[] PlayerStasis = { StasisLevel.CHARACTERSELECT, StasisLevel.CHARACTERSELECT };


    public enum StasisLevel
    {
        CHARACTERSELECT,
        ITEMSELECT,
        ALLOK,

    }
    // Use this for initialization

    private void Reset()
    {
        

    }
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            ItemIconCtrl.switchs[i]();
            TempMenu[i] = CharacterSelecter[i];
            StartCoroutine("CharacterSelect", i);
        }
    }
    void CursorSetting(ref Vector2 vec,int index)
    {
        switch (index)
        {
            case 0: vec.y--; break;
            case 2: vec.y++; break;
            case 1:vec.x--; break;
            case 3: vec.x++; break;
        }
    }

    void Update()
    {
    }
    IEnumerator MoveMenu(object[] obj)
    {
        SelectMenu menu = obj[0] as SelectMenu;
        float yValue = (float)obj[1];
        RectTransform tr = menu.GetComponent<RectTransform>();
        Vector2 v2 = tr.anchoredPosition;
        v2.y = yValue;
        for (int i = 0; i < 100; i++)
        {
            tr.anchoredPosition = Vector2.Lerp(tr.anchoredPosition,v2,0.05f); 
            yield return null;
        }
    }
    IEnumerator CharacterSelect(int i)
    {
        TempMenu[i] = CharacterSelecter[i];
        TempMenu[i].isRun = true;
        int TempCharacter;//선택한 캐릭터의 번호  
        Vector2 v;
        while (!TempMenu[i].isSelect(out v))
        {
            TempCharacter = (int)(v.x + TempMenu[i].XLength * v.y);
            UIImage[i].sprite = SpriteImg[TempCharacter];
            yield return null;
        }
        PlayerStasis[i]++;
        TempMenu[i].isRun = false;
        StartCoroutine("StasisChecker", i);
    }
    IEnumerator ItemSelect(int i)
    {
        ItemIconCtrl.switchs[i]();
        bool[] ItemsOK = { false, false}; 
        SelectMenu Sm = TempMenu[i] = ItemSelecter[i];
        TempMenu[i].isRun = true; 

        Vector2 v;

        yield return StartCoroutine("MoveMenu", new object[] { Sm,  DownMenuY });
        while (true)
        {
            if (Sm.isSelect(out v))
            {
                if (false)//예외적 아이템 클릭.
                {
                    Sm.Cancel();
                    continue;
                }
                if (!ItemsOK[0])
                {
                    //1번아이템
               //     GameData.ins.PotionCode[i] = Item.ITEMCODE();
                    ItemsOK[0] = true;
                    Sm.Cancel();
                    yield return StartCoroutine("MoveMenu", new object[] { Sm, UpMenuY });
                    ItemIconCtrl.switchs[i]();
                    //이미지교체
                    yield return StartCoroutine("MoveMenu", new object[] { Sm, DownMenuY });
                }
                else if(!ItemsOK[1])
                {
                    //2번아이템
                    AllOK[i] = true;
                    CheckingCharacterIndex(i);
                    ItemsOK[1] = true;
                    
                    yield return StartCoroutine("MoveMenu", new object[] { Sm, UpMenuY });
                    ItemIconCtrl.switchs[i]();

                }
            }
            if (Input.GetKeyDown(Sm.KeysData[Sm.KeysData.Length - 1]))
                //exit눌렀을때
            {
                AllOK[i] = false;
                bool isCancel = false;
                for (int j = 1; j >= 0; j--)
                {
                    if (ItemsOK[j])
                    {
                        ItemsOK[j] = !ItemsOK[j];
                        isCancel = true;
                        if(j != 1)
                        yield return StartCoroutine("MoveMenu", new object[] { Sm, UpMenuY });
                        ItemIconCtrl.switchs[i]();
                        yield return StartCoroutine("MoveMenu", new object[] { Sm, DownMenuY });

                        break;
                    }
                }
                if (!isCancel)
                {

                    PlayerStasis[i]--;
                    Sm.isRun = false;
                    CharacterSelecter[i].Cancel();
                    yield return StartCoroutine("MoveMenu", new object[] { Sm, UpMenuY });

                    ItemIconCtrl.switchs[i]();
                    StartCoroutine("StasisChecker", i);
                    yield break;
                }

            }

                yield return null;
        }
    }
    IEnumerator StasisChecker(int i)
    {
        while(true)
        {
            switch (PlayerStasis[i])
            {
                case StasisLevel.CHARACTERSELECT:
                    {
                        Debug.Log("asdsa");
                        StartCoroutine("CharacterSelect",i);
                        yield break;
                    }
                case StasisLevel.ITEMSELECT:
                    {
                        StartCoroutine("ItemSelect", i);
                        yield break;
                    }
                case StasisLevel.ALLOK:
                    break;
                default:
                    break;
            }
        }
    }
    void CheckingCharacterIndex(int index)//선택하면 들어옴.
    {
        int TempCharacter = (int)(TempMenu[index].Cursor.x + 3 * TempMenu[index].Cursor.y);//선택한 캐릭터의 번호  

        GameData.ins.SetPlayer(TempCharacter, index);//게임데이터에 저장

        if (AllOK[1] && AllOK[0])//둘다 선택됬으면
        {
            StartCoroutine(MoveScene());
        }
    }
    IEnumerator MoveScene()
    {
        for (int i = 0; i < 2; i++) TempMenu[i].StopAllCoroutines();
        StopCoroutine("ItemSelect");
        yield return new WaitForSeconds(1f);
        //StopAllCoroutines();
        ItemIconCtrl.ResetThis();
        SceneManager.LoadScene("Main");//씬변경

    }
}
