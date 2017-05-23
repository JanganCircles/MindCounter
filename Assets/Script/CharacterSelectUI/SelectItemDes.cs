using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SelectItemDes : MonoBehaviour {
    public static List<string>[] ItemStrs = null;
    public Text Name;
    public Text Des;
    public Text Weight;
    public Text Rarity;
    private int itemidx = -1;
    // Use this for initialization
    void Awake () {
        if (ItemStrs == null)
        {
            TextAsset Text = Resources.Load("ItemDes") as TextAsset;
            string str = Text.text;
            string[] strarr;
            strarr = str.Split('\n');
            ItemStrs = new List<string>[strarr.Length];
            for (int i = 0; i < ItemStrs.Length; i++)
            {
                ItemStrs[i] = new List<string>();
                string[] Strs = strarr[i].Split(',');
                try
                {
                    for (int j = 0; j < Strs.Length && Strs[j] != ""; j++)
                    {
                        ItemStrs[i].Add(Strs[j]);
                    }
                }
                catch (Exception e)
                {
                    int a = 0;
                }
            }
        }

    }
    public void SetItemIndex(int idx,MenuItemIndex.RESULTTYPE type)
    {
        if (itemidx == idx) return;
        Item.ITEMCODE Code;
        MenuItemIndex.GetItemIndexToCode(out Code, type, idx);
        bool NoneValue = Code == Item.ITEMCODE.NONE;
        EnableText(!NoneValue);
        if (NoneValue)
            return;
        itemidx = (int)Code;
        SetTexts();
    }
    private void EnableText(bool bvalue)
    {
        GetComponent<Image>().enabled = bvalue;
        Name.enabled = bvalue;
        Rarity.enabled = bvalue;
        Des.enabled = bvalue;
        Weight.enabled = bvalue;



    }
    void SetTexts()
    {
        Name.text = ItemStrs[itemidx][1];
        Rarity.text = ItemStrs[itemidx][2];
        Des.text = ItemStrs[itemidx][3];
        Weight.text = ItemStrs[itemidx][4] + "무게";
    }
}
