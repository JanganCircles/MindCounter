﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSetting : MonoBehaviour {
    public Text TargetText;
    int Number;
    public int TenPos;
    public bool isZeroDrow;
    static Sprite[] Spr = new Sprite[10];
    Image img;
    // Use this for initialization
	void Start () {
        img = GetComponent<Image>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/charScene_render");
        for (int i = 1; i < 10; i++)
        {
            Spr[i] = sprites[i-1];
        }
        Spr[0] = sprites[9];
	}

    // Update is called once per frame
    void Update()
    {
        if (int.TryParse(TargetText.text, out Number))
        {
            if (!isZeroDrow && Number < TenPos)
            {
                img.enabled = false;
            }
            else
            {

                img.enabled = true;
                int Value = (Number % (TenPos * 10));
                if (Value != 0)
                {
                    Value /= TenPos;
                }
                img.sprite = Spr[Value];
            }
        }
    }
}
