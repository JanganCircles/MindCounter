using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UINumberSet : MonoBehaviour {

    public static Dictionary<string, float> UIList = null;
    public string Name;
    public Text TargetTxt;
    void Awake()
    {
        if (UIList == null)
        {
            UIList = new Dictionary<string, float>();
        }
        UIList.Add(Name, 0);
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
            TargetTxt.text = ((int)UIList[Name]).ToString();

            yield return new WaitForSeconds(0.1f);
        }
    }
}
