using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectState : MonoBehaviour {
    public PRIORITY TempPriorty = 0;
    public int abs;
    public enum PRIORITY : int
    {
        ZERO,
        RCP,
        TOP
    };
}/*
    public enum PRIORITY : int
    {
        NONE,
        ONLYLOSE,
        GUARD,
        ROCK,             //압박
        PAPER,            //화력
        SCISSOR,          //연속
        GUARDLOSE,
        GUARDWIN,
        ONLYWIN

    };
  //  public PRIORITY TempState;
    public int TempState;
    public bool Check = false;
    public const float MaxTime = 5.0f;
    public KeyData Keys;
    public bool Select = false;
    public Dictionary<KeyCode, PRIORITY> KeyToState;
    public static string[] StateToString = { "None", "무조건패","압박","화력","연속",
                                                    "가드하위","가드","가드상위","무조건승"};
    public int[] CounterCard = { 3, 3, 3 };
    public int[] MaxCard = { 3, 3, 3 };
    void Reset()
    {
        int[] intArr = { 3,3,3};
        MaxCard = intArr;
        CounterCard = MaxCard;
    }
    // Use this for initialization
    void Start () {
        KeyToState = new Dictionary<KeyCode, PRIORITY>();
        KeyToState.Add(Keys[KeyData.Rock], PRIORITY.ROCK);
        KeyToState.Add(Keys[KeyData.Scissors], PRIORITY.SCISSORS);
        KeyToState.Add(Keys[KeyData.Paper], PRIORITY.PAPER);
        KeyToState.Add(Keys[KeyData.Guard], PRIORITY.GUARD);
        KeyToState.Add(Keys[KeyData.Provoke], PRIORITY.NONE);
        KeyToState.Add(Keys[KeyData.Sp1], PRIORITY.NONE);
        KeyToState.Add(Keys[KeyData.Sp2], PRIORITY.NONE);
        KeyToState.Add(Keys[KeyData.Sp3], PRIORITY.NONE);
    }
	
    bool KeyCheck(KeyCode Key)
    {
        return Input.GetKeyDown(Key);
    }
    public void checkState(bool Disable)
    {
        if (Disable)
        {
            Select = true;
            TempState = PRIORITY.NONE;
        }
        else
        {
            Select = false;
            StartCoroutine("iSelectState");
        }
    }
    public bool DownCount(int Index)
    {
        if (Index > KeyData.Guard)
            return true;
        else
        {
            if (Index == KeyData.Guard)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (CounterCard[i] != MaxCard[i])
                        CounterCard[i]++;
                }
                return true;
            }
            else if (CounterCard[Index] == 0)
            {
                return false;
            }
            else
            {
                CounterCard[Index]--;
                return true;
            }
        }
    }
    IEnumerator iSelectState()
    {
        TempState = (int)PRIORITY.NONE;
        float Timer = 0f;
        while (MaxTime > Timer)
        {
            Timer += Time.unscaledDeltaTime;
            for (int i = 0; i < Keys.Length; i++)
            {
                if (KeyCheck(Keys[i]))
                {
                    if (!DownCount(i))
                        continue;
                    TempState = KeyToState[Keys[i]];
                    TempKey = i;
                    Select = true;
                    yield break;
                }
            }
            yield return null;
        }
        Select = true;
        TempState = PRIORITY.NONE;
    }
}
*/
