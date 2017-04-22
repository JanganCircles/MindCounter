using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Object : MonoBehaviour, EFFECT.Effect
{
    public void OnEnable()
    {

    }
    public void Start()
    {

    }
    public float Timer { get; set; }
    public bool isLoof { get; set; }
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
}