using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSkill : SkillBase {
    
    public override void setUp(int index,ref SkillSlot List)
    {
        Debug.Log("고양이 셋업");
        List.AddPassiveSlot(Cat_Passive(index));
    }
    public Skill Cat_Passive(int Order)
    {
        //5콤보시 1라이프
        Skill Blooding = new Skill("Blooding");//초기화
        Blooding.SetCharacter(Order);
        Blooding.AddPassive(
           delegate (Skill skil)
           {
               int Enemy = gameManager.ins.UserStatus[Order].Enemy();
               if (gameManager.ins.UserStatus[Enemy].Guard) return;
               SkillSlot OrderSlot = gameManager.ins.UserSlot[Order];
               float PowerDamage = DamageData.Cat_Blooding;
               SkillManager.ins.DebuffSkillSet(Enemy, DebuffSkillList.LIST.Blooding, 3, PowerDamage);
           }, "Attack");
        ;
        return Blooding;
    }

}
