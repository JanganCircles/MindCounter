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
        //적 방어시 카운터치면 상대방 무력화
        Skill PerpectCounter = new Skill("PerpectCounter");//초기화
        return PerpectCounter;
    }

}
