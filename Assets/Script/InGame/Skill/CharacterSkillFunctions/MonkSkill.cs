using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkSkill : SkillBase {
    
    public override void setUp(int index,ref SkillSlot List)
    {
        Debug.Log("고양이 셋업");
        List.AddPassiveSlot(Monk_Passive(index));
    }
    public Skill Monk_Passive(int Order)
    {
        //압박 성공시키면 다음공격 50%추뎀
        StackSkill FlowForce = new StackSkill(2,"FlowForce");//초기화
        CharacterStatus OrderStat;
        FlowForce.SetCharacter(Order);
        FlowForce.StackSet(0);
        FlowForce.AddPassive(
           delegate (Skill skil)
           {
               SkillSlot OrderSlot = gameManager.ins.UserSlot[Order];
               OrderStat = gameManager.ins.UserStatus[Order];//
               if (FlowForce.TempStack != 0)
               {
                   DamageCalculator.ins.AddDamage("Multiple", 1.5f, "FlowForce");
               }
               if (OrderSlot.GetPriority() == Priority.ROCK)
               {
                   FlowForce.StackSet(2);
               }
           }
           , "Attack");
        FlowForce.AddPassive(
           delegate (Skill skil)
           {
               FlowForce.StackMinus();
           }
           , "End");
        return FlowForce;
    }

}
