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
        StackSkill EagleEyes = new StackSkill(3,"EagleEyes");
        SkillSlot OrderSlot;
        EagleEyes.SetCharacter(Order);
        EagleEyes.StackSet(0);
        EagleEyes.AddPassive(
           delegate (Skill skil)
           {
               CharacterStatus OrderStat = skil.GetOrder();


               Debug.Log("펜싱맨 스킬 들어옴");
               if (!OrderStat.Guard)
               {
                   EagleEyes.StackSet(0);
               }
           }, "Hit");
        EagleEyes.AddPassive(
           delegate (Skill skil)
           {
               Skill sk = SkillManager.ins.GetSkill(Order, "Critical");
               CharacterStatus EnemyStat = skil.GetEnemy();


               Debug.Log("펜싱맨 스킬 들어옴");
               if (!EnemyStat.Guard)
               {
                   EagleEyes.StackPlus();
                   if (EagleEyes.TempStack == 2)
                   {
                       Debug.Log("펜싱선수 패시브 발동");
                       sk.PassiveCount["Critical"] = 100;
                   }
               }
           }, "Attack");
        EagleEyes.AddPassive(
           delegate (Skill skil)
           {
               if (EagleEyes.TempStack == 2)
               {
                   EagleEyes.StackPlus();
               }
           }, "Start");
        EagleEyes.AddPassive(
           delegate (Skill skil)
           {
               Skill sk = SkillManager.ins.GetSkill(Order, "Critical");
               if (EagleEyes.TempStack == EagleEyes.MaxStack)
               {
                   Debug.Log("펜싱선수 패시브 종료");
                   sk.PassiveCount["Critical"] = ProbabilityData.Critical;
                   EagleEyes.StackSet(0);
               }
           }, "End");
        return EagleEyes;
    }

}
