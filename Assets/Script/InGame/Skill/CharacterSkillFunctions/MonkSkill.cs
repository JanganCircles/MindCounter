using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkSkill : SkillBase {
    
    public override void setUp(int index,ref SkillSlot List)
    {
        Debug.Log("고양이 셋업");
        List.AddPassiveSlot(Monk_Passive(index));
    }
    public Skill Monk_Passive(int Order)
    {
        //압박 성공시키면 다음공격 50%추뎀
        StackSkill FlowForce = new StackSkill(2,"FlowForce");//초기화
        return FlowForce;
    }

}
