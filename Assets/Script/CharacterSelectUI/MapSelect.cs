using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelect : MonoBehaviour {

    public enum MAPSTASIS
    {
        RANDOM,APLANIT, BPLANIT,CPLANIT,DPLANIT
    }

    public SelectMenu sMenu;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 v = new Vector2();
        if (sMenu.isSelect(out v))
        {
            GameData.ins.MapData = (MAPSTASIS)v.x;
            MainManuSceneMove.GotoScene("Main");
        }
	}
}
