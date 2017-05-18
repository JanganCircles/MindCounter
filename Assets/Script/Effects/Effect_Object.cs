using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Object : MonoBehaviour, EFFECT.Effect
{
	public float RunTime;
    public void OnEnable()
    {

    }
    public void Start()
    {

    }
    public void Run()
    {

    }
    IEnumerator IERun()
    {
		
        while (true)
        {
            yield return null;
        }
	}
	public void Init()
	{
		RunningTime = RunTime;
	}
	public float RunningTime{ get; set;}
	public void SetTimer (float t)
	{
		if (t > -1) {
			StartCoroutine (Timer (t));
		}
	}
	IEnumerator Timer(float t)
	{
		while(t > 0)
		{
			t -= Time.deltaTime;
			yield return null;
		}
		Destroy(gameObject);
	}
}