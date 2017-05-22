using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageEffect : MonoBehaviour {

    public Image[] img;
    private Vector3 PrevPos;
         
    private void Awake()
    {
        
        img =  GetComponentsInChildren<Image>();
        PrevPos = transform.position;
        for (int i = 0; i < img.Length; i++)
        {
            img[i].color = Color.white;
            img[i].gameObject.SetActive(false);
        }
    }
    // Use this for initialization
	// Use this for initialization
	void OnEnable() {
        StartCoroutine(ActiveClose());
    }
    public void OnDamage()
    {
        for (int i = 0; i < img.Length; i++)
        {
            img[i].color = Color.white;
            img[i].gameObject.SetActive(true);
            StartCoroutine(ActiveClose());
        }
    }
    IEnumerator ActiveClose()
    {
        Color c;
        for (float i = 0; i < 1.5f; i += Time.deltaTime)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 10);
            for (int j = 0; j < img.Length; j++)
            {
                c = img[j].color;
                c.a = 1 - i / 1.5f;
                img[j].color = c;
            }
            yield return null;
        }
        transform.position =  PrevPos;
        for (int i = 0; i < img.Length; i++)
        {
            img[i].gameObject.SetActive(false);
        }
    }
    private void Update()
    {
    }

}
