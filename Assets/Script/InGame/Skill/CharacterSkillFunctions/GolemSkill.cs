using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSkill : SkillBase {
    
    public override void setUp(int index,ref SkillSlot List)
    {
        Debug.Log("골렘 셋업");
        List.AddPassiveSlot(Golliath_Passive(index));
    }
    Skill Golliath_Passive(int Order)
    {
        Skill MagmaPunch = new Skill("MagmaPunch");
        MagmaPunch.SetCharacter(Order);
        MagmaPunch.AddPassive(
           delegate (Skill skil)
           {
               int Enemy = gameManager.ins.UserStatus[Order].Enemy();
               if (gameManager.ins.UserStatus[Enemy].Guard) return;
               SkillSlot OrderSlot = gameManager.ins.UserSlot[Order];
               float PowerDamage = OrderSlot.findSkill("압박").PassiveCount["Damage"] / 2f;
               SkillManager.ins.DebuffSkillSet(Enemy, DebuffSkillList.LIST.GOLEMMAGMA, 2, PowerDamage);
           }, "Attack");
        ;
        return MagmaPunch;
    }

}
