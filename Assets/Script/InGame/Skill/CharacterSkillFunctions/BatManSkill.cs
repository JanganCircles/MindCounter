using System;
using System.Collections.Generic;
using UnityEngine;

public class BatManSkill : SkillBase
{

    public override void setUp(int index, ref SkillSlot List)
    {
        Debug.Log("야구선수 셋업");
        List.AddPassiveSlot(Batting_Passive(index));
    }
    public Skill Batting_Passive(int Order)
    {
        //압박 3회 성공즉시 화력데미지만큼 추가 데미지
        StackSkill ThreeOut = new StackSkill(3,"ThreeOut");//
        SkillSlot OrderSlot;
        ThreeOut.SetCharacter(Order);
        ThreeOut.StackSet(0);
        ThreeOut.AddPassive(
           delegate (Skill skil)
           {
               OrderSlot = gameManager.ins.UserSlot[skil.Order];


               if (OrderSlot.GetPriority() == Priority.ROCK)
               {
                   ThreeOut.StackPlus();
                   if (ThreeOut.TempStack == 3)
                   {
                       Debug.Log("3진아웃");
                       Skill sk = SkillManager.ins.GetSkill(Order, "화력");
                       ThreeOut.StackSet(0);
                       DamageCalculator.ins.AddDamage("Plus", sk.PassiveCount["Damage"], "BatPassive");
                   }
               }
           }
           , "Attack");
        return ThreeOut;
    }

}
