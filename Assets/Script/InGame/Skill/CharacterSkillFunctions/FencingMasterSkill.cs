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
        return EagleEyes;
    }

}
