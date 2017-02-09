using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UITextSet : MonoBehaviour {

    public static Dictionary<string, string> UIList = null;
    public string Name;
    public bool useName;
    public Text TargetTxt;
    void Awake()
    {
        if (UIList == null)
        {
            UIList = new Dictionary<string, string>();
        }
        if (useName)
        {
            Name = name;
        }

        UIList.Add(Name, "");
    }
    public static void Release()
    {
        if (UIList != null)
        UIList.Clear();
        UIList = null;
    }
    void Start()
    {
        StartCoroutine("NumCheck");
    }

    IEnumerator NumCheck()
    {
        while (true)
        {
            TargetTxt.text = UIList[Name];

            yield return new WaitForSeconds(0.1f);
        }
    }
}
