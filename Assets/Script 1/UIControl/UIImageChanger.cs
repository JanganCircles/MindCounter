using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class UIImageChanger : MonoBehaviour{
    public bool useSpriteRender = false;
    public static Dictionary<string, UIImageChanger> UIList = null;
    public string Name;
    public Sprite[] Sprites;
    private Image img;
    private SpriteRenderer spr;
    void Awake()
    {
        if (UIList == null)
        {
            UIList = new Dictionary<string, UIImageChanger>();
        }
        UIList.Add(Name, this);
        img = GetComponent<Image>();
        spr = GetComponent<SpriteRenderer>();
    }
    public static void Release()
    {
        if (UIList != null)
        UIList.Clear();
        UIList = null;
    }
    public void ChangeImage(int Index)
    {
        if (useSpriteRender)
            spr.sprite = Sprites[Index];
        else
            img.sprite = Sprites[Index];
    }
}
