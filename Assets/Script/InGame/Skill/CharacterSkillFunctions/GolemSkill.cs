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
        MagmaPunch.PassiveCount.Add("ManaDamage", 0f);
        MagmaPunch.AddPassive(
           delegate (Skill skil)
           {
               if (MagmaPunch.PassiveCount.ContainsKey("ManaDamage"))
               {
                   MagmaPunch.PassiveCount["ManaDamage"] = (float)skil.GetOrder().Cost / ((float)skil.GetOrder().MaxCost*2);
               }
           }, "KeyCheck");

        MagmaPunch.AddPassive(
           delegate (Skill skil)
           {
               DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, 1f + MagmaPunch.PassiveCount["ManaDamage"], "ManaPunch");
           }, "Attack");
        ;
        return MagmaPunch;
    }

}
