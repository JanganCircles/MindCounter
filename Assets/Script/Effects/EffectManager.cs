using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class EffectManager : MonoBehaviour
{

    public static EffectManager ins;
    //풀링
    //이벤트사용
    //스크립트에서 해당 -만 바꾸면 돌아가게.
    //~_~
    private Dictionary<string, GameObject> TempRunningObject;
    private Stack<GameObject> AnimSleepEffect;
    private Stack<GameObject> ObjSleepEffect;
    private string TextData;
    public EFFECT.Effect TempSetEvent { get; set; }
    void Awake()
    {
        AnimSleepEffect = new Stack<GameObject>();
        ObjSleepEffect = new Stack<GameObject>();
        TempRunningObject = new Dictionary<string, GameObject>();
        ins = this;
    }
    // Use this for initialization
    void Start()
    {

    }
    public void EventRun(Vector3 Position,string Name,EFFECT.EFFECTLIST EffCode, bool isLoof)
    {
        
        //현재 TempEvent에 값이 들어가있는 상태

    }
    public void EventRun(Vector3 Position, string Name, EFFECT.EFFECTLIST EffCode, float Timer)
    {
        //현재 TempEvent에 값이 들어가있는 상태

    }
    public void EventRun(Vector3 Position, string Name, EFFECT.EFFECTLIST EffCode, int LoofTIme)
    {
        //현재 TempEvent에 값이 들어가있는 상태

    }
    public void EventRun(Vector3 Position, string Name, EFFECT.EFFECTLIST EffCode, Delegate RunFunc)
    {
        //현재 TempEvent에 값이 들어가있는 상태

    }
    public bool Stop(string Name)
    {
        return false;
    }
    public void MakeEffect(EFFECT.EFFECTYPE type)
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
}

//만들어야 하는 스크립트
// -1 애니메이션
// -2 그냥오브젝트->?
namespace EFFECT
{
    public enum EFFECTYPE
    {
        OBJECT, ANIMATION
    }
    public enum EFFECTLIST
    {

    }
    public interface Effect
    {
        float Timer { get; set; }
        bool isLoof { get; set; }
        void Run();
    }
}