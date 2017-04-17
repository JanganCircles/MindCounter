using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item  {

    public static TextAsset Text = null;
    public enum ITEMCODE
    {
        aura_S,
        manaup_S,
        hpup_S,
        focusup_S,
        speedup_S,
        shortsword_S,
        armor_S,
        wand_S,
        shield_S,
        doubleaxe_S,
        glove_S,
        bible_S,
        russianroulette_S,
        aura_M,
        manaup_M,
        hpup_M,
        focusup_M,
        speedup_M,
        shortsword_M,
        armor_M,
        wand_M,
        shield_M,
        doubleaxe_M,
        glove_M,
        bible_M,
        russianroulette_M,
        aura_L,
        manaup_L,
        hpup_L,
        focusup_L,
        speedup_L,
        shortsword_L,
        armor_L,
        wand_L,
        shield_L,
        doubleaxe_L,
        glove_L,
        bible_L,
        russianroulette_L





    }
    public static List<string>[] Items = null;
    public static ItemData GetItem(ITEMCODE Code)
    {
        ItemData item;
        if (Text == null)//아이템 처음들어온거면
        {
            Text = Resources.Load("Item") as TextAsset;
            string str = Text.text;
            //Debug.Log(str);
            string[] strarr;
            strarr = str.Split('\n');
            Items = new List<string>[strarr.Length];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new List<string>();
                string[] Strs = strarr[i].Split(',');
                try
                {
                    for (int j = 0; j < Strs.Length && Strs[j] != "" ; j++)
                    {
                        Items[i].Add(Strs[j]);
                    }
                }
                catch(Exception e)
                {   
                    int a = 0;
                }
            }
        }
        List<string> TempStr =Items[(int)Code];

        string Name = TempStr[1];
        Skill skl = null;
        if (TempStr[4].Contains("DMG"))
        {
            skl = new Skill(Name);

            Add_Damage(ref skl, "Attack",/*데미지*/5, Name);
            //파싱한 내용에 따라서 구현
        }
        else
        {
            string Consumable = TempStr[5];
            int UsingNum;
            int TurnNum;
            int i = 0;
            string Str = "";
            while (Consumable != "U")
            {
                Str += Consumable[i];
                i++;
            }
            UsingNum = int.Parse(Str);
            i++;
            while (Consumable != "T")
            {
                Str += Consumable[i];
                i++;
            }
            TurnNum = int.Parse(Str);
            skl = new StackSkill(UsingNum,Name);
            //너두나두 야나두

        }
        item = new ItemData(Code,TempStr[1],int.Parse(TempStr[3]),skl);

        //디버그
        for (int i = 0; i < Items.Length; i++)
        {
            string str = "";
            for (int j = 0; j < Items[i].Count; j++)
            {
                str += Items[i][j];
            }
            Debug.Log(str);
        }
        return item;

    }


    public static void Add_CostPlus(ref Skill skl, string Stasis, int CostHeal, string name)
    {
        skl.AddPassive(delegate (Skill skill)
        {
            //체크
            CharacterStatus OrderStatus = skill.GetOrder();
            if (gameManager.ins.UserSlot[skill.Order].findSkill("Energy").PassiveCount["Switch"] != 0)
            {
                OrderStatus.CostPlus(CostHeal);
            }

        }, "Decision");//시작시 발동
    }
    public static void Add_Damage(ref Skill skl, string Stasis, float Damage,string name)
    {
        skl.AddPassive(delegate (Skill skill)
        {
            //체크
            DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, Damage * 0.01f, name);
        }, Stasis);//
    }
    public struct ItemData
    {
        ITEMCODE Code;
        string name;
        int weight;
        Skill skill;
        
        public ItemData(ITEMCODE _code, string _name,int _weight, Skill skl)
        {
            name = _name;
            Code = _code;
            weight = _weight;
            skill = skl;
        }
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

