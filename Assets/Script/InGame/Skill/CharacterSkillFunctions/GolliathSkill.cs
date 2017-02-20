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
        return SuperMuscle;
    }

}
