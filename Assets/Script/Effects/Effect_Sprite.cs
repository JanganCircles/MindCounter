using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Sprite : MonoBehaviour, EFFECT.Effect
{
    private Animation Anim;
    public void Awake()
    {
        Anim = GetComponent<Animation>();
    }
    public void OnEnable()
    {
        Anim.Play();
        Anim.Stop();
    }
    public float Timer { get; set; }
    public bool isLoof { get; set; }
    public void Run()
    {

    }
}