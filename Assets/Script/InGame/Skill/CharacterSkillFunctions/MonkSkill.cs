using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkSkill : SkillBase {
    
    public override void setUp(int index,ref SkillSlot List)
    {
        Debug.Log("몽크 셋업");
        List.AddPassiveSlot(Monk_Passive(index));
    }
    public Skill Monk_Passive(int Order)
    {
        //압박 성공시키면 다음공격 50%추뎀
        Skill FlowForce = new Skill("FlowForce");//초기화
        FlowForce.SetCharacter(Order);
        FlowForce.AddPassive(
           delegate (Skill skil)
           {

               Debug.Log("몽크스킬실행");
               SkillSlot OrderSlot = gameManager.ins.UserSlot[Order];
               if (OrderSlot.GetPriority() == Priority.SCISSOR)
               {
                   skil.GetOrder().CostPlus(10);
               
               }
           }
           , "Decision");
        return FlowForce;
    }

}
