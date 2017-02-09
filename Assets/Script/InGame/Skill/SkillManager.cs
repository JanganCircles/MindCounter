
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
        for (int i = 0; i < 2; i++)
        {
            if(Slots[i] != null)
            Slots[i].PassivesRun(str);
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
        CharacterData.Charas CharacterNumber = GameData.ins.Characters[index];
        SkillSlot SkSlotRef = gameManager.ins.UserSlot[index];
        SkillBase Passives = null;
        switch (CharacterNumber)
        {
            case CharacterData.Charas.StreetFighter: Passives = new StreetFighterSkill(); break;
            case CharacterData.Charas.ShieldWorrier: Passives = new ShieldWorriorSkill(); break;
            case CharacterData.Charas.FencingMaster: Passives = new FencingMasterSkill(); break;
            case CharacterData.Charas.BatMan: Passives = new BatManSkill(); break;
            case CharacterData.Charas.Golliath: Passives = new GolliathSkill(); break;
            case CharacterData.Charas.GauntletsMan:Passives = new GauntletGirlSkill(); break;
            case CharacterData.Charas.Golem: Passives = new GolemSkill(); break;
            case CharacterData.Charas.Cat: Passives = new CatSkill(); break;
            case CharacterData.Charas.Monk: Passives = new MonkSkill(); break;
            case CharacterData.Charas.Assassin:
                break;
            case CharacterData.Charas.dibidibidip:
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
            case DebuffSkillList.LIST.GOLEMMAGMA:
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