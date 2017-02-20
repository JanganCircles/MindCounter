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
        return OverCharge;
    }

}
