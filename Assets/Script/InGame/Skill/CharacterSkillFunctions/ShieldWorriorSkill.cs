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
        return Indomitable;
    }

}
