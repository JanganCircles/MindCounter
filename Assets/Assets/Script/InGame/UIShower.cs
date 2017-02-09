using UnityEngine;
using System.Collections;

public class UIShower : MonoBehaviour {
    private CharacterStatus status;
    //private SelectState state;
    private string ControllerName;
    // Use this for initialization
    
    void Awake()
    {
        status = gameObject.GetComponent<CharacterStatus>();
        //state = gameObject.GetComponent<SelectState>();
    }
	void Start () {
        ControllerName = status.Controller == 0 ? "Champion" : "Challanger"; 
	}
	
	// Update is called once per frame
	void Update () {
        UITextSet.UIList[ControllerName + "Hp"] = status.HP.ToString();                              //체력
        //박세찬 HPbar
        
        
        UITextSet.UIList[ControllerName + "WallDistance"] = status.WallDistance.ToString();         //벽까지 남은 거리
        string str0 = "";
        string str1 = "";
        str0 = (status.Disable ? "무력화" : "") + (status.Defence ? "방어" : "") + (status.Down ? "다운" : "");
        UITextSet.UIList[ControllerName + "Debuff"] = str0;                              //디버프 상태
        if (gameManager.ins.UIOpen)
        {
            for (int i = 0; i < 3; i++)
                str1 += status.RSPTempCount[i].ToString() + " ";
            UITextSet.UIList[ControllerName + "Counter"] = str1;                              //남은 카드 갯수
            UITextSet.UIList["InputKey"] = "";                                             //입력문구
        }
        else
        {
            UITextSet.UIList[ControllerName + "Stasis"] = "' ? ."; //상태(확인불가)
            UITextSet.UIList["InputKey"] = "지금 입력 하세요";                              //입력문구
        }
    }
}
