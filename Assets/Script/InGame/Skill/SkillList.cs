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
        AddSkill_Energy(ref List);

    }
    //회복
    private void AddSkill_Energy(ref SkillSlot List)
    {

        CharacterStatus Orderstat;//스텟받아올 변수
        Skill Energy = new Skill(Priority.ENERGY, "Energy");//우선순위 = 가드, 이름 Guard설정
        Energy.PassiveCount.Add("Switch", 0);         //패시브카운트에 "RSPATTACK"추가

        Energy.ActiveSkillSet(delegate (Skill skil)
        {//사용시
            skil.PassiveCount["Switch"] = 1;
        });//액티브 스킬 사용

        Energy.AddPassive(delegate (Skill skl)
        {//체크
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if (skl.PassiveCount["Switch"] == 1)
            {
                skl.PassiveCount["Switch"] = 0;
                Orderstat.CostPlus(5);
            }
        }, "Decision");//시작시 발동

        List.GetSlot("회복").SkillChange(Energy);
    }

    //가드
    private void AddSkill_Guard(ref SkillSlot List)
    {
        CharacterStatus Orderstat;//스텟받아올 변수
        Skill guard = new Skill(Priority.GUARD, "Guard");//우선순위 = 가드, 이름 Guard설정
        guard.PassiveCount.Add("Cost", 1);         //패시브카운트에 "Cost"추가

        guard.ActiveSkillSet(delegate (Skill skil)
           {//사용시
               Orderstat = gameManager.ins.UserStatus[skil.Order];
               SaveData.ins.AddData(SaveData.TYPE.GUARD, Orderstat.Controller, SaveData.Try, 1);
               Orderstat.Guard = true;
               Orderstat.AttackType = -1;
               Orderstat.Cost -= (int)skil.PassiveCount["Cost"];                                        //코스트4
           });//액티브 스킬 사용

        guard.AddPassive(delegate (Skill skil)
           {//맞았을떄
               Orderstat = gameManager.ins.UserStatus[skil.Order];
               if (Orderstat.Guard)
               {
                   SaveData.ins.AddData(SaveData.TYPE.GUARD, Orderstat.Controller, SaveData.Success, 1);
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
                Orderstat.Guard = false;
            }
        }, "Start");//시작시 발동

        List.GetSlot("가드").SkillChange(guard);
    }
    //압박
    private void AddSkill_Rock(ref SkillSlot List)
    {
        CharacterStatus Orderstat;

        Skill NewSkill = new Skill(Priority.ROCK, "중");     //우선순위 = 압박, 이름 압박설정
        NewSkill.PassiveCount.Add("RSPATTACK", Stop);         //패시브카운트에 "RSPATTACK"추가
        NewSkill.PassiveCount.Add("Damage", 5);         //패시브카운트에 "RSPATTACK"추가
        NewSkill.PassiveCount.Add("Cost", 4);         //패시브카운트에 "Cost"추가

        NewSkill.ActiveSkillSet(delegate (Skill skl)
        {//사용시
            Debug.Log("CallThis - Skill_UseRock");
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Run;
            Orderstat.AttackType = Priority.ROCK;
            Orderstat.Cost -= (int)skl.PassiveCount["Cost"];                                        //코스트4

            SaveData.ins.AddData(SaveData.TYPE.ROCK, Orderstat.Controller, SaveData.Try, 1);//저장용
        });     //액티브 스킬 사용

        NewSkill.AddPassive(delegate (Skill skl)
        {//공격시
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            Debug.Log((int)skl.PassiveCount["RSPATTACK"] + " " + Run + "맞음?");
            if ((int)skl.PassiveCount["RSPATTACK"] == Run)//키누른게 이 함수가 맞는지 체크
            {
                DamageCalculator.ins.SetDamage((int)skl.PassiveCount["Damage"]);             //데미지 5
                WallManager.ins.Move((int)skl.PassiveCount["Damage"] * 20, Orderstat.Enemy());    //벽이동

                SaveData.ins.AddData(SaveData.TYPE.ROCK, Orderstat.Controller, SaveData.Success, 1);//저장용
            }
            skl.PassiveCount["RSPATTACK"] = Stop;
        }, "Attack");//공격시 발동

        NewSkill.AddPassive(Skill_UseRSPEnd, "End");           //종료시 발동

        List.GetSlot("중").SkillChange(NewSkill);//인자로 받은 리스트에 추가
    }
    //화력
    private void AddSkill_Scissors(ref SkillSlot List)
    {
        CharacterStatus Orderstat;
        
        Skill NewSkill = new Skill(Priority.SCISSOR, "약");
        NewSkill.PassiveCount.Add("RSPATTACK", Stop);
        NewSkill.PassiveCount.Add("Damage", 2);         //패시브카운트에 "RSPATTACK"추가
        NewSkill.PassiveCount.Add("Cost", 2);         //패시브카운트에 "Cost"추가

        NewSkill.ActiveSkillSet(delegate (Skill skl)
        {//사용시
            Debug.Log("CallThis - Skill_UseScissors");
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Run;
            Orderstat.AttackType = Priority.SCISSOR;
            Orderstat.Cost -= (int)skl.PassiveCount["Cost"];

            SaveData.ins.AddData(SaveData.TYPE.SCISSOR, Orderstat.Controller, SaveData.Try, 1);//저장용
        });
        NewSkill.AddPassive(delegate (Skill skl)
        {//공격시
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            Debug.Log((int)skl.PassiveCount["RSPATTACK"] + " " + Run + "맞음?");
            if ((int)skl.PassiveCount["RSPATTACK"] == Run)//키누른게 이 함수가 맞는지 체크
            {
                DamageCalculator.ins.SetDamage((int)skl.PassiveCount["Damage"]);         //데미지 2
                WallManager.ins.Move((int)skl.PassiveCount["Damage"] * 50, Orderstat.Enemy());//벽이동

                SaveData.ins.AddData(SaveData.TYPE.SCISSOR, Orderstat.Controller, SaveData.Success, 1);//저장용
            }
            skl.PassiveCount["RSPATTACK"] = Stop;
        }, "Attack");
        NewSkill.AddPassive(Skill_UseRSPEnd, "End");
        List.GetSlot("약").SkillChange(NewSkill);
    }
    //연속
    private void AddSkill_Paper(ref SkillSlot List)
    {//위와 동일
        CharacterStatus Orderstat;

        Skill NewSkill = new Skill(Priority.PAPER, "강");
        NewSkill.PassiveCount.Add("RSPATTACK", Stop);
        NewSkill.PassiveCount.Add("Damage", 8);         //패시브카운트에 "RSPATTACK"추가
        NewSkill.PassiveCount.Add("Cost", 6);         //패시브카운트에 "Cost"추가

        NewSkill.ActiveSkillSet(delegate (Skill skl)
        {//사용시
            Debug.Log("CallThis - Skill_UseRock");
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Run;
            Orderstat.AttackType = Priority.PAPER;
            Orderstat.Cost -= (int)skl.PassiveCount["Cost"];

            SaveData.ins.AddData(SaveData.TYPE.PAPER, Orderstat.Controller, SaveData.Try, 1);//저장용
        });
        NewSkill.AddPassive(delegate (Skill skl)
        {//공격시
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            Debug.Log((int)skl.PassiveCount["RSPATTACK"] + " " + Run + "맞음?");
            if ((int)skl.PassiveCount["RSPATTACK"] == Run)//키누른게 이 함수가 맞는지 체크
            {
                bool EnemyGuard = gameManager.ins.UserStatus[Orderstat.Enemy()].Guard;
                if (EnemyGuard)
                {
                    gameManager.ins.UserStatus[Orderstat.Enemy()].Guard = false;
                }
                DamageCalculator.ins.SetDamage((int)skl.PassiveCount["Damage"]);
                WallManager.ins.Move((int)skl.PassiveCount["Damage"] * 50, Orderstat.Enemy());
                SaveData.ins.AddData(SaveData.TYPE.PAPER, Orderstat.Controller, SaveData.Success, 1);//저장용
                skl.PassiveCount["RSPATTACK"] = Stop;
            }
        }, "Attack");
        NewSkill.AddPassive(delegate(Skill skl)
        {
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Stop;
        }
        , "End");
        List.GetSlot("강").SkillChange(NewSkill);
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
        List.AddPassiveSlot(DefencePlus);

        Skill DownPlus = new Skill("DownPlusDamage");//다운추뎀
        DownPlus.SetCharacter(List.GetComponent<CharacterStatus>().Controller);
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
    public const int ENERGY = 1;
    public const int GUARD = 2;
    public const int SCISSOR = 3;
    public const int ROCK = 4;
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

        if (PassiveCount.ContainsKey("Cost"))
        {
            return stat.Cost >= PassiveCount["Cost"];
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