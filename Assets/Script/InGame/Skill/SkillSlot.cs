﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SkillSlot : MonoBehaviour {

    public string[] SlotName;
    private EmptySlot[] SlotList;
    private Item.ItemData[] Items = null;
    private const int SlotLength = 7;
    public int slotLength {
        get {
            return SlotLength;
        } }
    private List<Skill> PassiveSkillList;

    public List<Skill> DebuffList;

    public int selectedSlot;    // 몇번 사용할껀지의 인덱스
    void Reset()
    {
        SlotName = new string[SlotLength];
        SlotName[0] = "약";
        SlotName[1] = "중";
        SlotName[2] = "강";
        SlotName[3] = "가드";
        SlotName[4] = "회복";//여기서부터 미구현
        SlotName[5] = "아이템0";
        SlotName[6] = "아이템1";
        SlotName[7] = "특수3";
    }
	void Awake(){
        PassiveSkillList = new List<Skill>();
        DebuffList = new List<Skill>();
        SlotList = new EmptySlot[SlotLength];
		for (int i = 0; i < SlotLength; i++) {
			SlotList[i] = new EmptySlot();
		}
		
	}
    public void SetItem(Item.ItemData item)
    {
        int index;
        if (Items == null)
        {
            Items = new Item.ItemData[2];
            index = 0;
        }
        else
        {
            index = 1;
        }
        Items[index] = item;
        GetSlot("아이템" + index.ToString()).SkillChange(Items[index].skill);
    }
	void Start () {
        CharacterStatus stat =  GetComponent<CharacterStatus>();
        KeyCode[] codes;
        string Name;
        if (stat.Controller == gameManager.CHALLANGER)
            Name = "Challanger";
        else
            Name = "Champion";
        UserKeyData.ins.SettingKey(stat.Controller,GameData.ins.OnePlayer ? 1 : 2);
        codes = UserKeyData.ins.GetKeyData(Name);
        SkillSlot sk = this;
        SkillList.ins.AddingDefaultSkill(ref sk);
        SkillList.ins.Skill_DefaultPassive(ref sk);

        //아이템은 여기 들어와야한다
        // SetItem(Item.GetItem(Item.ITEMCODE.armor_L));
        if (stat.Controller == gameManager.CHALLANGER)
        {
            SetItem(Item.GetItem(Item.ITEMCODE.russianroulette_L));
            SetItem(Item.GetItem(Item.ITEMCODE.russianroulette_S));
        }
        else
        {
            SetItem(Item.GetItem(Item.ITEMCODE.doubleaxe_L));
            SetItem(Item.GetItem(Item.ITEMCODE.doubleaxe_S));
        }
        for (int i = 0; i < SlotLength; i++)
        {
            SlotList[i].key = codes[i];
            if(SlotList[i] != null)
                SlotList[i].SetOrder(stat.Controller);
        }
    }
    public void AddPassiveSlot(Skill sk)
    {
        PassiveSkillList.Add(sk);
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
        {
            SlotList[i].RunPassive(str);
        }
        for (int i = 0; i < PassiveSkillList.Count; i++)
        {
            PassiveSkillList[i].RunPassive(str);
        }
        for (int i = 0; i < DebuffList.Count; i++)
        {
            DebuffList[i].RunPassive(str);
        }
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
        if (selectedSlot == -1 || selectedSlot == SlotLength )
            return false;
        SlotList[selectedSlot].RunActive();
        return true;
    }
    public void SelectReset()
    {
        selectedSlot = SlotLength;
    }
    public Skill findSkill(string str)
    {
        for (int i = 0; i < SlotLength; i++)
        {
            if (SlotList[i].GetName() == str)
            {
                return SlotList[i].GetSkill();
            }
        }
        for (int i = 0; i < PassiveSkillList.Count; i++)
        {
            if (PassiveSkillList[i].Name == str)
            {
                return PassiveSkillList[i];
            }
        }
        return null;
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
        if (skl == null)
        {
            int abc = 0;
        }
        TempSkill.SkillNum = index;
    }
    public Skill GetSkill()
    {
        return TempSkill;
    }
    public bool UsingThis()
    {
        if (key == KeyCode.None || TempSkill == null) return false;
        bool keybool = Input.GetKeyDown(key);
        if (gameManager.ins.Simulate)
        {
            if (Random.Range(0, 4) == 0)
            {
                keybool = true;
            }
        }
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
    public string GetName()
    {
        return TempSkill.Name;
    }
}
