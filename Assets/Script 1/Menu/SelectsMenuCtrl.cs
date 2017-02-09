using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectsMenuCtrl : MonoBehaviour {

    public UIDescription[] Player;
    public Sprite[] SpriteImg;
    public RectTransform[] CursorTr;
    public Vector2[] Cursor;
    public Image[] Images;
    private bool[] MoveOK;
    private bool[] MoveRunning;
    private bool[] LockOn;
    private bool AllOK;
    private bool[] CoroutineCheck;
    // Use this for initialization
    void Start () {
        CoroutineCheck = new bool[2];
        AllOK = false;
        CoroutineCheck[0] = false;
        CoroutineCheck[1] = false;
        MoveOK = new bool[2];
        MoveRunning = new bool[2];
        LockOn = new bool[2];
        LockOn[0] = false;
        LockOn[1] = false;
        MoveOK[0] = false;
        MoveOK[1] = false;
        MoveRunning[0] = false;
        MoveRunning[1] = false;
        for (int i = 0; i < 2; i++)
        {
            Player[i].Index = (int)Cursor[i].x + (int)Cursor[i].y * 3;
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (AllOK)
        {
            return;
        }
        if (MoveOK[0] || LockOn[0]) { }
        else if (Input.GetKeyDown(KeyCode.W)) { Cursor[0].y--; MoveOK[0] = true; }
        else if (Input.GetKeyDown(KeyCode.S)) { Cursor[0].y++; MoveOK[0] = true; }
        else if (Input.GetKeyDown(KeyCode.A)) { Cursor[0].x--; MoveOK[0] = true; }
        else if (Input.GetKeyDown(KeyCode.D)) { Cursor[0].x++; MoveOK[0] = true; }
        else if (Input.GetKeyDown(KeyCode.F)) { LockOn[0] = true; }

        if (MoveOK[1] || LockOn[1]) { }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) { Cursor[1].y--; MoveOK[1] = true; }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) { Cursor[1].y++; MoveOK[1] = true; }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) { Cursor[1].x--; MoveOK[1] = true; }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) { Cursor[1].x++; MoveOK[1] = true; }
        else if (Input.GetKeyDown(KeyCode.Keypad1)) { LockOn[1] = true; }

        for (int i = 0; i < 2; i++)
        {
            if (MoveOK[i] && !MoveRunning[i])
            {
                if (Cursor[i].x < 0) Cursor[i].x = 3 + Cursor[i].x;
                else Cursor[i].x = Cursor[i].x % 3;

                if (Cursor[i].y < 0) Cursor[i].y = 3 + Cursor[i].y;
                else Cursor[i].y = Cursor[i].y % 3;

                StartCoroutine("MoveTarget", i);
                Images[i].sprite = SpriteImg[(int)Cursor[i].x + (int)Cursor[i].y * 3];
                MoveRunning[i] = true;
                Player[i].Index = (int)Cursor[i].x + (int)Cursor[i].y * 3;
            }
            else if (LockOn[i])
            {
                Animator[] anims = CursorTr[i].GetComponentsInChildren<Animator>();
                for (int j = 0; j < 2; j++)
                {
                    anims[j].SetBool("LockOn", true);
                }
                if (!CoroutineCheck[i])
                {
                    CoroutineCheck[i] = true;
                    StartCoroutine(CheckingCharacterIndex(i));
                }
            }
        }
    }
    IEnumerator CheckingCharacterIndex(int index)//여기만 바꾸면 되네
    {
        yield return new WaitForSeconds(1.0f);
        if(index == 0 && GameData.ins.BlueIndex != 2)
        {
            GameData.ins.BlueCharacter[GameData.ins.BlueIndex] = (int)(Cursor[0].x + 3 * Cursor[0].y);

            if (GameData.ins.BlueIndex == 0)
            {
                Animator[] anims = CursorTr[index].GetComponentsInChildren<Animator>();
                LockOn[index] = false;
                for (int j = 0; j < 2; j++)
                {
                    anims[j].SetBool("LockOn", false);
                }
            }
            GameData.ins.BlueIndex++;
        }
        else if (index == 1 && GameData.ins.RedIndex != 2)
        {
            GameData.ins.RedCharacter[GameData.ins.RedIndex] = (int)(Cursor[1].x + 3 * Cursor[1].y);
            if (GameData.ins.RedIndex == 0)
            {
                Animator[] anims = CursorTr[index].GetComponentsInChildren<Animator>();
                LockOn[index] = false;
                for (int j = 0; j < 2; j++)
                {
                    anims[j].SetBool("LockOn", false);
                }
            }
            GameData.ins.RedIndex++;
        }

        CoroutineCheck[index] = false;
        if (GameData.ins.BlueIndex == 2 && GameData.ins.RedIndex == 2 && !AllOK)
        {
            AllOK = true;
            yield return new WaitForSeconds(2f);
            Debug.Log("SelectOK");
            GameData.ins.RedIndex = 0;
            GameData.ins.BlueIndex = 0;
            UIImageChanger.Release();
            UIIsVisible.Release();
            UINumberSet.Release();
            UIProgressBar.Release();
        }
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
        MoveRunning[index] = false;

    }
}
