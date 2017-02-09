using System;
using System.Collections.Generic;
using UnityEngine;

public class FencingMasterSkill : SkillBase
{

    public override void setUp(int index, ref SkillSlot List)
    {
        Debug.Log("펜싱맨 셋업");
        List.AddPassiveSlot(Fencing_Passive(index));
    }
    public Skill Fencing_Passive(int Order)
    {
        //연속 3회성공시 다음공격 치명타
        StackSkill EagleEyes = new StackSkill(4,"EagleEyes");
        SkillSlot OrderSlot;
        EagleEyes.SetCharacter(Order);
        EagleEyes.StackSet(0);
        EagleEyes.AddPassive(
           delegate (Skill skil)
           {
               OrderSlot = gameManager.ins.UserSlot[skil.Order];
               Skill sk = SkillManager.ins.GetSkill(Order, "Critical");
               
               if (EagleEyes.TempStack >= 3)
               {
                   EagleEyes.StackSet(0);
                   sk.PassiveCount["Critical"] = 10;
               }

               if (OrderSlot.GetPriority() == Priority.PAPER)
               {
                   EagleEyes.StackPlus();
                   if (EagleEyes.TempStack == 3)
                   {
                       Debug.Log("펜싱선수 패시브 발동");
                       sk.PassiveCount["Critical"] = 100;
                   }
               }
           }
           , "Attack");
        return EagleEyes;
    }

}
