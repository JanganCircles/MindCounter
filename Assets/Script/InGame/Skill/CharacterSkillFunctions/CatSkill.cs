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
        CharacterStatus OrderStat;
        EternalLife.SetCharacter(Order);
        EternalLife.AddPassive(
           delegate (Skill skil)
           {
               OrderStat = gameManager.ins.UserStatus[skil.Order];//
               if (gameManager.ins.Combo == 4 && gameManager.ins.ComboContinues == OrderStat.Controller)
               {
                   OrderStat.Life++;
                   if (OrderStat.Life > 9) OrderStat.Life = 9;
               }
           }
           , "Attack");
        return EternalLife;
    }

}
