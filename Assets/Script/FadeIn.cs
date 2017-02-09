using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {
    public Image img;
	// Use this for initialization
	void Awake() {
        img = GetComponent<Image>();
        Color c = img.color;
        c.a = 1;
        img.color = c;
        img.enabled = false;
    }
    void Start()
    {
        StartCoroutine(RunFadeOut());
    }

    public IEnumerator RunFadeOut()
    {
        img.enabled = true;
        Color c = img.color;
        c.a = 1;
        img.color = c;
        while (c.a > 0)
        {
            c.a -= Time.deltaTime;
            img.color = c; 
            yield return null;
        }
        c.a = 0;
        img.color = c;
        img.enabled = false;

    }
}
