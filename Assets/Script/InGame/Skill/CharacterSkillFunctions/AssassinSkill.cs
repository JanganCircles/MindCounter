using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinSkill : SkillBase{

    public override void setUp(int index, ref SkillSlot List)
    {
        Debug.Log("앰살자 셋업");
        List.AddPassiveSlot(Assassin_Passive(index));
    }
    public Skill Assassin_Passive(int Order)
    {
        //5콤보시 1라이프
        Skill BigCounter = new Skill("BigCounter");//초기화
        BigCounter.SetCharacter(Order);
        BigCounter.AddPassive(
           delegate (Skill skil)
           {
               int Enemy = gameManager.ins.UserStatus[Order].Enemy();
               SkillSlot OrderSlot = gameManager.ins.UserSlot[Order];
               if (gameManager.ins.UserSlot[Enemy].GetPriority() == Priority.PAPER)
               {
                   DamageCalculator.ins.AddDamage(DamageCalculator.PLUS_s, DamageData.Strong - DamageData.Weak, "SuperWeak");
               }
           }, "Attack");
        ;
        return BigCounter;
    }

}
