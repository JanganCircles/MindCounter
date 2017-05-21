using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectsMenuCtrl : MonoBehaviour {


    int TimeNumber = 99;
    public Sprite[] SpriteImg;              //캐릭터 이미지들
    public Image[] UIImage;              //UI사용되는 이미지
    SelectMenu[] TempMenu = new SelectMenu[2];//현재 메뉴
    public SelectMenu[] CharacterSelecter; // 캐릭셀렉
    public SelectMenu[] ItemSelecter;      // 아이템 셀렉
    public int[] Duration;                  //무게
    public float UpMenuY;
    public float DownMenuY;
    public const int MAXDURATION = 10;      //최종무게
    private Item.ITEMCODE[] ItemCode = {0,0,0,0};        // 선택한 아이템 인덱스
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
        StartCoroutine(Timer());
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ItemIconCtrl.ResetThis();
            MainManuSceneMove.GotoScene("MainManu");
        }
    }
    IEnumerator Timer()
    {
        while (TimeNumber > 0)
        {

            UITextSet.UIList["Timer"] = TimeNumber.ToString();

            yield return new WaitForSeconds(1f);
            TimeNumber--;
        }
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
        int TempCharacter = 0;//선택한 캐릭터의 번호  
        Vector2 v;
        while (!TempMenu[i].isSelect(out v))
        {
            TempCharacter= (int)(TempMenu[i].Cursor.x + 3 * TempMenu[i].Cursor.y);//선택한 캐릭터의 번호  

            GameData.ins.SetPlayer(TempCharacter, i);//게임데이터에 저장

            TempCharacter = (int)(v.x + TempMenu[i].XLength * v.y);
            if(TempCharacter == 0)
                UIImage[i].sprite = SpriteImg[Random.Range(1,5)];
            else
                UIImage[i].sprite = SpriteImg[TempCharacter];
            yield return null;
        }
        EffectManager.ins.EffectRun(TempMenu[i].CursorTr.GetComponent<RectTransform>().position, Vector3.one, "LeftBar",0.63f, true);
        PlayerStasis[i]++;
        TempMenu[i].isRun = false;
        CharacterCode[i] = TempCharacter;
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
                if(!ItemsOK[1])
                    EffectManager.ins.EffectRun(TempMenu[i].CursorTr.GetComponent<RectTransform>().position, Vector3.one, "LeftBar", 0.63f, true);

                if (!ItemsOK[0])
                {
                    //1번아이템
                    int index = (int)v.x + (int)v.y * Sm.XLength;
                    MenuItemIndex.GetItemIndexToCode(out ItemCode[i], MenuItemIndex.RESULTTYPE.EQULPMENT, index);
                    if (ItemCode[i ] != Item.ITEMCODE.NONE)
                    {
                        Item.ItemData data = Item.GetItem(ItemCode[i]);
                        if (Duration[i] + data.weight <= MAXDURATION)
                        {
                            GameData.ins.EquipmentCode[i] = ItemCode[i];
                            Duration[i] += data.weight;
                            ItemsOK[0] = true;
                            Sm.Cancel();
                            yield return StartCoroutine("MoveMenu", new object[] { Sm, UpMenuY });
                            ItemIconCtrl.switchs[i]();
                            //이미지교체
                            yield return StartCoroutine("MoveMenu", new object[] { Sm, DownMenuY });
                        }
                        else
                        {
                            Sm.Cancel();
                        }
                        }
                    else
                    {
                        Sm.Cancel();
                    }
                }
                else if(!ItemsOK[1])
                {
                    //2번아이템
                    int index = (int)v.x + (int)v.y * Sm.XLength;
                    MenuItemIndex.GetItemIndexToCode(out ItemCode[i + 2], MenuItemIndex.RESULTTYPE.POTION, index);

                    if (ItemCode[i + 2] != Item.ITEMCODE.NONE)
                    {
                        Item.ItemData data = Item.GetItem(ItemCode[i + 2]);
                        if (Duration[i] + data.weight <= MAXDURATION)
                        {
                            GameData.ins.PotionCode[i] = ItemCode[i + 2];
                            Duration[i] += data.weight;
                            AllOK[i] = true;
                            CheckingCharacterIndex(i);
                            ItemsOK[1] = true;

                            yield return StartCoroutine("MoveMenu", new object[] { Sm, UpMenuY });
                            ItemIconCtrl.switchs[i]();
                        }
                        else
                        {
                            Sm.Cancel();
                        }
                    }
                    else
                    {
                        Sm.Cancel();
                    }
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
                        Item.ItemData data = Item.GetItem(ItemCode[i + 2 * j]);
                        Duration[i] -= data.weight;
                        if (j != 1)
                            yield return StartCoroutine("MoveMenu", new object[] { Sm, UpMenuY });
                        ItemIconCtrl.switchs[i]();
                        yield return StartCoroutine("MoveMenu", new object[] { Sm, DownMenuY });
                        if (j == 1)
                        {
                            GameData.ins.PotionCode[i] = Item.ITEMCODE.NONE;
                        }
                        else
                            GameData.ins.EquipmentCode[i] = Item.ITEMCODE.NONE;
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
        for (int i = 0; i < 2; i++)
        {
            GameData.ins.EquipmentCode[i] = ItemCode[i];
            GameData.ins.PotionCode[i] = ItemCode[i + 2];
        }
        ItemIconCtrl.ResetThis();
        SceneManager.LoadScene("Main");//씬변경

    }
}
