using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillList : MonoBehaviour {

    public static SkillList ins = null;

    private const int Run = 1;      //클래스에서 사용할 상수
    private const int Stop = -1;    //위동일
    
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
    public void AddingDefaultSkill(ref SkillSlot List)//기본스킬추가
    {
        AddSkill_Guard(ref List);
        AddSkill_Rock(ref List);
        AddSkill_Scissors(ref List);
        AddSkill_Paper(ref List);

    }
    //가드
    private void AddSkill_Guard(ref SkillSlot List)
    {
        CharacterStatus Orderstat;//스텟받아올 변수
        Skill guard = new Skill(Priority.GUARD, "Guard");//우선순위 = 가드, 이름 Guard설정

        guard.ActiveSkillSet(delegate (Skill skil)
           {//사용시
               Orderstat = gameManager.ins.UserStatus[skil.Order];
               SaveData.ins.AddData(SaveData.TYPE.GUARD, Orderstat.Controller, SaveData.Try, 1);
               Orderstat.Guard = true;
               Orderstat.AttackType = -1;
           });//액티브 스킬 사용

        guard.AddPassive(delegate (Skill skil)
           {//맞았을떄
               Orderstat = gameManager.ins.UserStatus[skil.Order];
               if (Orderstat.Guard)
               {
                   SaveData.ins.AddData(SaveData.TYPE.GUARD, Orderstat.Controller, SaveData.Success, 1);
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
                       WallManager.ins.Move(50, Orderstat.Controller);
                   }
               }
           }, "Hit");//피격시 발동

        guard.AddPassive(delegate (Skill skl)
        {//사후처리
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if (Orderstat.Guard)
            {
                for (int i = 0; i < 3; i++)
                    Orderstat.RSPPlus(i);
            }
        }, "End");//시작시 발동
        guard.AddPassive(delegate (Skill skl)
        {//사후처리
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if (Orderstat.Guard)
            {
                Orderstat.Guard = false;
            }
        }, "Start");//시작시 발동

        List.GetSlot("가드").SkillChange(guard);
    }
    //압박
    private void AddSkill_Rock(ref SkillSlot List)
    {
        CharacterStatus Orderstat;

        Skill NewSkill = new Skill(Priority.ROCK, "압박");     //우선순위 = 압박, 이름 압박설정
        NewSkill.PassiveCount.Add("RSPATTACK", Stop);         //패시브카운트에 "RSPATTACK"추가
        NewSkill.PassiveCount.Add("Damage", 50);         //패시브카운트에 "RSPATTACK"추가

        NewSkill.ActiveSkillSet(delegate (Skill skl)
        {//사용시
            Debug.Log("CallThis - Skill_UseRock");
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Run;
            Orderstat.AttackType = Priority.ROCK;
            Orderstat.RSPTempCount[skl.SkillNum]--;

            SaveData.ins.AddData(SaveData.TYPE.ROCK, Orderstat.Controller, SaveData.Try, 1);//저장용
        });     //액티브 스킬 사용

        NewSkill.AddPassive(delegate (Skill skl)
        {//공격시
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            Debug.Log((int)skl.PassiveCount["RSPATTACK"] + " " + Run + "맞음?");
            if ((int)skl.PassiveCount["RSPATTACK"] == Run)//키누른게 이 함수가 맞는지 체크
            {
                DamageCalculator.ins.SetDamage((int)skl.PassiveCount["Damage"]);             //데미지 50
                DebuffManager.ins.OnDefence();                  //디버프설정-방어
                WallManager.ins.Move((int)skl.PassiveCount["Damage"], Orderstat.Enemy());    //벽이동

                SaveData.ins.AddData(SaveData.TYPE.ROCK, Orderstat.Controller, SaveData.Success, 1);//저장용
            }
            skl.PassiveCount["RSPATTACK"] = Stop;
        }, "Attack");//공격시 발동

        NewSkill.AddPassive(Skill_UseRSPEnd, "End");           //종료시 발동

        List.GetSlot("압박").SkillChange(NewSkill);//인자로 받은 리스트에 추가
    }
    //화력
    private void AddSkill_Scissors(ref SkillSlot List)
    {
        CharacterStatus Orderstat;
        
        Skill NewSkill = new Skill(Priority.SCISSOR, "화력");
        NewSkill.PassiveCount.Add("RSPATTACK", Stop);
        NewSkill.PassiveCount.Add("Damage", 100);         //패시브카운트에 "RSPATTACK"추가

        NewSkill.ActiveSkillSet(delegate (Skill skl)
        {//사용시
            Debug.Log("CallThis - Skill_UseRock");
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Run;
            Orderstat.AttackType = Priority.SCISSOR;
            Orderstat.RSPTempCount[skl.SkillNum]--;

            SaveData.ins.AddData(SaveData.TYPE.SCISSOR, Orderstat.Controller, SaveData.Try, 1);//저장용
        });
        NewSkill.AddPassive(delegate (Skill skl)
        {//공격시
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            Debug.Log((int)skl.PassiveCount["RSPATTACK"] + " " + Run + "맞음?");
            if ((int)skl.PassiveCount["RSPATTACK"] == Run)//키누른게 이 함수가 맞는지 체크
            {
                DamageCalculator.ins.SetDamage((int)skl.PassiveCount["Damage"]);         //데미지 100
                DebuffManager.ins.OnDown();                  //디버프설정-다운
                WallManager.ins.Move((int)skl.PassiveCount["Damage"], Orderstat.Enemy());//벽이동

                SaveData.ins.AddData(SaveData.TYPE.SCISSOR, Orderstat.Controller, SaveData.Success, 1);//저장용
            }
            skl.PassiveCount["RSPATTACK"] = Stop;
        }, "Attack");
        NewSkill.AddPassive(Skill_UseRSPEnd, "End");
        List.GetSlot("화력").SkillChange(NewSkill);
    }
    //연속
    private void AddSkill_Paper(ref SkillSlot List)
    {//위와 동일
        CharacterStatus Orderstat;

        Skill NewSkill = new Skill(Priority.PAPER, "연속");
        NewSkill.PassiveCount.Add("RSPATTACK", Stop);
        NewSkill.PassiveCount.Add("Damage", 40);         //패시브카운트에 "RSPATTACK"추가

        NewSkill.ActiveSkillSet(delegate (Skill skl)
        {//사용시
            Debug.Log("CallThis - Skill_UseRock");
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Run;
            Orderstat.AttackType = Priority.PAPER;
            Orderstat.RSPTempCount[skl.SkillNum]--;

            SaveData.ins.AddData(SaveData.TYPE.PAPER, Orderstat.Controller, SaveData.Try, 1);//저장용
        });
        NewSkill.AddPassive(delegate (Skill skl)
        {//공격시
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            Debug.Log((int)skl.PassiveCount["RSPATTACK"] + " " + Run + "맞음?");
            if ((int)skl.PassiveCount["RSPATTACK"] == Run)//키누른게 이 함수가 맞는지 체크
            {
                bool EnemyGuard = gameManager.ins.UserStatus[Orderstat.Enemy()].Guard;
                DamageCalculator.ins.SetDamage((int)skl.PassiveCount["Damage"]);
                DebuffManager.ins.OnDefence();
                DebuffManager.ins.NextDisable = true;
                WallManager.ins.Move((int)skl.PassiveCount["Damage"], Orderstat.Enemy());
                if (EnemyGuard)
                {
                    Orderstat.RSPTempCount[2] = 0;
                }
                SaveData.ins.AddData(SaveData.TYPE.PAPER, Orderstat.Controller, SaveData.Success, 1);//저장용
                skl.PassiveCount["RSPATTACK"] = Stop;
            }
        }, "Attack");
        NewSkill.AddPassive(delegate(Skill skl)
        {
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if(skl.PassiveCount["RSPATTACK"] == Run)
                Orderstat.RSPTempCount[2] = 0;
            skl.PassiveCount["RSPATTACK"] = Stop;
        }
        , "End");
        List.GetSlot("연속").SkillChange(NewSkill);
    }
    public void Skill_UseRSPEnd(Skill skl)
    {
        CharacterStatus Orderstat = gameManager.ins.UserStatus[skl.Order];
        skl.PassiveCount["RSPATTACK"] = Stop;
    }
    //도발 '' 


    public void Skill_DefaultPassive(ref SkillSlot List)//게임시스템패시브(치명타, 방어추뎀, 다운추뎀)
    {
        CharacterStatus OrderStat;
        CharacterStatus Enemy;
        Skill Critical = new Skill("Critical");//치명타
        Critical.PassiveCount.Add("Critical", 5);
        Critical.SetCharacter(List.GetComponent<CharacterStatus>().Controller);
        Critical.AddPassive(
           delegate (Skill skil)
           {
               OrderStat = gameManager.ins.UserStatus[skil.Order];
               if (Random.Range(0, (100 / skil.PassiveCount["Critical"])) < 1)//5%
               {
                   DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, 2f, "Critical");
                   SaveData.ins.AddData(SaveData.TYPE.CRITICAL, skil.Order, 1);
               }
           }
           , "Attack");
        List.AddPassiveSlot(Critical);

        Skill DefencePlus = new Skill("DefencePlusDamage");//방어추뎀
        DefencePlus.SetCharacter(List.GetComponent<CharacterStatus>().Controller);
        DefencePlus.AddPassive(
           delegate (Skill skil)
           {
               OrderStat = gameManager.ins.UserStatus[skil.Order];
               Enemy = gameManager.ins.UserStatus[OrderStat.Enemy()];
               if(Enemy.Defence)//방어시 10%
                   DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, 1.1f, "Defence");
           }
           , "Attack");
        List.AddPassiveSlot(DefencePlus);

        Skill DownPlus = new Skill("DownPlusDamage");//다운추뎀
        DownPlus.SetCharacter(List.GetComponent<CharacterStatus>().Controller);
        DownPlus.AddPassive(
           delegate (Skill skil)
           {
               OrderStat = gameManager.ins.UserStatus[skil.Order];
               Enemy = gameManager.ins.UserStatus[OrderStat.Enemy()];
               if (Enemy.Down)//다운시 20%
                   DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, 1.2f, "Down");
           }
           , "Attack");
        List.AddPassiveSlot(DownPlus);
    } 

}

public class StackSkill : Skill {
    
    public int MaxStack = 0;
    public int TempStack;

    public void ChangeMaxStack(int num)
    {
        TempStack =MaxStack= num;
    }
    public StackSkill(int _MaxStack, string name) : base(name)
    {
        TempStack = MaxStack = _MaxStack;
    }
    public StackSkill(int _MaxStack,int Pri,string name) : base( Pri,name)
    {
        TempStack = MaxStack = _MaxStack;
    }
    public void StackPlus()
    {
        if(MaxStack != TempStack)
        TempStack++;
    }
    public void StackSet(int num)
    {
        TempStack = num;
        if (MaxStack < TempStack)
            TempStack = MaxStack;
        else if (0 > TempStack)
            TempStack = 0;
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
    public string Name;                     //이름
    public Skill(int Pri, string _Name )
    {
        //생성자
        Name = _Name;
        Priority = Pri;
        PassiveList = new Dictionary<string, Active>();
        PassiveCount = new Dictionary<string, float>();
    }
    public void ActiveSkillSet(Active actives)
    {
        TempActive = actives;
    }
    public Skill(string _Name)
    {
        //생성자
        Name = _Name;
        TempActive = null;
        PassiveList = new Dictionary<string, Active>();
        PassiveCount = new Dictionary<string, float>();
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
            {
                PassiveList[str](this);
            }
        }
    }
}