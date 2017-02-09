using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletGirlSkill : SkillBase {
    
    public override void setUp(int index,ref SkillSlot List)
    {
        Debug.Log("바이 셋업");
        List.AddPassiveSlot(Gauntlet_Passive(index));
    }
    public Skill Gauntlet_Passive(int Order)
    {
        //가드시 화력한칸소모해서 다음스킬 데미지 화력만큼 증가
        StackSkill OverCharge = new StackSkill(1,"OverCharge");
        OverCharge.SetCharacter(Order);
        CharacterStatus OrderStat;
        OverCharge.StackSet(0);
        OverCharge.AddPassive(
           delegate (Skill skil)
           {
               OrderStat = gameManager.ins.UserStatus[Order];
               if (OrderStat.Guard && OrderStat.RSPTempCount[CharacterStatus.SCISSOR] > 1)
               {
                   //  if (OverCharge.TempStack == 0)
                   OrderStat.RSPTempCount[CharacterStatus.SCISSOR]--;
                   OverCharge.StackPlus();
               }
               else
               {
                   OverCharge.StackMinus();
               }
           }, "End");

        OverCharge.AddPassive(
           delegate (Skill skil)
           {
               if (OverCharge.TempStack == OverCharge.MaxStack)
               {
                   SkillSlot OrderSlot = gameManager.ins.UserSlot[Order];
                   float PowerDamage = OrderSlot.findSkill("화력").PassiveCount["Damage"];
                   DamageCalculator.ins.AddDamage("Plus", PowerDamage, "OverCharge");
               }
           }, "Attack");
        ;
        return OverCharge;
    }

}
