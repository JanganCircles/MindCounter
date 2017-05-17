using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

public class EffectManager : MonoBehaviour
{
	public GameObject[] Effects;
	private Dictionary<string,GameObject> dicEffects;
	public Canvas UICanvas;
    public static EffectManager ins;
    //풀링안함
    //이벤트사용
    //스크립트에서 해당 -만 바꾸면 돌아가게.
    //~_~


    void Awake()
    {
		ins = this;
		dicEffects = new Dictionary<string, GameObject> ();
		for(int i = 0 ; i < Effects.Length;i++)
		{
			dicEffects.Add (Effects [i].name, Effects [i]);
		}
	}
    // Use this for initialization
	public void EffectRun(Vector3 Position,Vector3 Scale, string Name,bool isLoof,bool OnUI)
	{
		GameObject LoadObj = Instantiate(dicEffects[Name],Position,Quaternion.identity) as GameObject;
		Debug.Log (LoadObj);
		if (OnUI)
			LoadObj.transform.parent = UICanvas.transform;
		else
			LoadObj.transform.parent = transform;
		LoadObj.SendMessage ("SetisLoof", isLoof);
		LoadObj.SendMessage ("Run");
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
		void SetTimer(float t);
		void Run();

    }
}