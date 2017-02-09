using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolliathSkill : SkillBase {
    
    public override void setUp(int index,ref SkillSlot List)
    {
        Debug.Log("헐크 셋업");
        List.AddPassiveSlot(Golliath_Passive(index));
    }
    Skill Golliath_Passive(int Order)
    {
        //증뎀10% , 피격데미지 10%감소
        Skill SuperMuscle = new Skill("SuperMuscle");
        SuperMuscle.SetCharacter(Order);
        SuperMuscle.PassiveCount.Add("DamageDecrease", 0.9f);
        SuperMuscle.PassiveCount.Add("DamageIncrease", 1.1f);
        SuperMuscle.AddPassive(
           delegate (Skill skil)
           {
               DamageCalculator.ins.AddDamage("Multiple", skil.PassiveCount["DamageDecrease"], "DamageDecrease");
           }, "Hit");

        SuperMuscle.AddPassive(
           delegate (Skill skil)
           {
               DamageCalculator.ins.AddDamage("Multiple", skil.PassiveCount["DamageIncrease"], "DamageIncrease");
           }, "Attack");
        ;
        return SuperMuscle;
    }

}
