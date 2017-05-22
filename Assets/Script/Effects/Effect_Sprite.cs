using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Sprite : MonoBehaviour, EFFECT.Effect
{

	private Animator animator;
	public float RunTime;
	public void Awake()
    {
        RunningTime = RunTime;
		animator = GetComponent<Animator>();
    }
    public void OnEnable()
    {
	}

    public void Run()
    {
		//??
	}
	public void Init()
	{
		RunningTime = RunTime;
	}
	public float RunningTime{ get; set;}
	public void SetTimer (float t)
	{
		if (t > -1f) {
			{
				StartCoroutine (Timer (t));
			}
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