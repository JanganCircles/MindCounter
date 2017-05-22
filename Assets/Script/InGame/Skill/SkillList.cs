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
    void Start()
    {
    }
	// Update is called once per frame
	void Update () {

    }
    public void AddingDefaultSkill(ref SkillSlot List)//기본스킬추가
    {
        Debug.Log("abcd");
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
            skil.GetOrder().DontDash = true;
            gameManager.ins.AnimationSetting(skil.Order, 0, CharacterAnim.AnimStasis.MANA);
            gameManager.ins.AnimationSetting(skil.Order, 1, CharacterAnim.AnimStasis.MANA);
            skil.PassiveCount["Switch"] = 1;
        });//액티브 스킬 사용

        Energy.AddPassive(delegate (Skill skl)
        {//체크
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if (skl.PassiveCount["Switch"] == 1)
            {
                EffectManager.ins.EffectRun(skl.GetOrder().transform, Vector3.one, "Energy", false);
                CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.MANA, skl.Order);
                Orderstat.CostPlus((int)(CostData.EnergyRecovery * gameManager.ins.TimingWeight[skl.Order]));
            }
        }, "Decision");//시작시 발동
        Energy.AddPassive(delegate (Skill skl)
        {
            if (skl.GetEnemy().Guard)
                gameManager.ins.AnimationSetting(skl.Order, 1, CharacterAnim.AnimStasis.MANA);
        }
        , "Hit");
        Energy.AddPassive(delegate (Skill skl)
        {
            skl.GetOrder().DontDash = false;
            if (skl.PassiveCount["Switch"] == 1)
                skl.PassiveCount["Switch"] = 0;
        }
        ,"End");

        List.GetSlot("회복").SkillChange(Energy);
    }

    //가드
    private void AddSkill_Guard(ref SkillSlot List)
    {
        CharacterStatus Orderstat;//스텟받아올 변수
        Skill guard = new Skill(Priority.GUARD, "Guard");//우선순위 = 가드, 이름 Guard설정
        guard.PassiveCount.Add("Cost", CostData.Guard);         //패시브카운트에 "Cost"추가

        guard.ActiveSkillSet(delegate (Skill skil)
        {//사용시    
            skil.GetOrder().DontDash = true;
            gameManager.ins.AnimationSetting(skil.Order, 0, CharacterAnim.AnimStasis.GUARD);
            gameManager.ins.AnimationSetting(skil.Order, 1, CharacterAnim.AnimStasis.GUARD);
            Orderstat = gameManager.ins.UserStatus[skil.Order];
               SaveData.ins.AddData(SaveData.TYPE.GUARD, Orderstat.Controller, SaveData.Try, 1);
               Orderstat.Guard = true;
               Orderstat.AttackType = -1;
               Orderstat.CostUse((int)skil.PassiveCount["Cost"]);                                      //코스트4
           });//액티브 스킬 사용

        guard.AddPassive(delegate (Skill skil)
           {//맞았을떄
               Orderstat = gameManager.ins.UserStatus[skil.Order];
               if (Orderstat.Guard)
               {
                   gameManager.ins.AnimationSetting(skil.Order, 1, CharacterAnim.AnimStasis.GUARD);
                   SaveData.ins.AddData(SaveData.TYPE.GUARD, Orderstat.Controller, SaveData.Success, 1);
                   if (Orderstat.WallDistance == 0)
                   {
                       if (Random.Range(1, 10) == 1)
                       {
                           float BlockPer = 0.5f - (1 - gameManager.ins.TimingWeight[skil.Order]);
                           BlockPer = BlockPer < 0 ? 0 : BlockPer;
                           DamageCalculator.ins.AddDamage("Multiple", BlockPer, "GuardBlock");
                       }
                   }
                   else
                   {
                       float BlockPer = 1 - gameManager.ins.TimingWeight[skil.Order];
                       BlockPer = BlockPer < 0 ? 0 : BlockPer;
                       DamageCalculator.ins.AddDamage("Multiple", BlockPer, "GuardBlock");
                   }
               }
           }, "Hit");//피격시 발동

        guard.AddPassive(delegate (Skill skl)
        {//체크
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if (Orderstat.Guard)
            {
                CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.GUARD, skl.Order);
            }
        }, "Decision");//시작시 발동
        guard.AddPassive(delegate (Skill skl)
        {//사후처리
            skl.GetOrder().DontDash = false;
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
        NewSkill.PassiveCount.Add("Damage", DamageData.Middle);         //패시브카운트에 "RSPATTACK"추가
        NewSkill.PassiveCount.Add("Cost", CostData.Middle);         //패시브카운트에 "Cost"추가
        NewSkill.PassiveCount.Add("KnockBack", KnockBackData.Middle);         //패시브카운트에 "KnockBack"추가

        NewSkill.ActiveSkillSet(delegate (Skill skl)
        {//사용시
            gameManager.ins.AnimationSetting(skl.Order, 0, CharacterAnim.AnimStasis.MATK);
            gameManager.ins.AnimationSetting(skl.Order, 1, CharacterAnim.AnimStasis.MATK);
            Debug.Log("CallThis - Skill_UseRock");
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Run;
            Orderstat.AttackType = Priority.ROCK;
            Orderstat.CostUse((int)skl.PassiveCount["Cost"]);                                      //코스트4

            SaveData.ins.AddData(SaveData.TYPE.ROCK, Orderstat.Controller, SaveData.Try, 1);//저장용
        });     //액티브 스킬 사용

        NewSkill.AddPassive(delegate (Skill skl)
        {//공격시
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if ((int)skl.PassiveCount["RSPATTACK"] == Run)//키누른게 이 함수가 맞는지 체크
            {
                gameManager.ins.AnimationSetting(skl.Order, 2, CharacterAnim.AnimStasis.IDLE);
                DamageCalculator.ins.SetDamage((int)skl.PassiveCount["Damage"]);             //데미지 5
                WallManager.ins.Move((int)(skl.PassiveCount["KnockBack"]* gameManager.ins.TimingWeight[skl.Order]) , Orderstat.Enemy());    //벽이동

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
        NewSkill.PassiveCount.Add("Damage", DamageData.Weak);         //패시브카운트에 "RSPATTACK"추가
        NewSkill.PassiveCount.Add("Cost", CostData.Weak);         //패시브카운트에 "Cost"추가
        NewSkill.PassiveCount.Add("KnockBack", KnockBackData.Weak);         //패시브카운트에 "KnockBack"추가

        NewSkill.ActiveSkillSet(delegate (Skill skl)
        {//사용시
            gameManager.ins.AnimationSetting(skl.Order, 0, CharacterAnim.AnimStasis.SATK);
            gameManager.ins.AnimationSetting(skl.Order, 1, CharacterAnim.AnimStasis.SATK);
            Debug.Log("CallThis - Skill_UseScissors");
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Run;
            Orderstat.AttackType = Priority.SCISSOR;
            Orderstat.CostUse((int)skl.PassiveCount["Cost"]);

            SaveData.ins.AddData(SaveData.TYPE.SCISSOR, Orderstat.Controller, SaveData.Try, 1);//저장용
        });
        NewSkill.AddPassive(delegate (Skill skl)
        {//공격시
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if ((int)skl.PassiveCount["RSPATTACK"] == Run)//키누른게 이 함수가 맞는지 체크
            {
                gameManager.ins.AnimationSetting(skl.Order, 2, CharacterAnim.AnimStasis.IDLE);
                DamageCalculator.ins.SetDamage((int)skl.PassiveCount["Damage"]);         //데미지 2
                WallManager.ins.Move((int)(skl.PassiveCount["KnockBack"] * gameManager.ins.TimingWeight[skl.Order]), Orderstat.Enemy());    //벽이동

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
        NewSkill.PassiveCount.Add("Damage", DamageData.Strong);         //패시브카운트에 "RSPATTACK"추가
        NewSkill.PassiveCount.Add("Cost", CostData.Strong);         //패시브카운트에 "Cost"추가
        NewSkill.PassiveCount.Add("KnockBack", KnockBackData.Strong);         //패시브카운트에 "KnockBack"추가

        NewSkill.ActiveSkillSet(delegate (Skill skl)
        {//사용시
            gameManager.ins.AnimationSetting(skl.Order, 0, CharacterAnim.AnimStasis.LATK);
            gameManager.ins.AnimationSetting(skl.Order, 1, CharacterAnim.AnimStasis.LATK);
            Debug.Log("CallThis - Skill_UseRock");
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            skl.PassiveCount["RSPATTACK"] = Run;
            Orderstat.AttackType = Priority.PAPER;
            Orderstat.CostUse((int)skl.PassiveCount["Cost"]);

            SaveData.ins.AddData(SaveData.TYPE.PAPER, Orderstat.Controller, SaveData.Try, 1);//저장용
        });
        NewSkill.AddPassive(delegate (Skill skl)
        {//공격시
            Orderstat = gameManager.ins.UserStatus[skl.Order];
            if ((int)skl.PassiveCount["RSPATTACK"] == Run)//키누른게 이 함수가 맞는지 체크
            {
                gameManager.ins.AnimationSetting(skl.Order, 2,CharacterAnim.AnimStasis.IDLE);
                DamageCalculator.ins.SetDamage((int)skl.PassiveCount["Damage"]);
                WallManager.ins.Move((int)(skl.PassiveCount["KnockBack"] * gameManager.ins.TimingWeight[skl.Order]), Orderstat.Enemy());    //벽이동
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
        Skill Critical = new Skill("Critical");//치명타
        Critical.PassiveCount.Add("Critical", ProbabilityData.Critical);
        Critical.PassiveCount.Add("BaseCritical", ProbabilityData.Critical);
        Critical.SetCharacter(List.GetComponent<CharacterStatus>().Controller);
        Critical.AddPassive(
           delegate (Skill skil)
           {
               OrderStat = gameManager.ins.UserStatus[skil.Order];
				if(gameManager.ins.UserStatus[OrderStat.Enemy()].WallDistance == 0)
				{
					skil.PassiveCount["Critical"] += 80;
				}
               if (Random.Range(0, (100 / skil.PassiveCount["Critical"])) < 1 )//5%
               {
                   Debug.Log("크리티컬");
                   DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, 1.5f, "Critical");
                   SaveData.ins.AddData(SaveData.TYPE.CRITICAL, skil.Order, 1);
				}
				if(gameManager.ins.UserStatus[OrderStat.Enemy()].WallDistance == 0)
				{
					skil.PassiveCount["Critical"] -= 80;
				}
           }
           , "Attack");
        List.AddPassiveSlot(Critical);
        
        Skill SuperArmor = new Skill("SuperArmor");
        SuperArmor.SetCharacter(List.GetComponent<CharacterStatus>().Controller);
        SuperArmor.PassiveCount.Add("isHit", 0);
        SuperArmor.AddPassive(delegate (Skill skill)
        {
            skill.PassiveCount["isHit"] = 1;
        }, "Hit");
        SuperArmor.AddPassive(delegate (Skill skill) 
        {
            if (gameManager.ins.UserStatus[skill.Order].isSuperArmor && skill.PassiveCount["isHit"] == 1)
            {
                CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.LAND, skill.Order);
                WallManager.ins.ResetPivot();
            }

            skill.PassiveCount["isHit"] = 0;
        }, "WallSetting");
        List.AddPassiveSlot(SuperArmor);
    } 

}

public class StackSkill : Skill {
    
    public int MaxStack = 0;
    public int TempStack;
    public bool isUseToStack = false;

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
    override public bool RunActive()//액티브함수실행
    {
        if (Order != -1)
        {
            if (isUseToStack && TempStack == 0)
            {
                return false;
            }
            else
            {
                TempActive(this);
                if (isUseToStack)
                    StackMinus();

                return isUseTurn;
            }
        }
        return false;
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
                               //돌아갈 수 있는가
    public string Name;              
    public int Order = -1;
    protected int Enemy;
    public delegate void Active(Skill This);                                   //액티브delegate
    public Active TempActive = null;                                           //현재 액티브
    public Dictionary<string, Active> PassiveList;                             //패시브들
    public Dictionary<string, float> PassiveCount;                             //패시브에 사용할 실수 값
    public int Priority = 0;                                                   //해당 액티브의 우선순위 값
    public int Cost;
    public int SkillNum;                                                       //뭐하는스킬인가 0~2 RSP 3 가드 4 도발 5~7 스페셜
    public bool isRunning = true;                                              //이름
    public bool isUseTurn = false;
    public Skill(int Pri, string _Name )
    {
        //생성자
        isUseTurn = true;
        Name = _Name;
        Priority = Pri;
        PassiveList = new Dictionary<string, Active>();
        PassiveCount = new Dictionary<string, float>();
    }
    public void ActiveSkillSet(Active actives)
    {
        TempActive = actives;
    }
    public static CharacterStatus GetOrder(Skill skil)
    {
        return gameManager.ins.UserStatus[skil.Order];
    }
    public CharacterStatus GetOrder()
    {
        return gameManager.ins.UserStatus[Order];
    }
    public static CharacterStatus GetEnemy(Skill skil)
    {
        return gameManager.ins.UserStatus[gameManager.ins.UserStatus[skil.Order].Enemy()];
    }
    public CharacterStatus GetEnemy()
    {
        return gameManager.ins.UserStatus[gameManager.ins.UserStatus[Order].Enemy()];
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
        if (PassiveList.ContainsKey(str))
        {
            PassiveList[str] += pas;
        }
        else
        {
            PassiveList.Add(str, pas);
        }
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
    virtual public bool RunActive()//액티브함수실행
    {
        if (Order != -1)
        {
            TempActive(this);
            return isUseTurn;
        }
        return false;
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