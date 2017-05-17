using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Sprite : MonoBehaviour, EFFECT.Effect
{

	private Animator animator;
	public void Awake()
    {
		animator = GetComponent<Animator>();
    }
    public void OnEnable()
    {
	}

    public void Run()
    {

	}
	public bool isLoof{ get; set;}
	public void SetTimer (float t)
	{
		
	}
}