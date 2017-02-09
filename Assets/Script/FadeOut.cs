using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {
    public static FadeOut ins;
    bool isFadeOut;
    public Image img;
	// Use this for initialization
	void Awake() {
        ins = this;
        img = GetComponent<Image>();
        img.enabled = false;
    }
    void Start()
    {
    }

    public IEnumerator RunFadeOut()
    {
        img.enabled = true;
        Color c = img.color;
        c.a = 0;
        img.color = c;
        while (c.a < 1)
        {
            c.a += Time.deltaTime;
            img.color = c; 
            yield return null;
        }
    }

}
