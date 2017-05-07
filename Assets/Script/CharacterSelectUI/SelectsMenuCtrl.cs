using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SelectsMenuCtrl : MonoBehaviour {
    
    
    public Sprite[] SpriteImg;              //캐릭터 얼굴들
    public RectTransform[] CursorTr;        //캔버스 내부의 커서 트랜스폼.

    //0 = 1피, 1 = 2피
    public RectTransform[] CharacterIconTr;
    public int XLength;
    public int YLength;
    public Vector2[] Cursor;                //커서의 논리적 위치(인덱스)
    public Image[] Images;                  //커서에 대응되는 현재 캐릭터 이미지
    private bool[] MoveOK;                  //false = 이동끝남 , true = 이동중
    private bool[] CharacterSelectLockOn;                  //false = 선택안함 , true = 선택중
    private bool[] PlayerSelect;            //현재 캐릭터 선택 되어있는가
    public KeyCode[] KeysData = {KeyCode.W,KeyCode.A,KeyCode.S,KeyCode.D,KeyCode.F,                                       //1피 컨트롤
                                  KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.Keypad1}; //2피 컨트롤
    public KeyCode[] CancelKey = { KeyCode.G, KeyCode.Keypad2 };
    public StasisLevel[] PlayerStasis = { StasisLevel.CHARACTERSELECT, StasisLevel.CHARACTERSELECT };


    public enum StasisLevel
    {
        CHARACTERSELECT,
        ITEMSELECT,
        ALLOK,

    }
    // Use this for initialization

    private void Reset()
    {

        KeysData = new KeyCode[]{
            KeyCode.W,KeyCode.A,KeyCode.S,KeyCode.D,KeyCode.F,                                       //1피 컨트롤
                                  KeyCode.UpArrow,KeyCode.LeftArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.Keypad1}; //2피 컨트롤
        CancelKey = new KeyCode[] { KeyCode.G, KeyCode.Keypad2 };

    }
    void Start ()
    {
        PlayerSelect = new bool[2];
        MoveOK = new bool[2];
        CharacterSelectLockOn = new bool[2];
        for (int i = 0; i < 2; i++)
        {
            PlayerSelect[i] = false;
            CharacterSelectLockOn[i] = false;
            MoveOK[i] = false;
            StartCoroutine("StasisChecker", i);
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
    void Update()
    {
    }

    IEnumerator CharacterSelect(int i)
    {
        while(!CharacterSelectLockOn[i])//모든 입력 확인
        {
            if (MoveOK[i] || CharacterSelectLockOn[i])
            {
                //아무것도 들어오면 안됩니다.아직은
            }
            else if (Input.GetKeyDown(KeysData[5 * i + 4]))//입력키 눌렀다.
            {
                CharacterSelectLockOn[i] = true;//선택
                PlayerStasis[i] = StasisLevel.ITEMSELECT;//아이템저장
            }


            else
            {
                for (int j = 0; j < 4; j++)
                    if (Input.GetKeyDown(KeysData[j + i * 5]))//키입력이 들어왔다.
                    {
                        CursorSetting(ref Cursor[i], j);   //커서이동.
                        MoveOK[i] = true;//이동할거임
                    }
            }
            if (MoveOK[i])//이동함?
            {
                if (Cursor[i].x < 0) Cursor[i].x = XLength + Cursor[i].x;
                else Cursor[i].x = Cursor[i].x % XLength;

                if (Cursor[i].y < 0) Cursor[i].y = YLength + Cursor[i].y;
                else Cursor[i].y = Cursor[i].y % YLength;

                StartCoroutine("MoveTarget", i);//타겟으로 이동
                Images[i].sprite = SpriteImg[(int)Cursor[i].x + (int)Cursor[i].y * 3];//큰얼굴 변경
            }
            else if (CharacterSelectLockOn[i])//선택함?
            {
                if (!PlayerSelect[i])//플레이어가 선택이 되있지 않으면
                {
                    PlayerSelect[i] = true;//선택
                    CheckingCharacterIndex(i);//값저장
                }
            }
            yield return null;
        }

    }
    IEnumerator StasisChecker(int i)
    {
        while(true)
        {
            switch (PlayerStasis[i])
            {
                case StasisLevel.CHARACTERSELECT:
                    {
                        Debug.Log("asdsa");
                        StartCoroutine("CharacterSelect",i);
                        yield break;
                    }
                case StasisLevel.ITEMSELECT:
                    break;
                case StasisLevel.ALLOK:
                    break;
                default:
                    break;
            }
        }
    }
    IEnumerator CancelChecker(int i)
    {
        while (true)
        {
            if (Input.GetKeyDown(CancelKey[i]))
            {
                switch (PlayerStasis[i])
                {
                    case StasisLevel.ITEMSELECT:
                        {
                            CharacterSelectLockOn[i] = false;
                            PlayerStasis[i] = StasisLevel.CHARACTERSELECT;
                            StartCoroutine(StasisChecker(i));
                            
                        }
                        break;
                    case StasisLevel.ALLOK:
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }
    }
    void  CheckingCharacterIndex(int index)//선택하면 들어옴.
    {
        int TempCharacter = (int)(Cursor[index].x + 3 * Cursor[index].y);//선택한 캐릭터의 번호  
         GameData.ins.SetPlayer(TempCharacter, index);//게임데이터에 저장
        if (CharacterSelectLockOn[1] && CharacterSelectLockOn[0])//둘다 선택됬으면
            SceneManager.LoadScene("Main");//씬변경
    }
    IEnumerator MoveTarget(int index)
    {
        Vector3 Prev = CursorTr[index].localPosition;
        Vector3 v = new Vector3();
        int CursorIndex = (int)Cursor[index].x + (int)(Cursor[index].y * XLength);
        Debug.Log(CursorIndex);
        v = CharacterIconTr[CursorIndex].localPosition;
        Debug.Log(v);
       // v.x = Cursor[index].x * Length;
       // v.y = Cursor[index].y * Length;
        for (int i = 0; i <= 5; i++)
        {
            CursorTr[index].localPosition = Vector3.Lerp(Prev,v,i / 5f);
            yield return null;
        }
        MoveOK[index] = false;
    }
}
