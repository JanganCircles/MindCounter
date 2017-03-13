using System;
using System.Collections.Generic;
using UnityEngine;

public class StreetFighterSkill : SkillBase
{

    public override void setUp(int index, ref SkillSlot List)
    {
        Debug.Log("스트리트파이터 셋업");
        List.AddPassiveSlot(StFighter_Passive(index));
    }
    public Skill StFighter_Passive(int Order)
    {
        //적이 가드면 50%확률로 치명타
        Skill PerpectCounter = new Skill("PerpectCounter");//초기화
        Debug.Log("스트리트 파이터 스킬 초기화");
        PerpectCounter.SetCharacter(Order);
        PerpectCounter.AddPassive(
           delegate (Skill skil)
           {
               Skill sk = SkillManager.ins.GetSkill(Order, "Critical");
               CharacterStatus EnemyStat = skil.GetEnemy();
               

               Debug.Log("스트리트 파이터 스킬 들어옴");
               if (EnemyStat.Guard )
               {
                   if (UnityEngine.Random.Range(0, 2) == 0)//50%
                   {
                       EnemyStat.Guard = false;
                       sk.PassiveCount["Critical"] = 100;
                   }
               }
               CharacterStatus OrderStat = gameManager.ins.UserStatus[Order];
           }, "Attack");
        PerpectCounter.AddPassive(
           delegate (Skill skil)
           {
               Skill sk = SkillManager.ins.GetSkill(Order, "Critical");
               if (sk.PassiveCount["Critical"] == 100)
               {
                   sk.PassiveCount["Critical"] = ProbabilityData.Critical;
               }
           }, "End");
        return PerpectCounter;
    }

}
