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
        superarmor_S,
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
        superarmor_M,
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
        superarmor_L,
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
    private static Skill TempSkill;

    private static int SkillIndex;

    public static int UsingNum;
    public static int TurnNum;
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
        SkillIndex = 0;
    }
    public static ItemData GetItem(ITEMCODE Code)
    {
        ItemData item;
        if (Text == null)//아이템 처음들어온거면
            MakeItem();

        List<string> TempStr =Items[(int)Code];
        TempStr.Remove("\r");
        TempName = TempStr[1];
        if (TempStr[4] == "P")
        {
            TempSkill = new Skill(TempName);

            //파싱한 내용에 따라서 구현
            OptionSetting( TempStr, 6);
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
            TempSkill = new StackSkill(UsingNum,TempName);
            //OptionSetting(TempStr, 6);
            //파싱한 내용 따라서 구현

        }
        Debug.Log(TempName + " "+ TempStr[3] + " 성공");
        item = new ItemData(Code,TempStr[1],int.Parse(TempStr[3]), TempSkill);
        TempSkill = null;
        //디버그
        // for (int i = 0; i < Items.Length; i++)
        // {
        //     string str = "";
        //     for (int j = 0; j < Items[i].Count; j++)
        //     {
        //         str += Items[i][j];
        //     }
        //     Debug.Log(str);
        // }
        return item;

    }

    public static void OptionSetting(List<string> lstr,int index)
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
                case "PDMG": { Potion_DamageUp((int)Value); } break;
                case "PCOST": { Potion_CostHeal((int)Value); } break;
                case "PHEAL": { Potion_HpHeal((int)Value); } break;
                case "PSUAMR": { Potion_SuperArmor(); } break;
                case "PPER": { Potion_Perfect(); } break;
                case "CRIRATE": { Add_BaseCriRate((int)Value); } break;
                case "HP": {
                        Add_MaxHp((int)Value);
                    } break;
                case "COSTHEAL": { Add_Costheal((int)Value); } break;
                case "SHIELD": { Shield((int)(Value)); } break;

                //방패
                case "DMG": { Add_Damage("Attack",/*데미지*/(float)Value); } break;
                case "HIT": { Add_Damage("Hit",/*데미지*/(float)Value); } break;
                case "DOUBLE": { DoubleAttack((int)Value); } break;
                case "BIBLE": { Bible((int)Value); } break;
                case "ROUL": { Roulette((float)Value, Value > 0 ? true : false); } break;
                default:
                    {
                        Debug.Break();
                    }
                    break;
            }

            OptionSetting( lstr, index + 1);

        }
    }
    public static void Potion_DamageUp( float Damage)
    {
        StackSkill s = TempSkill as StackSkill;
        if (s == null) return;
        s.PassiveCount.Add("Count", 0);

        s.ActiveSkillSet(delegate (Skill skill)
        {            //체크
            s.PassiveCount["Count"] = 1;
        });
        s.AddPassive(delegate (Skill skill)
        {
            if (skill.PassiveCount["Count"] == 1)
            {
                DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, Damage * 0.01f + 1, "PotionUp");
            }
        }, "Attack");
        s.AddPassive(delegate (Skill skill)
        {
            s.PassiveCount["Count"] = 0;
        }, "End");
    }
    public static void Potion_CostHeal( int Cost)
    {
        StackSkill s = TempSkill as StackSkill;
        if (s == null) return;
        s.ActiveSkillSet(delegate (Skill skill)
        {
            if (s.TempStack != 0)
            {
                s.StackMinus();
                skill.GetOrder().CostPlus(Cost);
            }
        });
        TempSkill = s;
    }
    public static void Potion_HpHeal( int Heal)
    {
        StackSkill s = TempSkill as StackSkill;
        if (s == null) return;
        s.ActiveSkillSet(delegate (Skill skill)
        {
            if (s.TempStack != 0)
            {
                s.StackMinus();
                CharacterStatus Stat = skill.GetOrder();
                Stat.HP += Heal;
                if (Stat.MaxHP > Stat.HP)
                    Stat.HP = Stat.MaxHP;

            }
        });
        TempSkill = s;
    }
    public static void Potion_SuperArmor( )
    {
        StackSkill s = TempSkill as StackSkill;
        if (s == null) return;
        s.ActiveSkillSet(delegate (Skill skill)
        {
            if (s.TempStack != 0)
            {
                s.StackMinus();
                CharacterStatus Stat = skill.GetOrder();
                Stat.isSuperArmor = true;

            }
        });
        TempSkill = s;
    }
    public static void Potion_Perfect()
    {
        StackSkill s = TempSkill as StackSkill;
        if (s == null) return;
        s.PassiveCount.Add("Using",0);
        s.ActiveSkillSet(delegate (Skill skill)
        {
            s.PassiveCount["Using"] = 1;
        });
        s.AddPassive(delegate (Skill skill)
        {
            if (s.PassiveCount["Using"] == 1)
            {
                gameManager.ins.TimingWeight[s.Order] = gameManager.PERPECT;
                s.PassiveCount["Using"] = 0;
            }
        }, "Decision");

    }
    public static void Add_Costheal( int CostHeal)
    {
        TempSkill.AddPassive(delegate (Skill skill)
        {
            //체크
            CharacterStatus OrderStatus = skill.GetOrder();
            if (gameManager.ins.UserSlot[skill.Order].findSkill("Energy").PassiveCount["Switch"] != 0)
            {
                OrderStatus.CostPlus(CostHeal);
            }

        }, "Decision");//판정시 발동
    }
    public static void Shield(int shieldValue)
    {
        TempSkill.PassiveCount.Add("Shield", 0);
        TempSkill.PassiveCount.Add("Damage", 0);
        TempSkill.AddPassive(delegate (Skill skill)
        {
                TempSkill.PassiveCount["Shield"] = 0;
        }, "Start");
        TempSkill.AddPassive(delegate (Skill skill)
        {
            if (skill.GetOrder().Guard)
            {
                TempSkill.PassiveCount["Shield"] = 1;
                TempSkill.PassiveCount["Damage"] = DamageCalculator.ins.TempDamage;
            }
        }, "Hit");
        TempSkill.AddPassive(delegate (Skill skill)
        {
            if(skill.PassiveCount["Shield"] == 1)
            {
                WallManager.ins.ResetPivot();
                WallManager.ins.Move((int)TempSkill.PassiveCount["Damage"] * shieldValue, skill.GetOrder().Enemy() );

            }
        },"Wallsetting");
    }
    public static void Add_MaxHp( int HPValue)
    {
        TempSkill.PassiveCount.Add("PlusHP", HPValue);
        
        TempSkill.AddPassive(delegate (Skill skill)
        {
            //체크
            CharacterStatus OrderStatus = skill.GetOrder();
            OrderStatus.SetMaxHP(true,(int)skill.PassiveCount["PlusHP"]);

        }, "GameStart");//시작시 발동
    }
    public static void Add_BaseCriRate( int Crirate)
    {
        TempSkill.AddPassive(delegate (Skill skill)
        {
            //체크
            Skill sk = SkillManager.ins.GetSkill(skill.Order, "Critical");
            sk.PassiveCount["BaseCritical"] += Crirate;
        }, "GameStart");//시작시 발동
    }
    public static void Add_Damage( string Stasis, float Damage)
    {
        string name = TempName;
        TempSkill.AddPassive(delegate (Skill skill)
        {
            //체크
            DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, 1 + Damage * 0.01f, name);
        }, Stasis);//
    }
    public static void DoubleAttack( int Rate)
    {
        TempSkill.PassiveCount.Add("DoubleRate", Rate);
        TempSkill.AddPassive(delegate (Skill skill)
        {
            //체크
            if (skill.PassiveCount["DoubleRate"] > UnityEngine.Random.Range(0, 100))
            {
                DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, 1.5f, TempName);
            }
        }, "Attack");//

    }
    public static void Bible( int GetMana)
    {
        TempSkill.PassiveCount.Add("UsingCost", 0);
        TempSkill.AddPassive(delegate (Skill skill)
        {
            skill.PassiveCount["UsingCost"] = skill.GetOrder().Cost;
        }, "KeyCheck");
        TempSkill.AddPassive(delegate (Skill skill)
        {
            float UsingCost = (int)(skill.PassiveCount["UsingCost"] - skill.GetOrder().Cost);
            skill.GetOrder().CostPlus((int)(UsingCost * 0.01f * GetMana));

        }, "Attack");

    }
    public static void Roulette( float Value,bool isPerfect)
    {
        int a = 0;
        string name = TempName;
        if (isPerfect)
        {
            TempSkill.AddPassive(delegate (Skill TempSkill)
                {
                    CharacterStatus stat = TempSkill.GetOrder();
                    if (stat.usingPotion)
                        return;//포션사용했으면 탈출
                    else
                    {
                        if (gameManager.ins.TimingWeight[TempSkill.Order] == gameManager.PERPECT)
                        {
                            DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, 1+ Value * 0.01f, name + "성공");
                        }
                    }
                }, "Attack");
        }
        else
        {
            TempSkill.AddPassive(delegate (Skill TempSkill)
            {
                CharacterStatus stat = TempSkill.GetOrder();
                if (stat.usingPotion)
                    return;//포션사용했으면 탈출
                else
                {
                    if (gameManager.ins.TimingWeight[TempSkill.Order] != gameManager.PERPECT)
                    {
                        //아님
                        DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, (100 + Value) * 0.01f, name + "실패");
                    }
                }
            }, "Attack");
        }
    
    }
    public struct ItemData
    {
        public ITEMCODE Code;
        public string TempName;
        public int weight;
        public Skill skill;
        
        public ItemData(ITEMCODE _code, string _TempName,int _weight, Skill TempSkill)
        {
            TempName = _TempName;
            Code = _code;
            weight = _weight;
            skill = TempSkill;
        }
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

