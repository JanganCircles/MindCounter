using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Iconset : MonoBehaviour {
    public bool isPotion;
    public int Order;
    Image img;
	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {
        SetImage();
    }
    public void SetImage()
    {
        Item.ITEMCODE code = isPotion ? GameData.ins.PotionCode[Order] : GameData.ins.EquipmentCode[Order];
        if (code == Item.ITEMCODE.NONE)
        {
            img.color = Color.clear;
            return;

        }
        else {
            img.color = Color.white;
        }
        Item.ItemData data = Item.GetItem(code);
        img.sprite = data.GetSprite();

    }
}
