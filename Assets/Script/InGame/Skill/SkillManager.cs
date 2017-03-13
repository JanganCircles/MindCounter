
using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {
    //조건-
    //피격시(Hit) 공격시(Attack) 시작시(Start) 종료시(End) 
    public static SkillManager ins = null;
    public SkillSlot[] Slots;
    
    // Use this for initialization
    void Awake()
    {
        if(ins == null)
        ins = this;
        Slots = new SkillSlot[2];

    }
	void OnEnable () {
        Debug.Log("FUCK");
        Slots[gameManager.CHALLANGER] = gameManager.ins.UserStatus[gameManager.CHALLANGER].GetComponent<SkillSlot>();
        Slots[gameManager.CHAMPION] = gameManager.ins.UserStatus[gameManager.CHAMPION].GetComponent<SkillSlot>();
    }
    // Update is called once per frame
    void Update () {

    }
    public void RunPassives(string str)
    {
        Debug.Log(str + "실행");
        for (int i = 0; i < 2; i++)
        {
            RunPassives(str,i);
        }
    }
    public Skill GetSkill(int index, string name)
    {
        return Slots[index].findSkill(name);
    }
    public void RunPassives(string str,int index)
    {
        if (Slots[index] != null)
            Slots[index].PassivesRun(str);
    }
    public void SetSkill(int Character)
    {

    }
    public bool CharacterSkillSetup(int index)//캐릭터 스킬 초기화
    {
        CharacterStatusSetup.Charas CharacterNumber = GameData.ins.Characters[index];
        SkillSlot SkSlotRef = gameManager.ins.UserSlot[index];
        SkillBase Passives = null;
        switch (CharacterNumber)
        {
            case CharacterStatusSetup.Charas.StreetFighter: Passives = new StreetFighterSkill(); break;
            case CharacterStatusSetup.Charas.ShieldWorrier: Passives = new ShieldWorriorSkill(); break;
            case CharacterStatusSetup.Charas.FencingMaster: Passives = new FencingMasterSkill(); break;
            case CharacterStatusSetup.Charas.BatMan: Passives = new BatManSkill(); break;
            case CharacterStatusSetup.Charas.Golliath: Passives = new GolliathSkill(); break;
            case CharacterStatusSetup.Charas.GauntletsMan:Passives = new GauntletGirlSkill(); break;
            case CharacterStatusSetup.Charas.Golem: Passives = new GolemSkill(); break;
            case CharacterStatusSetup.Charas.Cat: Passives = new CatSkill(); break;
            case CharacterStatusSetup.Charas.Monk: Passives = new MonkSkill(); break;
            case CharacterStatusSetup.Charas.Assassin:
                break;
            case CharacterStatusSetup.Charas.dibidibidip:
                break;
            default: return false;
        }
        if (Passives != null)
            Passives.setUp(index, ref SkSlotRef);
        else
            return false;
        return true;
    }

    

    public void DebuffSkillSet(int Enemy,DebuffSkillList.LIST Debuff,params float[] Param)//디버프 세팅.
    {
        SkillSlot SkSlotRef = gameManager.ins.UserSlot[Enemy];
        DebuffBase Debuffs = null;
        switch (Debuff)
        {
            case DebuffSkillList.LIST.Blooding:
                Debuffs = new DebuffSkill_GolemMagma();
                break;
            case DebuffSkillList.LIST.NONE:
                break;
            default:
                break;
        }
        if (Debuffs != null)
            Debuffs.setUp(Enemy, ref SkSlotRef, Param);
    }

}
public abstract class SkillBase
{
    public abstract void setUp(int index,ref SkillSlot List);
}
public abstract class DebuffBase
{
    public int Number;
    private static int NameNum;
    public abstract void setUp(int index, ref SkillSlot List, params float[] Parameter);    //사용자 설정 값(개수 충족못할경우 걍 디폴트로)
    public DebuffBase()
    {
        Debug.Log("호출함?");
        Number = NameNum;
        NameNum++;
    }

}