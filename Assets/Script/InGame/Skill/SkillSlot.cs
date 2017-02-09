using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SkillSlot : MonoBehaviour {

    public string[] SlotName;
    private EmptySlot[] SlotList;
    private const int SlotLength = 4;
    private List<Skill> PassiveSkillList;
    public int selectedSlot;    // 몇번 사용할껀지의 인덱스
    void Reset()
    {
        SlotName = new string[SlotLength];
        SlotName[0] = "압박";
        SlotName[1] = "화력";
        SlotName[2] = "연속";
        SlotName[3] = "가드";
        SlotName[4] = "도발";//여기서부터 미구현
        SlotName[5] = "특수1";
        SlotName[6] = "특수2";
        SlotName[7] = "특수3";
    }
	void Awake(){
        PassiveSkillList = new List<Skill>();
        SlotList = new EmptySlot[SlotLength];
		for (int i = 0; i < SlotLength; i++) {
			SlotList[i] = new EmptySlot();
		}
		
	}
	void Start () {
        CharacterStatus stat =  GetComponent<CharacterStatus>();
        KeyCode[] codes;
        string Name;
        if (stat.Controller == gameManager.CHALLANGER)
            Name = "Challanger";
        else
            Name = "Champion";
        codes = UserKeyData.ins.GetKeyData(Name);
        SkillSlot sk = this;
        SkillList.ins.AddingSkill(ref sk);
        for (int i = 0; i < SlotLength; i++)
        {
            SlotList[i].key = codes[i];
            if(SlotList[i] != null)
                SlotList[i].SetOrder(stat.Controller);
        }
    }

    public EmptySlot GetSlot(string name)
    {
        int index = 0;
        for (int i = 0; i < SlotLength; i++)
            if (SlotName[i] == name)
            {
                index = i;
                break;
            }
        SlotList[index].index = index;
        return SlotList[index];
    }
    public void PassivesRun(string str)
    {
        for (int i = 0; i < SlotLength; i++)
            SlotList[i].RunPassive(str);
    }
    public bool KeyCheck()
    {
        for (int i = 0; i < SlotLength; i++)
            if (SlotList[i].UsingThis())
            {
                selectedSlot = i;
                return true;
            }
        return false;
    }
    public int GetPriority()
    {
        if(selectedSlot == SlotLength)
            return Priority.NONE;
        return SlotList[selectedSlot].GetPriority();
    }
    public bool RunActive()
    {
        if (selectedSlot == -1)
            return false;
        SlotList[selectedSlot].RunActive();
        return true;
    }
    public void SelectReset()
    {
        selectedSlot = SlotLength;
    }
}
public class EmptySlot
{
    public KeyCode key = KeyCode.None;
    Skill TempSkill = null;
    public int index;
    void Reset()
    {
            TempSkill = null;
    }
    public void SkillChange(Skill skl)
    {
        Reset();
        TempSkill = skl;
        TempSkill.SkillNum = index;
    }
    public bool UsingThis()
    {
        if (key == KeyCode.None || TempSkill == null) return false;
        bool keybool = Input.GetKeyDown(key);
        bool SkillOn = TempSkill.isRun();
        return keybool && SkillOn;
    }
    public int GetPriority()
    {
        return TempSkill.Priority;
    }
    public void RunPassive(string str)
    {
		if(TempSkill != null)
      	  TempSkill.RunPassive(str);
    }
    public void RunActive()
    {
        if (TempSkill != null)
            TempSkill.RunActive();
    }
    public void SetOrder(int or)
    {
        TempSkill.SetCharacter(or);
    }
}
