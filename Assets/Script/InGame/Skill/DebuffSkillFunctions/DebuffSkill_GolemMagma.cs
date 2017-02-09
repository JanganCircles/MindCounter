using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffSkill_GolemMagma : DebuffBase {

//    public DebuffSkill_GolemMagma() : base(){}
    public override void setUp(int index, ref SkillSlot List, params float[] Parameter)
    {        
        List.DebuffList.Add(Magma_Debuff(index,Parameter));
    }

    Skill Magma_Debuff(int Order, float[] Parameter)
    {
        int[] Value = { 2, 50 };//2턴간 50의 데미지(기본값).
        float LoofLength = Value.Length > Parameter.Length ? Parameter.Length : Value.Length;
        for (int i = 0; i < LoofLength; i++)
        {
            Value[i] = (int)Parameter[i];
        }
        StackSkill Magma = new StackSkill(Value[0], base.Number.ToString());
        Magma.SetCharacter(Order);
        Magma.AddPassive(
           delegate (Skill skil)
           {
               Debug.Log("마그마 디버프 들어옴?");
               if (!Magma.StackMinus())
               {
                   gameManager.ins.UserSlot[Order].DebuffList.Remove(skil);
                   return;
               }
               CharacterStatus OrderStat = gameManager.ins.UserStatus[Order];
               OrderStat.HpDown(Value[1]);
           }, "KeyCheck");
        return Magma;
    }
}
