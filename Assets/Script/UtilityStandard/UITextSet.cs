using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UITextSet : MonoBehaviour {

    //어디서든 접근가능한 UIList
    public static Dictionary<string, string> UIList = null;
    public string Name;//해당 UI의 이름
    public bool useName; //오브젝트의 이름으로 사용할것인가.
    public Text TargetTxt = null; // 타깃 Text
    void Awake()
    {
        if (UIList == null)
        {
            //처음 들어왔을때.
            UIList = new Dictionary<string, string>();//UIList 생성            
        }
        if (useName)//오브젝트의 이름으로 사용한다하면
        {
            Name = name;//넣어줌
        }
        if (TargetTxt == null) // 텍스트가 Null이면
        {
            TargetTxt = GetComponent<Text>(); // 넣어준다.
        }
        if (!UIList.ContainsKey(name)) //씬 재 로드시 동일한 키를 Add하는것을 방지
            UIList.Add(Name, "");
    }
    public static void Release()
    {
        if (UIList != null)
        UIList.Clear();//깨끗하게
        UIList = null;//초기화
    }
    void Start()
    {
        StartCoroutine("TextCheck");//텍스트 값 변경해주는 코루틴을 실행.
    }
    
    IEnumerator TextCheck()
    {
        while (true)
        {
            TargetTxt.text = UIList[Name];//외부에서 값이 바뀔때 바로 적용됨.

            yield return null;//프레임마다 갱신
        }
    }
    /* 사용방법
     * UITextSet["이름"] = 내용; < 처럼 넣어주면
     * 해당 텍스트 프리팹에서 알아서 값을 넣어줌.
     * 
     */

}
