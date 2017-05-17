using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Sprite : MonoBehaviour, EFFECT.Effect
{

	private Animator animator;
	public AnimationClip clip;
	public void Awake()
    {
		animator = GetComponent<Animator>();
    }
    public void OnEnable()
    {
		clip = animator.GetCurrentAnimatorClipInfo (0) [0].clip;
	}

    public void Run()
    {

	}
	public bool isLoof{ get; set;}
	public void SetisLoof(bool value)
	{
		isLoof = value;
	}
}