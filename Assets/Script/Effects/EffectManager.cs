using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class EffectManager : MonoBehaviour
{

    public Effect_Object[] ObjEffects;
    public Effect_Sprite[] SprEffects;
    private Dictionary<string, EFFECT.Effect> dicEffects;
    private Dictionary<string, GameObject> dicGameObject;
    public Canvas UICanvas;
    public static EffectManager ins;
    //풀링안함
    //이벤트사용
    //스크립트에서 해당 -만 바꾸면 돌아가게.
    //~_~


    void Awake()
    {
        ins = this;
        dicEffects = new Dictionary<string, EFFECT.Effect>();
        dicGameObject = new Dictionary<string, GameObject>();
        for (int i = 0; i < ObjEffects.Length; i++)
        {
            ObjEffects[i].RunningTime = ObjEffects[i].GetComponent<Effect_Object>().RunTime;

            dicEffects.Add(ObjEffects[i].name, ObjEffects[i]);
            dicGameObject.Add(ObjEffects[i].name, ObjEffects[i].gameObject);
        }
        for (int i = 0; i < SprEffects.Length; i++)
        {
            SprEffects[i].RunningTime = SprEffects[i].GetComponent<Effect_Sprite>().RunTime;

            dicEffects.Add(SprEffects[i].name, SprEffects[i]);
            dicGameObject.Add(SprEffects[i].name, SprEffects[i].gameObject);
        }
    }
    // Use this for initialization
    public void EffectRun(Transform Parent, Vector3 Scale, string Name, bool OnUI)
    {
        GameObject LoadObj = LoadObject(Name, Parent.position, OnUI);
        LoadObj.transform.parent = Parent;
        setUpEffect(LoadObj, Scale, dicEffects[Name].RunningTime);
    }

    public void EffectRun(Vector3 Position, Vector3 Scale, string Name, bool OnUI)
    {
        GameObject LoadObj = LoadObject(Name, Position, OnUI);
       setUpEffect(LoadObj, Scale, dicEffects[Name].RunningTime);
    }
    public void EffectRun(Vector3 Position, Vector3 Scale, string Name, float Timer, bool OnUI)
    {
        GameObject LoadObj = LoadObject(Name, Position, OnUI);        
        setUpEffect(LoadObj, Scale, Timer);
    }
    public GameObject LoadObject(string Name,Vector3 Pos,bool onui)
    {
        GameObject LoadObj = Instantiate(dicGameObject[Name], Pos, Quaternion.identity) as GameObject;
        if (onui)
            LoadObj.transform.parent = UICanvas.transform;
        else
            LoadObj.transform.parent = transform;
        return LoadObj;
    }
    public void setUpEffect(GameObject gm,Vector3 Scale,float Timer)
    {
        gm.transform.localScale = Scale;
        Debug.Log(gm.transform.localScale);
        gm.SendMessage("SetTimer", Timer);
        gm.SendMessage("Run");
    }

    public bool Stop(string Name)
    {
        return false;
    }
    // Update is called once per frame
    void Update()
    {
	}
	string EFFECTPath(EFFECT.EFFECTLIST list)
	{
		string str =  "Prefab/Effect/";
		switch (list) {
		case EFFECT.EFFECTLIST.HPBar:
			{
				str += "HpBar";
			}
			break;
		case EFFECT.EFFECTLIST.NONE:
			{
				//아무것도 업읍니다.
			}
			break;
		}
		return str;
	}
}

//만들어야 하는 스크립트
// -1 애니메이션
// -2 파티클
namespace EFFECT
{
	public delegate void StopEffect();

    public enum EFFECTYPE
    {
        OBJECT, ANIMATION
    }
    public enum EFFECTLIST
    {
		LeftTimingBar,RightTimingBar,
		HPBar,
		NONE
    }

    public interface Effect
	{
		void Init();
		void SetTimer(float t);
		float RunningTime{get;set;}
		void Run();

    }
}