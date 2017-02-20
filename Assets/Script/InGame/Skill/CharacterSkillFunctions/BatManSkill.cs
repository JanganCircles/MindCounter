using System;
using System.Collections.Generic;
using UnityEngine;

public class BatManSkill : SkillBase
{

    public override void setUp(int index, ref SkillSlot List)
    {
        Debug.Log("야구선수 셋업");
        List.AddPassiveSlot(Batting_Passive(index));
    }
    public Skill Batting_Passive(int Order)
    {
        //압박 3회 성공즉시 화력데미지만큼 추가 데미지
        StackSkill ThreeOut = new StackSkill(3,"ThreeOut");//
        return ThreeOut;
    }

}
