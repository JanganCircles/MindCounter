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
        StackSkill OverCharge = new StackSkill(2,"OverCharge");
        OverCharge.SetCharacter(Order);
        OverCharge.StackSet(0);
        OverCharge.AddPassive(
           delegate (Skill skil)
           {
               CharacterStatus OrderStat = skil.GetOrder();


               Debug.Log("건틀릿 스킬 들어옴");
               if (OrderStat.Guard)
               {
                   OverCharge.StackSet(1);
               }
           }, "Hit");
        OverCharge.AddPassive(
           delegate (Skill skil)
           {
               CharacterStatus OrderStat = skil.GetOrder();


               Debug.Log("건틀릿 스킬 들어옴");
               if (OverCharge.TempStack == OverCharge.MaxStack)
               {
                   DamageCalculator.ins.AddDamage(DamageCalculator.PLUS_s, DamageData.Strong, "OverCharge");
               }
           }, "Attack");
        OverCharge.AddPassive(
           delegate (Skill skil)
           {

               if (OverCharge.TempStack == OverCharge.MaxStack)
               {
                   OverCharge.TempStack = 0;
               }
               else if (OverCharge.TempStack != 0)
               {
                   OverCharge.StackPlus();
               }
           }, "End");
        return OverCharge;
    }

}
