using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManuSceneMove : MonoBehaviour {
    public SelectMenu Menu;
    public Vector2 PrevV;
    public Vector2 v;
	// Use this for initialization
	void Start () {
        v = new Vector2( 0,0);

    }
	
	// Update is called once per frame
	void Update () {
        if (Menu.isSelect(out v))
        {
            GotoScene(Menu.IconTr[(int)v.y].GetComponentInChildren<Text>().name);
        }
        else
        {
            if (v != PrevV)
            {
                Text PrevText = Menu.IconTr[(int)PrevV.y].GetChild(0).transform.GetChild(0).GetComponent<Text>();
                Text TempText = Menu.IconTr[(int)v.y].GetChild(0).transform.GetChild(0).GetComponent<Text>();
                PrevText.gameObject.SetActive(false);
                TempText.gameObject.SetActive(true);
                PrevV = v;
                //  PrevV //정상화
                //  v // 미백
            }
        }


    }

    public static void GotoScene(string str)
    {
        SceneManager.LoadScene(str);
    }
}
