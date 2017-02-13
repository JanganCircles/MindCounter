using UnityEngine;
using System.Collections;

public class UIShower : MonoBehaviour {
    private CharacterStatus status;
    private SkillSlot Slots;
    private string ControllerName;
    // Use this for initialization
    void Awake()
    {
        Slots = gameObject.GetComponent<SkillSlot>();
        status = gameObject.GetComponent<CharacterStatus>();
    }
	void Start () {
        ControllerName = status.Controller == 0 ? "Champion" : "Challanger"; 
	}
	
	// Update is called once per frame
	void Update ()
    {

        UITextSet.UIList["Combo"] = (gameManager.ins.ComboContinues == 0 ? "챔피언 " : "챌린저 ") + gameManager.ins.Combo.ToString()+ "콤보" + (gameManager.ins.Combo >=8 ? "!!" : "");         //벽까지 남은 거리

        UITextSet.UIList[ControllerName + "Hp"] = status.HP.ToString();                              //체력
        UITextSet.UIList[ControllerName + "WallDistance"] = status.WallDistance.ToString();         //벽까지 남은 거리
        string str0 = "";
        string str1 = "";
        str0 = (status.Disable ? "무력화" : "") + (status.Defence ? "방어" : "") + (status.Down ? "다운" : "");
        UITextSet.UIList[ControllerName + "Debuff"] = str0;                              //디버프 상태
        if (gameManager.ins.UIOpen)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i > 0) str1 += "< ";
                str1 += status.RSPTempCount[i].ToString() + " ";
            }
            UITextSet.UIList[ControllerName + "Counter"] = str1;            //남은 카드 갯수
            UITextSet.UIList["InputKey"] = "";                              //입력문구
            UITextSet.UIList[ControllerName + "Stasis"] = SetTempStatis(Slots.GetPriority()); //상태(확인불가)
        }
        else
        {
            if (gameManager.ins.Simulate)
            {
                UITextSet.UIList["InputKey"] = "AI돌리는중";                              //입력문구
            }
            else
                UITextSet.UIList["InputKey"] = "지금 입력 하세요";                              //입력문구
        }
    }
    public string SetTempStatis(int Index)
    {
        switch (Index)
        {
            case Priority.ROCK: return "압박";
            case Priority.SCISSOR: return "화력";
            case Priority.PAPER: return "연속";
            case Priority.GUARD: return "가드";
            case Priority.PROVOKE: return "도발";
            case Priority.NONE: return "암것도안함";

        }
        return "";
    }
}
