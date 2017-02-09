using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillList : MonoBehaviour {

    public static SkillList ins = null;
    public enum CHARACTERS
    {
        MUST,
        BOXER,
        LAST
    }
    // Use this for initialization
    void Awake()    {
        Debug.Log("스킬리스트 어웨이크");
        if(ins == null)
            ins = this;
    }
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }
    public void AddingSkill(ref SkillSlot List)
    {
        CharacterStatus Orderstat;
        //가드
        Skill guard = new Skill(Priority.GUARD);
        guard.ActiveSkillSet(
           delegate (Skill skil)
           {
               Orderstat = gameManager.ins.UserStatus[skil.Order];
               Orderstat.Guard = true;
               Orderstat.AttackType = -1;
           });
        guard.AddPassive(
           delegate (Skill skil)
           {
               Orderstat = gameManager.ins.UserStatus[skil.Order];
               if (Orderstat.Guard)
               {
                   DebuffManager.ins.Clean();
                   DebuffManager.ins.NextDisable = false;
                   if (Orderstat.Defence)
                       DebuffManager.ins.OnDefence();
                   if (Orderstat.Down)
                       DebuffManager.ins.OnDown();
                   if (Orderstat.WallDistance == 0)
                   {
                       DamageCalculator.ins.AddDamage("Multiple", 0.5f, "GuardBlock");
                   }
                   else
                   {
                       DamageCalculator.ins.AddDamage("Multiple", 0, "GuardBlock");
                       WallManager.ins.ResetPivot();
                       WallManager.ins.Move(100, Orderstat.Controller);
                   }
               }
           }, "Hit");
        guard.AddPassive(delegate (Skill skl)
        {
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if (Orderstat.Guard)
            {
                for (int i = 0; i < 3; i++)
                    Orderstat.RSPPlus(i);
                Orderstat.Guard = false;
            }
        }, "End");
        List.GetSlot("가드").SkillChange(guard);
        //가드끝
        for (int i = Priority.ROCK; i <= Priority.PAPER; i++)
        {
            Skill NewSkill = new Skill(i);
            string Names = "";
            switch (i)
            {
                case Priority.ROCK:
                    {
                        NewSkill.PassiveCount.Add("RSPATTACK",-1);
                        NewSkill.AddPassive(delegate (Skill skl)
                        {
                            Orderstat = gameManager.ins.UserStatus[skl.Order];
                            Debug.Log((int)skl.PassiveCount["RSPATTACK"] + " " + Priority.ROCK + "맞음?");
                            if ((int)skl.PassiveCount["RSPATTACK"] == Priority.ROCK)
                            {
                                DamageCalculator.ins.SetDamage(50);
                                DebuffManager.ins.OnDefence();
                                WallManager.ins.Move(50, Orderstat.Enemy());
                            }
                            skl.PassiveCount["RSPATTACK"] = -1;
                        }, "Attack");

                        NewSkill.ActiveSkillSet(Skill_UseRock);
                        NewSkill.AddPassive(Skill_UseRSPEnd, "End");
                        Names = "압박";
                    }
                    break;

                case Priority.SCISSOR:
                    {
                        NewSkill.PassiveCount.Add("RSPATTACK", -1);
                        NewSkill.AddPassive(delegate (Skill skl)
                        {
                            Orderstat = gameManager.ins.UserStatus[skl.Order];
                            Debug.Log((int)skl.PassiveCount["RSPATTACK"] + " " + Priority.SCISSOR + "맞음?");
                            if ((int)skl.PassiveCount["RSPATTACK"] == Priority.SCISSOR)
                            {
                                DamageCalculator.ins.SetDamage(100);
                                DebuffManager.ins.OnDown();
                                WallManager.ins.Move(100, Orderstat.Enemy());
                            }
                            skl.PassiveCount["RSPATTACK"] = -1;
                        }, "Attack");
                        NewSkill.ActiveSkillSet(Skill_UseScissor);
                        NewSkill.AddPassive(Skill_UseRSPEnd, "End");
                        Names = "화력";
                    }
                    break;

                case Priority.PAPER:
                    {
                        NewSkill.PassiveCount.Add("RSPATTACK", -1);
                        NewSkill.AddPassive(delegate (Skill skl)
                        {
                            Orderstat = gameManager.ins.UserStatus[skl.Order];
                            Debug.Log((int)skl.PassiveCount["RSPATTACK"] + " " + Priority.PAPER + "맞음?");
                            if ((int)skl.PassiveCount["RSPATTACK"] == Priority.PAPER)
                            {
                                DamageCalculator.ins.SetDamage(40);
                                DebuffManager.ins.OnDefence();
                                DebuffManager.ins.NextDisable = true;
                                WallManager.ins.Move(40, Orderstat.Enemy());
                            }
                            skl.PassiveCount["RSPATTACK"] = -1;
                        }, "Attack");
                        NewSkill.ActiveSkillSet(Skill_UsePaper);
                        NewSkill.AddPassive(Skill_UseRSPEnd,"End");
                    //    AddSkill_AttackPassive(NewSkill, Priority.PAPER, "RSPATTACK");
                         Names = "연속";
                    }
                    break;
            }
			List.GetSlot(Names).SkillChange(NewSkill);
        }
        Debug.Log(List.GetInstanceID());

    }
    public void AddSkill_AttackPassive(Skill skills ,int Priority,string AttackName)
    {
        skills.ActiveSkillSet(
            delegate (Skill skl)
            {
                CharacterStatus Orderstat = gameManager.ins.UserStatus[skl.Order];
                skl.PassiveCount[AttackName] = Priority;

                Orderstat.AttackType = Priority;
            });
        skills.AddPassive(delegate (Skill skl)
        {
            CharacterStatus Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount[AttackName] = -1;
        }, "End");
    }
    public void Skill_UseRSPEnd(Skill skl)
    {
        CharacterStatus Orderstat = gameManager.ins.UserStatus[skl.Order];
        skl.PassiveCount["RSPATTACK"] = -1;
    }
    public void Skill_UseRock(Skill skl)
    {
        Debug.Log("CallThis - Skill_UseRock");
        CharacterStatus Orderstat = gameManager.ins.UserStatus[skl.Order];
        skl.PassiveCount["RSPATTACK"] = Priority.ROCK;
        Orderstat.AttackType = Priority.ROCK;
        Orderstat.RSPTempCount[skl.SkillNum]--;
    }
    public void Skill_UseScissor(Skill skl)
    {
        Debug.Log("CallThis - Skill_UseScissor");
        CharacterStatus Orderstat = gameManager.ins.UserStatus[skl.Order];
        skl.PassiveCount["RSPATTACK"] = Priority.SCISSOR;
        Orderstat.AttackType = Priority.SCISSOR;
        Orderstat.RSPTempCount[skl.SkillNum]--;
    }
    public void Skill_UsePaper(Skill skl)
    {
        Debug.Log("CallThis - Skill_UsePaper");
        CharacterStatus Orderstat = gameManager.ins.UserStatus[skl.Order];
        skl.PassiveCount["RSPATTACK"] = Priority.PAPER;
        Orderstat.AttackType = Priority.PAPER;
        Orderstat.RSPTempCount[skl.SkillNum]--;
    }
    //도발 '' 
    public void Skill_DefaultPassive(Skill skl)
    {

    } 

}

public class StackSkill : Skill {
    
    public int MaxStack = 0;
    public int TempStack;

    public void ChangeMaxStack(int num)
    {
        TempStack =MaxStack= num;
    }
    public StackSkill(Active actives,int Pri) : base( Pri)
    {
    }
    public StackSkill(Active actives,int _MaxStack,int Pri) : base( Pri)
    {
        TempStack = MaxStack;
    }
    public StackSkill( int _MaxStack) : base()
    {
        TempStack = MaxStack;
    }
    public void StackPlus()
    {
        if(MaxStack != TempStack)
        TempStack++;
    }
    public bool StackMinus()
    {
        if (TempStack == 0)
        {
            return false;
        }
        else
            TempStack--;
        return true;
    }
}
public class Priority
{
    public const int NONE = 0;
    public const int PROVOKE = 1;
    public const int GUARD = 2;
    public const int ROCK = 3;
    public const int SCISSOR = 4;
    public const int PAPER = 5;
}
public class Skill {

    public int Order = -1;
    protected int Enemy;
    public delegate void Active(Skill This);   //액티브delegate
    public Active TempActive = null;                                                   //현재 액티브
    public Dictionary<string, Active> PassiveList;                             //패시브들
    public Dictionary<string, float> PassiveCount;                              //패시브에 사용할 실수 값
    public int Priority = 0;                            //해당 액티브의 우선순위 값
    public int Cost;
    public int SkillNum;                                //뭐하는스킬인가 0~2 RSP 3 가드 4 도발 5~7 스페셜
    public bool isRunning = true;                                  //돌아갈 수 있는가
    public Skill(int Pri)
    {
        //생성자
        Priority = Pri;
        PassiveList = new Dictionary<string, Active>();
        PassiveCount = new Dictionary<string, float>();
    }
    public void ActiveSkillSet(Active actives)
    {
        TempActive = actives;
    }
    public Skill()
    {
        //생성자
        TempActive = null;
        PassiveList = new Dictionary<string, Active>();
    }
    public void SetCharacter(int num)
    {
        Order = num;
        Enemy = num + 1 % 2;
    }
    public void AddPassive(Active pas, string str, float Num)//패시브함수, 발동할상태, 사용할함수
    {
        PassiveList.Add(str, pas);
        PassiveCount.Add(str, Num);
    }
    public void AddPassive(Active pas, string str)//패시브함수, 발동할상태
    {
        PassiveList.Add(str, pas);
    }
    virtual public bool isRun()//돌아갈 수 있는가
    {
        if (Order == -1 || isRunning == false) return false;
        CharacterStatus stat = gameManager.ins.UserStatus[Order];

        if (SkillNum >= 0 && SkillNum <= 2)
        {
            return stat.RSPTempCount[SkillNum] != 0;
        }
        else if (SkillNum == 3)
        {
            return !stat.Down;
        }
        else if (SkillNum <= 5)
        {
            return stat.SpecialPower > Cost;
        }
        return true;
    }
    virtual public void RunActive()//액티브함수실행
    {
        if(Order != -1)
           TempActive(this);
    }
    virtual public void RunPassive(string str)//해당 패시브 실행
    {
        if (PassiveList.ContainsKey(str))
        {
            if (Order != -1)
                PassiveList[str](this);
        }
    }
}