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
        randomBox_S,
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
        randomBox_M,
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
        russianroulette_L,
        randomBox_L,





    }
    public static List<string>[] Items = null;
    private static string TempName;
    private static void MakeItem()
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
                for (int j = 0; j < Strs.Length && Strs[j] != ""; j++)
                {
                    Items[i].Add(Strs[j]);
                }
            }
            catch (Exception e)
            {
                int a = 0;
            }
        }
    }
    public static ItemData GetItem(ITEMCODE Code)
    {
        ItemData item;
        if (Text == null)//아이템 처음들어온거면
            MakeItem();

        List<string> TempStr =Items[(int)Code];
        TempStr.Remove("\r");
        TempName = TempStr[1];
        Skill skl = null;
        if (TempStr[4] == "P")
        {
            skl = new Skill(TempName);

            //파싱한 내용에 따라서 구현
            OptionSetting(ref skl, TempStr, 6);
        }
        else
        {
            string Consumable = TempStr[5];
            int UsingNum;
            int TurnNum;
            int i = 0;
            string Str = "";

            while (Consumable[i] != 'U')
            {
                Str += Consumable[i];
                i++;
            }
            UsingNum = int.Parse(Str);
            i++;
            while (Consumable[i] != 'T')
            {
                Str += Consumable[i];
                i++;
            }
            TurnNum = int.Parse(Str);
            skl = new StackSkill(UsingNum, TempName);
            //파싱한 내용 따라서 구현

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

    public static void OptionSetting(ref Skill skill, List<string> lstr,int index)
    {
        if (lstr.Count == index)
            return;
        else
        {
            string Str = lstr[index];
            string Key = "";
            string ValueStr = "";
            double Value ;
            foreach (char e in Str)
            {
                if (!('A' <= e && e <= 'Z'))
                {
                    ValueStr += e;
                }
                else
                    Key += e;
            }
            Value = double.Parse(ValueStr);
            switch (Key)
            {
                case "DMG":
                    {
                        if(Value > 0)
                            Add_Damage(ref skill, "Attack",/*데미지*/(float)Value, TempName);
                        else
                            Add_Damage(ref skill, "Hit",/*데미지*/(float)Value, TempName);
                    }   
                    break;
                case "COST":
                    {
                        Add_Cost(ref skill, (int)Value, TempName);
                    }
                    break;
                case "HP":
                    {
                        Add_MaxHp(ref skill, (int)Value, TempName);
                    }
                    break;
                case "CRIRATE":
                    {
                        Add_BaseCriRate(ref skill, (int)Value, TempName);
                    }
                    break;
                case "ROUL":
                    {
                        Roulette(ref skill, (float)Value, Value > 0 ? true : false, TempName);
                    }
                    break;
                case "BIBLE":
                    {
                        Bible(ref skill,(int)Value,TempName);
                    }
                    break;
                default:
                    {
                        Debug.Break();
                    }
                    break;
            }

            OptionSetting(ref skill, lstr, index + 1);

        }
    }
    public static void Bible(ref Skill skl, int GetMana, string name)
    {
        skl.PassiveCount.Add("UsingCost", 0);
        skl.AddPassive(delegate (Skill skill)
        {
            skill.PassiveCount["UsingCost"] = skill.GetOrder().Cost;
        },"KeyCheck");
        skl.AddPassive(delegate (Skill skill)
        {
            float UsingCost = (int)(skill.PassiveCount["UsingCost"] - skill.GetOrder().Cost);
            skill.GetOrder().CostPlus((int)(UsingCost * 0.01f * GetMana));                        

        },"Attack");

    }
    public static void Add_Cost(ref Skill skl, int CostHeal, string name)
    {
        skl.AddPassive(delegate (Skill skill)
        {
            //체크
            CharacterStatus OrderStatus = skill.GetOrder();
            if (gameManager.ins.UserSlot[skill.Order].findSkill("Energy").PassiveCount["Switch"] != 0)
            {
                OrderStatus.CostPlus(CostHeal);
            }

        }, "Decision");//판정시 발동
    }
    public static void Add_MaxHp(ref Skill skl, int HPValue, string name)
    {
        skl.AddPassive(delegate (Skill skill)
        {
            //체크
            CharacterStatus OrderStatus = skill.GetOrder();
            OrderStatus.SetMaxHP(true, OrderStatus.MaxHP + HPValue);

        }, "GameStart");//시작시 발동
    }
    public static void Add_BaseCriRate(ref Skill skl, int Crirate, string name)
    {
        skl.AddPassive(delegate (Skill skill)
        {
            //체크
            Skill sk = SkillManager.ins.GetSkill(skill.Order, "Critical");
            sk.PassiveCount["BaseCritical"] += Crirate;
        }, "GameStart");//시작시 발동
    }
    public static void Add_Damage(ref Skill skl, string Stasis, float Damage,string name)
    {
        skl.AddPassive(delegate (Skill skill)
        {
            //체크
            DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, Damage * 0.01f, name);
        }, Stasis);//
    }
    public static void Roulette(ref Skill skill, float Value,bool isPerfect, string name)
    {
        skill.AddPassive(delegate (Skill skl)
            {
                CharacterStatus stat = skl.GetOrder();
                if (stat.usingPotion)
                    return;//포션사용했으면 탈출
                else
                {
                    float Dmg = 0;
                    if (gameManager.ins.TimingWeight[skl.Order] == gameManager.PERPECT && isPerfect)
                    {
                        //퍼펙트일때
                        Dmg = Value;
                        DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, Dmg * 0.01f, name);
                    }
                    else if(!isPerfect)
                    {
                        //아님
                        Dmg = Value;
                        DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, Dmg * 0.01f, name);
                    }
                }
            },"Attack");
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

