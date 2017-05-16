using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIconCtrl : MonoBehaviour {

    Sprite sprite;
    public static int ImageNumber;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static void ResetIndex()
    {
        ImageNumber = 0;
    }
    public void SetImage(bool isPotion)
    {
        Item.ITEMCODE Codes;
        GameValueInfo.MenuItemIndex.RESULTTYPE type = isPotion ? GameValueInfo.MenuItemIndex.RESULTTYPE.POTION : GameValueInfo.MenuItemIndex.RESULTTYPE.EQULPMENT;
        if (GameValueInfo.MenuItemIndex.GetItemIndexToCode(out Codes, type, ImageNumber))
        {
            ItemData data = Item.GetItem(Codes);
            sprite = Resources.Load(data.ItemPath) as Sprite;
        }
    }
}
