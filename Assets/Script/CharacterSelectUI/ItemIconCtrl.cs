using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIconCtrl : MonoBehaviour {

    public delegate void ChangeSwitch();
    public static ChangeSwitch[] switchs = null;
    Sprite EqulpMentSprite;
    Sprite PotionSprite;
    private int ImageNumber;
    public bool isInValue;
    bool isEqulp;
    public bool DebugOnlyRun;
    // Use this for initialization
    void Awake() {
        if (switchs == null)
        {
            switchs = new ChangeSwitch[2];
        }
        switchs[transform.parent.name == "Blue" ? 0 : 1] += SwitchImage;
        isEqulp = true;
        int result = -1;
        string Number = name.Substring(9, name.Length - 9);
        if (int.TryParse(Number, out result))
            ImageNumber = result;
        SetImage();
        GetComponent<Image>().sprite = EqulpMentSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugOnlyRun == true)
        {
            switchs[0]();
            DebugOnlyRun = false;
        }
    }
    void SwitchImage()
    {
        isEqulp = !isEqulp;
        if(isEqulp)
            GetComponent<Image>().sprite = EqulpMentSprite;
        else
            GetComponent<Image>().sprite = PotionSprite;
        isInValue = GetComponent<Image>().sprite == null;
    }
    public void SetImage()
    {
        Item.ITEMCODE Codes;
        for (int i = 0; i < 2; i++)
        {
            if (MenuItemIndex.GetItemIndexToCode(out Codes, (MenuItemIndex.RESULTTYPE)i, ImageNumber))
            {
                Item.ItemData data = Item.GetItem(Codes);
                if (i == 0) PotionSprite = data.GetSprite();
                else EqulpMentSprite= data.GetSprite();
            }
        }
    }
}
