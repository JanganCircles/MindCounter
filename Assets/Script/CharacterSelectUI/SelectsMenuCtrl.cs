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
    public SelectItemDes[] ItemDes;        // 아이템 설명
    public int[] Duration;                  //무게
    public float UpMenuY;
    public float DownMenuY;
    public const int MAXDURATION = 10;      //최종무게
    private Item.ITEMCODE[] ItemCode = {0,0,0,0};        // 선택한 아이템 인덱스
    private int[] CharacterCode = { -1, -1 };           // 선택한 캐릭터 인덱스
    public int[] SelectItem = { -1, -1, -1, -1 };
    public string[] BigSelectEffect;
    public string[] SmallSelectEffect;
    private bool[] AllOK = { false,false};

    //0 = 1피, 1 = 2피
    private StasisLevel[] PlayerStasis = { StasisLevel.CHARACTERSELECT, StasisLevel.CHARACTERSELECT };


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

            UITextSet.UIList["TimerNumber"] = TimeNumber.ToString();

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
        float f = Time.deltaTime * 3;
        int OneFrame = (int)(1f / f);
        for (int i = 0; i < OneFrame; i++)
        {
            tr.anchoredPosition = Vector2.Lerp(tr.anchoredPosition,v2, f*2.5f); 
            yield return null;
        }
    }
    IEnumerator CharacterSelect(int i)
    {
        string str = i == 0 ? "Blue" : "Red";
        string strDes = str + "Des";
        string strNameTag = str + "NameTag";
        TempMenu[i] = CharacterSelecter[i];
        TempMenu[i].isRun = true;
        int TempCharacter = 0;//선택한 캐릭터의 번호  
        Vector2 v;
        while (!TempMenu[i].isSelect(out v))
        {
            TempCharacter= (int)(TempMenu[i].Cursor.x + 3 * TempMenu[i].Cursor.y);//선택한 캐릭터의 번호  

            GameData.ins.SetPlayer(TempCharacter, i);//게임데이터에 저장

            TempCharacter = (int)(v.x + TempMenu[i].XLength * v.y);
            if (TempCharacter == 0)
            {
                UIImageChanger.UIList[strNameTag].ChangeImage(Random.Range(0, 4));
                UIImage[i].sprite = SpriteImg[Random.Range(1, 5)];
            }
            else
            {
                UIImageChanger.UIList[strNameTag].ChangeImage((int)v.x - 1);
                UIImage[i].sprite = SpriteImg[TempCharacter];
            }
            UITextSet.UIList[strDes] = CharacterDestext.CharacterDes(TempCharacter);
            yield return null;
        }
        if (TempCharacter == 0)
        {
            TempCharacter = Random.Range(1, 5);
        }
        UIImage[i].sprite = SpriteImg[TempCharacter];
        GameData.ins.SetPlayer(TempCharacter, i);//게임데이터에 저장
        UIImageChanger.UIList[strNameTag].ChangeImage(TempCharacter - 1);
        UITextSet.UIList[strDes] = CharacterDestext.CharacterDes(TempCharacter);
        EffectManager.ins.EffectRun(TempMenu[i].CursorTr.GetComponent<RectTransform>().position, Vector3.one, BigSelectEffect[i],0.63f, true);
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
        ItemDes[i].gameObject.SetActive(true);
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
                    EffectManager.ins.EffectRun(TempMenu[i].CursorTr.GetComponent<RectTransform>().position, Vector3.one, SmallSelectEffect[i], 0.63f, true);

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
                            ItemDes[i].gameObject.SetActive(false);
                            yield return StartCoroutine("MoveMenu", new object[] { Sm, UpMenuY });
                            ItemIconCtrl.switchs[i]();
                            //이미지교체
                            yield return StartCoroutine("MoveMenu", new object[] { Sm, DownMenuY });
                            ItemDes[i].gameObject.SetActive(true);
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

                            ItemDes[i].gameObject.SetActive(false);
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
            ItemDes[i].SetItemIndex((int)(v.x + v.y * TempMenu[i].XLength), ItemsOK[0] ? MenuItemIndex.RESULTTYPE.POTION : MenuItemIndex.RESULTTYPE.EQULPMENT);
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
                        ItemDes[i].gameObject.SetActive(false);
                        if (j != 1)
                            yield return StartCoroutine("MoveMenu", new object[] { Sm, UpMenuY });
                        ItemIconCtrl.switchs[i]();
                        yield return StartCoroutine("MoveMenu", new object[] { Sm, DownMenuY });
                        ItemDes[i].gameObject.SetActive(true);
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
                    ItemDes[i].gameObject.SetActive(false);
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
class CharacterDestext
{
    public const string Cat = "카렌의 강공격은 적을 출혈상태에 빠지게 한다.\n출혈상태는 3턴동안 30의 데미지를 받는다.";
    public const string Assassin = "카나토는 상대가 강공격 시 약공격으로 카운터 칠때 빈틈을 노려 약의 데미지가 아니라 강의 데미지가 들어간다.";
    public const string Herk = "헤릭의 몸은 마력이 흐르고 있어서 모든공격이 공격+공격*(현재마나/전체마나)가 된다.";
    public const string Guntlet = "제인은 가드 성공시 다음턴에 가하는 공격에 강공격데미지가 합쳐진다\n(만약 가드 성공 후 회복이나 가드를 한번 더 한다면 이 기회는 박탈된다.)";
    public static string CharacterDes(int index)
    {
        switch (index)
        {
            case 1: return Cat;
            case 2: return Guntlet;
            case 3: return Assassin;
            case 4: return Herk;
        }
        return "";
    }
}