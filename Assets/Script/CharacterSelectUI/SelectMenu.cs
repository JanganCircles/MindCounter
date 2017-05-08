using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectMenu : MonoBehaviour
{
    //http://m.blog.naver.com/dieofflee/220071406618   스파인
    public bool isRun = false;            //동작할거냐(트리거)
    private bool IsInnerTrigger = false;  
    public RectTransform CursorTr;        //캔버스 내부의 커서 트랜스폼.

    //0 = 1피, 1 = 2피
    public RectTransform[] IconTr;// 아이콘 트랜스폼
    public int XLength;
    public int YLength;
    public Vector2 Cursor;                //커서의 논리적 위치(인덱스)
    private bool MoveOK = false;                  //false = 이동끝남 , true = 이동중
    private bool CharacterSelectLockOn = false;                  //false = 선택안함 , true = 선택중
    private bool PlayerSelect = false;            //현재 캐릭터 선택 되어있는가
    public KeyCode[] KeysData = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,KeyCode.G }; //위왼아래오른,선택,취소
    
    // Use this for initialization

    private void Reset()
    {
        KeysData = new KeyCode[] {KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.Space,KeyCode.Escape};
    }
    // Use this for initialization
    void Awake()
    {
        StartCoroutine("RunSelecter");
    }
    void CursorSetting(ref Vector2 vec, int index)
    {
        switch (index)
        {
            case 0: vec.y--; break;
            case 2: vec.y++; break;
            case 1: vec.x--; break;
            case 3: vec.x++; break;
        }
    }
    public bool isSelect(out Vector2 Index)
    {
        Index = Cursor;
        return PlayerSelect;
    }
    IEnumerator RunSelecter()
    {
        while (true)
        {
            if (isRun == IsInnerTrigger)
                yield return null;
            else
            {
                int a = 0;
                if (isRun)
                {
                    StartCoroutine("CharacterSelect");
                }
                else
                {
                    StopCoroutine("CancelChecker");
                    StopCoroutine("CharacterSelect");
                }
                IsInnerTrigger = isRun;
               // isRun ? StartCoroutine("CharacterSelect") : StopCoroutine("CharacterSelect"); ;
            }
        }
    }
    IEnumerator CharacterSelect()
    {
        CharacterSelectLockOn = false;
        PlayerSelect = false;
        while (!CharacterSelectLockOn)//모든 입력 확인
        {
            if (MoveOK || CharacterSelectLockOn)
            {
                //아무것도 들어오면 안됩니다.아직은
            }
            else if (Input.GetKeyDown(KeysData[4]))//입력키 눌렀다.
            {
                CharacterSelectLockOn = true;//선택
            }


            else
            {
                for (int j = 0; j < 4; j++)
                    if (Input.GetKeyDown(KeysData[j]))//키입력이 들어왔다.
                    {
                        CursorSetting(ref Cursor, j);   //커서이동.
                        MoveOK = true;//이동할거임
                    }
            }
            if (MoveOK)//이동함?
            {
                if (Cursor.x < 0) Cursor.x = XLength + Cursor.x;
                else Cursor.x = Cursor.x % XLength;

                if (Cursor.y < 0) Cursor.y = YLength + Cursor.y;
                else Cursor.y = Cursor.y % YLength;

                StartCoroutine("MoveTarget");//타겟으로 이동
            }
            else if (CharacterSelectLockOn)//선택함?
            {
                if (!PlayerSelect)//플레이어가 선택이 되있지 않으면
                {
                    PlayerSelect = true;//선택
                }
            }
            yield return null;
        }
        StartCoroutine("CancelChecker");
    }
    IEnumerator CancelChecker()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeysData[5]))
            {
                CharacterSelectLockOn = false;
                PlayerSelect = false;

                StartCoroutine("CharacterSelect");
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator MoveTarget()//커서변경하는 함수.
    {
        Vector3 Prev = CursorTr.localPosition;
        Vector3 v = new Vector3();
        int CursorIndex = (int)Cursor.x + (int)(Cursor.y * XLength);
        v = IconTr[CursorIndex].localPosition;
        Debug.Log(v);
        // v.x = Cursor[index].x * Length;
        // v.y = Cursor[index].y * Length;
        for (int i = 0; i <= 5; i++)
        {
            CursorTr.localPosition = Vector3.Lerp(Prev, v, i / 5f);
            yield return null;
        }
        MoveOK = false;
    }
}
