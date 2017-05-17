﻿using UnityEngine;
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

        float judgement = gameManager.ins.TimingWeight[status.Controller];
        UITextSet.UIList[ControllerName + "Judgement"] = judgement == gameManager.PERPECT ? "Perfect" : judgement == gameManager.GOOD ? "Good" : judgement == gameManager.BAD? "Bad" : "Miss";

//        UITextSet.UIList[ControllerName + "Judgement"] = gameManager.ins.TimingWeight[status.Controller].ToString(); 


        UITextSet.UIList[ControllerName + "Hp"] = status.HP.ToString();                              //체력
        UITextSet.UIList[ControllerName + "WallDistance"] = status.WallDistance.ToString();         //벽까지 남은 거리
        string str0 = "";
///        str0 = (status.Disable ? "무력화" : "") + (status.Defence ? "방어" : "") + (status.Down ? "다운" : "");
        UITextSet.UIList[ControllerName + "Debuff"] = str0;                              //디버프 상태
        UITextSet.UIList[ControllerName + "Counter"] = status.Cost.ToString() + " / " + status.MaxCost.ToString();            //남은 코스트
        if (gameManager.ins.UIOpen)
        {
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
            case Priority.SCISSOR: return "약";
            case Priority.ROCK: return "중";
            case Priority.PAPER: return "강";
            case Priority.GUARD: return "가드";
            case Priority.ENERGY: return "에너지";
            case Priority.NONE: return "암것도안함";

        }
        return "";
    }
}
