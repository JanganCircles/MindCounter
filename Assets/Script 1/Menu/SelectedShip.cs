using UnityEngine;
using System.Collections;

public class SelectedShip : MonoBehaviour
{
    public GameObject[] RedShip;
    public GameObject[] BlueShip;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 2; i++)
        {
            RedShip[i].SetActive(false);
            BlueShip[i].SetActive(false);
        }

	}

    // Update is called once per frame
    void Update()
    {
        for(int i = 0 ; i < 2 ;i++)
        {
            if (GameData.ins.RedCharacter[i] != -1 && RedShip[i].activeSelf == false)
            {
                RedShip[i].SetActive(true);
                UIImageChanger.UIList["RedIndex" + i].ChangeImage(GameData.ins.RedCharacter[i]);
                UIImageChanger.UIList["RedIndexBig" + i].ChangeImage(GameData.ins.RedCharacter[i]);
                UIImageChanger.UIList["RedIndexLittle" + i].ChangeImage(GameData.ins.RedCharacter[i]);
            }
            if (GameData.ins.BlueCharacter[i] != -1 && BlueShip[i].activeSelf == false)
            {
                BlueShip[i].SetActive(true);
                UIImageChanger.UIList["BlueIndex" + i].ChangeImage(GameData.ins.BlueCharacter[i]);
                UIImageChanger.UIList["BlueIndexBig" + i].ChangeImage(GameData.ins.BlueCharacter[i]);
                UIImageChanger.UIList["BlueIndexLittle" + i].ChangeImage(GameData.ins.BlueCharacter[i]);
            }
	
        }
    }
}
