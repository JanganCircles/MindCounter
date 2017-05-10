using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class UIProgressBar : MonoBehaviour
{
    public static Dictionary<string, Vector2> ProgressBarList = null;
    public string Name;
    public bool isUseName;
    public bool UseWidth;
    Image img;
    RectTransform rectTr;
    private int sign;
    private float width_x;
    private float Maximum;
    // Use this for initialization
    void Awake()
    {
        if (ProgressBarList == null)
        {
            ProgressBarList = new Dictionary<string, Vector2>();
        }
        if (isUseName)
        {
            Name = name;
        }
        if(!ProgressBarList.ContainsKey(Name))
         ProgressBarList.Add(Name, new Vector2(1,1));
    }
    void Start()
    {
        StartCoroutine("LateStart");
    }
    public static void SetData(string Name, Vector2 vec)
    {
        if (ProgressBarList.ContainsKey(Name))
        {
            ProgressBarList[Name] = vec;
        }
    }
    public static Vector2 GetData(string Name)
    {
        if (ProgressBarList.ContainsKey(Name))
        {
            Debug.Log(Name + ProgressBarList[Name]);
            return ProgressBarList[Name];
        }
        return Vector2.zero;
    }
    public static void Release()
    {
        if (ProgressBarList != null)
        ProgressBarList.Clear();
        ProgressBarList = null;
    }
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        rectTr = GetComponent<RectTransform>();
        Maximum = rectTr.sizeDelta.x;
        StartCoroutine("RunProgressBar");
    }
    IEnumerator RunProgressBar()
    {

        Vector2 wh = rectTr.sizeDelta;
        while (true)
        {
            float Per = ProgressBarList[Name].x/ ProgressBarList[Name].y;
            if (UseWidth)
                wh.x = Maximum * Per;
            else
                wh.y = Maximum * Per;
            rectTr.sizeDelta = wh;
            yield return null;
        }
    }
}