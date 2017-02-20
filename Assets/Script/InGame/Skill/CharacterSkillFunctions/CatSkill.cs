using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSkill : SkillBase {
    
    public override void setUp(int index,ref SkillSlot List)
    {
        Debug.Log("고양이 셋업");
        List.AddPassiveSlot(Cat_Passive(index));
    }
    public Skill Cat_Passive(int Order)
    {
        //5콤보시 1라이프
        Skill EternalLife = new Skill("EternalLife");//초기화
        return EternalLife;
    }

}
