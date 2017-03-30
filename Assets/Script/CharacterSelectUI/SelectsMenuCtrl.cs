using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectsMenuCtrl : MonoBehaviour {
    
    
    public Sprite[] SpriteImg;              //캐릭터 얼굴들
    public RectTransform[] CursorTr;        //캔버스 내부의 커서 트랜스폼.

    //0 = 1피, 1 = 2피
    public Vector2[] Cursor;                //커서의 논리적 위치(인덱스)
    public Image[] Images;                  //커서에 대응되는 현재 캐릭터 이미지
    private bool[] MoveOK;                  //false = 이동끝남 , true = 이동중
    private bool[] LockOn;                  //false = 선택안함 , true = 선택중
    private bool[] PlayerSelect;            //현재 캐릭터 선택 되어있는가
    private KeyCode[] KeysData = {KeyCode.W,KeyCode.A,KeyCode.S,KeyCode.D,KeyCode.F,                                       //1피 컨트롤
                                  KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.Keypad1}; //2피 컨트롤

    private bool AllOK;                     //두 캐릭터 전부 준비 되었는가.

    // Use this for initialization
    void Start ()
    {
        PlayerSelect = new bool[2];
        MoveOK = new bool[2];
        LockOn = new bool[2];
        AllOK = false;
        for (int i = 0; i < 2; i++)
        {
            PlayerSelect[i] = false;
            LockOn[i] = false;
            MoveOK[i] = false;
        }
    }
    void CursorSetting(ref Vector2 vec,int index)
    {
        switch (index)
        {
            case 0: vec.y--; break;
            case 2: vec.y++; break;
            case 1:vec.x--; break;
            case 3: vec.x++; break;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (AllOK)//둘다 준비 되면 메인씬으로 변경
        {
            return;
        }
        //키입력받는곳
        
        for (int i = 0; i < 2; i++)//빕빕ㅂ빕빕
        {

            if (MoveOK[i] || LockOn[i])
            {
                //아무것도 들어오면 안됩니다.
            }
            else if (Input.GetKeyDown(KeysData[5 * i + 4]))
            {
                LockOn[i] = true;//선택
            }
            else
            {
                for (int j = 0; j < 4; j++)
                    if (Input.GetKeyDown(KeysData[j + i * 5]))//키입력이 들어왔다.
                    {
                        CursorSetting(ref Cursor[i],j);   //커서이동.
                        MoveOK[i] = true;//이동할거임
                    }
            }
            if (MoveOK[i])//이동함?
            {
                if (Cursor[i].x < 0) Cursor[i].x = 3 + Cursor[i].x;
                else Cursor[i].x = Cursor[i].x % 3;

                if (Cursor[i].y < 0) Cursor[i].y = 3 + Cursor[i].y;
                else Cursor[i].y = Cursor[i].y % 3;

                StartCoroutine("MoveTarget", i);//타겟으로 이동
                Images[i].sprite = SpriteImg[(int)Cursor[i].x + (int)Cursor[i].y * 3];//큰얼굴 변경
            }
            else if (LockOn[i])//선택함?
            {
                if (!PlayerSelect[i])//플레이어가 선택이 되있지 않으면
                {
                    PlayerSelect[i] = true;//선택
                    CheckingCharacterIndex(i);//값저장
                }
            }
        }
    }
    void  CheckingCharacterIndex(int index)//선택하면 들어옴.
    {
        int TempCharacter = (int)(Cursor[index].x + 3 * Cursor[index].y);//선택한 캐릭터의 번호  
         GameData.ins.SetPlayer(TempCharacter, index);//게임데이터에 저장
        if (LockOn[1] && LockOn[0])//둘다 선택됬으면
            SceneManager.LoadScene("Main");//씬변경
    }
    IEnumerator MoveTarget(int index)
    {
        float Length = 200;
        Vector3 Prev = CursorTr[index].localPosition;
        Vector3 v = new Vector3();
        v.x = Cursor[index].x * Length;
        v.y = Cursor[index].y * Length;
        for (int i = 0; i <= 5; i++)
        {
            CursorTr[index].localPosition = Vector3.Lerp(Prev,v,i / 5f);
            yield return null;
        }
        MoveOK[index] = false;
    }
}
