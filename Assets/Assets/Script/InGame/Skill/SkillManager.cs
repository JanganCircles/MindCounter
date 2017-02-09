
using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {
    //조건-
    //피격시(Hit) 공격시(Attack) 시작시(Start) 종료시(End) 
    public static SkillManager ins = null;
    public SkillSlot[] Slots;
    
    // Use this for initialization
    void Awake()
    {
        if(ins == null)
        ins = this;
        Slots = new SkillSlot[2];

    }
	void OnEnable () {
        Debug.Log("FUCK");
        Slots[gameManager.CHALLANGER] = gameManager.ins.UserStatus[gameManager.CHALLANGER].GetComponent<SkillSlot>();
        Slots[gameManager.CHAMPION] = gameManager.ins.UserStatus[gameManager.CHAMPION].GetComponent<SkillSlot>();
    }
    // Update is called once per frame
    void Update () {

    }
    public void RunPassives(string str)
    {
        for (int i = 0; i < 2; i++)
        {
            if(Slots[i] != null)
            Slots[i].PassivesRun(str);
        }
    }
    public void RunPassives(string str,int index)
    {
        if (Slots[index] != null)
            Slots[index].PassivesRun(str);
    }
}
