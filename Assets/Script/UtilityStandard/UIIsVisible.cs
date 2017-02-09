using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIIsVisible : MonoBehaviour {

    public static Dictionary<string, bool> UIList = null;
    public string Name;
    public GameObject Target;
    void Awake()
    {
        if (UIList == null)
        {
            UIList = new Dictionary<string, bool>();
        }
        UIList.Add(Name, false);
    }
    public static void Release()
    {
        if(UIList !=null)
        UIList.Clear();
        UIList = null;
    }
    void Start()
    {
        StartCoroutine("Visible");
    }

    IEnumerator Visible()
    {
        Debug.Log("zz");
        while (true)
        {
            Target.SetActive(UIList[Name]);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
