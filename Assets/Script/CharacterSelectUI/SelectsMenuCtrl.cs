﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectsMenuCtrl : MonoBehaviour {


    private Text[] CharacterNames;
    public string[] DescriptNames;

    public Sprite[] SpriteImg;              //캐릭터 이미지들
    public Image[] UIImage;              //UI사용되는 이미지
    SelectMenu[] TempMenu = new SelectMenu[2];
    public SelectMenu[] CharacterSelecter;
    public SelectMenu[] ItemSelecter;

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
            UITextUpdate(i,v);
            yield return null;
        }
        PlayerStasis[i]++;
        TempMenu[i].isRun = false;
        AllOK[i] = true;
        StartCoroutine("StasisChecker", i);
    }
    IEnumerator ItemSelect(int i)
    {
        bool[] ItemsOK = { false, false}; 
        SelectMenu Sm = TempMenu[i] = ItemSelecter[i];
        TempMenu[i].isRun = true; 

        Vector2 v; 

        while (true)
        {
            if (Sm.isSelect(out v))
            {
                if (!ItemsOK[0])
                {
                    ItemsOK[0] = true;
                    Sm.Cancel();
                }
                else
                {
                    AllOK[i] = true;
                    CheckingCharacterIndex(i);
                    ItemsOK[1] = true;
                }
            }
            if (Input.GetKeyDown(Sm.KeysData[Sm.KeysData.Length - 1]))
            {
                AllOK[i] = false;
                bool isCancel = false;
                for (int j = 1; j >= 0; j--)
                {
                    if (ItemsOK[j])
                    {
                        ItemsOK[j] = !ItemsOK[j];
                        isCancel = true;
                        break;
                    }
                }
                if (!isCancel)
                {

                    PlayerStasis[i]--;
                    Sm.isRun = false;
                    CharacterSelecter[i].Cancel();
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
            StopAllCoroutines();
            SceneManager.LoadScene("Main");//씬변경
        }
    }
    void UITextUpdate(int i,Vector2 v)
    {
        string[] NameStr = { "Blue", "Red" };

        float Index = v.x + (v.y * TempMenu[i].XLength);
        UITextSet.UIList[NameStr[i] + "Name"] = DescriptNames[(int)Index];
    }
}
