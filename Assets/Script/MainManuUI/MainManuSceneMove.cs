using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManuSceneMove : MonoBehaviour
{
    public SelectMenu Menu;
    public Vector2 PrevV;
    public Vector2 v;
    public IllustrationChanger changer;
    // Use this for initialization
    void Start()
    {
        v = new Vector2(0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (Menu)
        {

            if (Menu.isSelect(out v))
            {
                if (v.y > 0 && v.y < 4)
                {
                    Menu.Cancel();
                }
                else if (v.y == 4)
                {
                    Application.Quit();
                }
                else
                    GotoScene(Menu.IconTr[(int)v.y].GetComponentInChildren<Image>().name);
            }
            else
            {
                if (v != PrevV)
                {
                    changer.TriggerOn();
                    Image PrevText = Menu.IconTr[(int)PrevV.y].GetChild(0).transform.GetChild(0).GetComponent<Image>();
                    Image TempText = Menu.IconTr[(int)v.y].GetChild(0).transform.GetChild(0).GetComponent<Image>();
                    PrevText.gameObject.SetActive(false);
                    TempText.gameObject.SetActive(true);
                    Menu.IconTr[(int)PrevV.y].GetChild(0).GetComponent<Image>().enabled = true;
                    Menu.IconTr[(int)v.y].GetChild(0).GetComponent<Image>().enabled = false;
                    PrevV = v;
                    //  PrevV //정상화
                    //  v // 미백
                }
            }
        }

    }

    public static void GotoScene(string str)
    {
        SceneManager.LoadScene(str);
    }
}
