using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class UIImageChanger : MonoBehaviour
{
    public static Dictionary<string, UIImageChanger> UIList = null;//저장용 리스트
    public bool useSpriteRender = false;//true = 캔버스x / false = 캔버스o
    public string Name;         //이름
    public Sprite[] Sprites;    //스프라이트(멀티플)
    private Image img;          //현재 이미지
    private SpriteRenderer spr; //스프라이트 렌더러
    void Awake()
    {
        if (UIList == null)
        {
            UIList = new Dictionary<string, UIImageChanger>();
        }
        UIList.Add(Name, this);//저장용 스태틱리스트에 자기자신을 넣는다
        img = GetComponent<Image>();//초기화
        spr = GetComponent<SpriteRenderer>();
    }
    public static void Release()
    {
        if (UIList != null)
        UIList.Clear();//제-거
        UIList = null;
    }
    public void ChangeImage(int Index)//교체작업
    {
        if (useSpriteRender)
            spr.sprite = Sprites[Index];
        else
            img.sprite = Sprites[Index];
    }
}
