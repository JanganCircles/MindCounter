using UnityEngine;
using System.Collections;

public class DebuffManager : MonoBehaviour {
    public static DebuffManager ins;
    public bool NextDown;
    public bool NextDefence;
    public bool NextDisable;
    // Use this for initialization
    void Awake()
    {
        ins = this;
    }
    public static void Cleaner(int Player)
    {
        CharacterStatus status = gameManager.ins.UserStatus[Player];
        status.Down = false;
        status.Defence = false;
        status.Disable = false;

    }
    public void SetDebuff(int Loser)
    {
        CharacterStatus status = gameManager.ins.UserStatus[Loser];
        status.Down = NextDown;
        status.Defence = NextDefence;
        status.Disable= NextDisable;
        NextDisable = false;
    }
    public void Clean()
    {
        NextDefence = false;
        NextDown = false;
        NextDisable = false;
    }
    public void OnDefence()
    {
        NextDown = false;
        NextDefence = true;
    }
    public void OnDown()
    {
        NextDown = true;
        NextDefence = false;
    }
    public void OnDisable()
    {
        NextDisable = true;
    }
}
