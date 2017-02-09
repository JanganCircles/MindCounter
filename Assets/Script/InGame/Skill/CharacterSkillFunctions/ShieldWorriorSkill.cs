using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldWorriorSkill : SkillBase {
    
    public override void setUp(int index,ref SkillSlot List)
    {
        Debug.Log("방패기사 셋업");
        List.AddPassiveSlot(Sw_Passive(index));
    }
    public Skill Sw_Passive(int Order)
    {
        //다운무효 , 피격데미지 10%감소
        Skill Indomitable = new Skill("Indomitable");
        Indomitable.SetCharacter(Order);
        CharacterStatus OrderStat;
        Indomitable.PassiveCount.Add("DamageDecrease", 0.9f);
        Indomitable.AddPassive(
           delegate (Skill skil)
           {
               DamageCalculator.ins.AddDamage("Multiple", skil.PassiveCount["DamageDecrease"], "DamageDecrease");
           }, "Hit");

        Indomitable.AddPassive(
           delegate (Skill skil)
           {
               OrderStat = gameManager.ins.UserStatus[skil.Order];
               if (OrderStat.Down)
                   OrderStat.Down = false;
           }
           , "End");
        return Indomitable;
    }

}
