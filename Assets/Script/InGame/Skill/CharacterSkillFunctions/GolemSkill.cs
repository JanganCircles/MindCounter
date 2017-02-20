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
        return MagmaPunch;
    }

}
