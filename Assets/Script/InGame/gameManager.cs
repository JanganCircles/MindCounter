using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour
{
    public static gameManager ins = null;
    public CharacterStatus[] UserStatus;
    public SkillSlot[] UserSlot;
    public const int CHAMPION = 0;
    public const int CHALLANGER = 1;
    public bool AllSelect = false;


    private int[] StasisArr = { 0, 1, 2, 2, 2, 3, 4, 5, 6 };
    public bool GameEnd = false;
    public bool UIOpen = false;
    public int Combo;

    public STEP TempStep = STEP.START;

    public bool isCounter = false;
    public int Winner;
    public const int DROW = -1;
    public bool KeyAllClick = false;


    public int Turn = 0;

    public int TotalWinner = -1;
    public enum STEP : int
    {
        START = 0, KEYCHECK = 1, DECISION = 2, HITDAMAGE = 3, END = 4
    }
    // Use this for initialization
    void Awake()
    {
        Turn = 0;
        KeyAllClick = false;
        Winner = -1;
        Combo = 0;
        UserStatus = new CharacterStatus[2];
        UserStatus[CHAMPION] = GameObject.Find("Champion").GetComponent<CharacterStatus>();
        UserStatus[CHALLANGER] = GameObject.Find("Challanger").GetComponent<CharacterStatus>();
        UserStatus[CHAMPION].Controller = CHAMPION;
        UserStatus[CHALLANGER].Controller = CHALLANGER;
        UserSlot = new SkillSlot[2];
        UserSlot[CHAMPION] = GameObject.Find("Champion").GetComponent<SkillSlot>();
        UserSlot[CHALLANGER] = GameObject.Find("Challanger").GetComponent<SkillSlot>();


        if (ins == null)
            ins = this;
        else
            gameObject.SetActive(false);
    }
    void OnEnable()
    {
    }
    void Start()
    {

        StartCoroutine(GamePlay());
    }
    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator KeyCheck()
    {
        KeyAllClick = false;
        const float WaitTime = 5f;
        float Num = 0;
        bool[] CheckingOK = new bool[2];
        for (int i = 0; i < 2; i++)
        {
            if (UserStatus[i].RSPTempCount[0] == 0 && UserStatus[i].RSPTempCount[2] == 0 && UserStatus[i].RSPTempCount[1] == 0 &&
                UserStatus[i].Down)
                CheckingOK[i] = true;
            CheckingOK[i] = UserStatus[i].Disable;
        }
        while (Num < WaitTime && !(CheckingOK[CHAMPION] && CheckingOK[CHALLANGER]))
        {
            for (int i = 0; i < 2; i++)
            {
                if (!CheckingOK[i]) { 
                   CheckingOK[i] = UserSlot[i].KeyCheck();//두 플레이어 키 체크
                   if(CheckingOK[i] )
                        UserSlot[i].RunActive();
                }
            }
            Num += Time.unscaledDeltaTime;
            yield return null;
        }
        KeyAllClick = true;
    }
    public void Dash(DashAnim[] dashs)
    {
        float DashPivot = WallManager.ins.DashPivotX /10f;

        for (int i = 0; i < 2; i++)
        {
            if (Mathf.Abs(dashs[i].tempX * 10 - WallManager.ins.WallPivot) < 10)
            {
                dashs[i].Wait(DashPivot + i == CHALLANGER ? 0.5f : -0.5f, 1);
            }
            else
            {
                float DashCorrection = i == CHALLANGER ? 0.5f : -0.5f;
                dashs[i].Dash(DashCorrection + DashPivot, 1);
            }
        }
    }
    float CharacterDirection(int User)
    {
        return User == CHALLANGER ? 1f : -1f;
    }
    IEnumerator GamePlay()
    {
        DashAnim[] Dashs = new DashAnim[2];
        for (int i = 0; i < 2; i++)
        {
            Dashs[i] = UserStatus[i].gameObject.GetComponent<DashAnim>();
        }
        WallManager.ins.SetPivot();//벽과의 거리 설정
        yield return new WaitForSeconds(1f);
        while (true)
        {
            UIOpen = false;
            TempStep = STEP.START;
            SkillManager.ins.RunPassives("Start");//시작시 패시브 발동
            Dash(Dashs);//돌진애니메이션
            yield return new WaitForSeconds(0.25f);//입력대기
            Time.timeScale = 0.05f;//슬로우
            UIOpen = false;
            TempStep = STEP.KEYCHECK;
            SkillManager.ins.RunPassives("KeyCheck");//키입력시 패시브 발동
            StartCoroutine(KeyCheck());
            while (!KeyAllClick)
            {
                yield return null;
            }
            Time.timeScale = 1;
            yield return new WaitForSeconds(0.5f);
            //판정
            TempStep = STEP.DECISION;
            SkillManager.ins.RunPassives("Decision");//승자결정
            CheckingWinner();//승자체크
            
            TempStep = STEP.HITDAMAGE;
            UIOpen = true;
            if (Winner == DROW)
            {
                SkillManager.ins.RunPassives("Drow");//승자결정
                for (int i = 0; i < 2; i++)
                {
                    DebuffManager.Cleaner(i); 
                    Dashs[i].Knockback((WallManager.ins.DashPivotX + 50f * CharacterDirection(i)) * 0.1f, 0.5f);

                }
            }
            else
            {
                //아무도없ㅇ,,심심해
                int Loser = UserStatus[Winner].Enemy();//패자
                SkillManager.ins.RunPassives("Attack", Winner);//승자 어택
                SkillManager.ins.RunPassives("Hit", Loser);//데미지
                DebuffManager.Cleaner(Winner);
                int damage = DamageCalculator.ins.Calculate();
                Debug.Log(damage + "최종 데미지");
                Debug.Log((Winner == 0 ? "챔피언" : "챌린저") + "승");

                UserStatus[Loser].HpDown(damage);
                
                
                DebuffManager.ins.SetDebuff(Loser);
                WallManager.ins.SetPivot();//벽과의 거리 설정
                float KnockPlus = 33f;
                if (UserStatus[Loser].Down || UserStatus[Loser].Down || UserStatus[Loser].Down)
                    KnockPlus = 0;
                else
                    KnockPlus = 33f;
                Dashs[Loser].Knockback((WallManager.ins.DashPivotX+ KnockPlus * CharacterDirection(Loser)) * 0.1f, 0.5f);
            }
            TempStep = STEP.END;
            SkillManager.ins.RunPassives("End");////종료

            //게임끝났냐
            if (DieCheck())
            {
                TotalWinner = UserStatus[CHALLANGER].HP <= 0 ? CHAMPION : CHALLANGER;
                yield break;
            } Debug.Log("끝");
            yield return new WaitForSeconds(0.25f);
            Debug.Log("한바퀴");
            Turn++;
            for (int i = 0; i < 2; i++)
            {
                UserSlot[i].SelectReset();
            }
        }
    }
    bool DieCheck()
    {
        for (int i = 0; i < 2; i++)
        {
            if (UserStatus[i].HP <= 0)
                return true;
        }
        return false;
    }
    void CheckingWinner()
    {
        int[] Prioritys = { UserSlot[CHAMPION].GetPriority(), UserSlot[CHALLANGER].GetPriority() };
        bool RSPCheck = true;
        for (int i = 0; i < 2; i++)
        {
            if (!(Prioritys[i] >= Priority.ROCK && Prioritys[i] <= Priority.PAPER))
            {
                RSPCheck = false;
                break;
            }
        }
        if (RSPCheck)
        {
            if (Prioritys[CHAMPION] == Prioritys[CHALLANGER])
            {
                isRSPDrow();
            }
            else if ((Prioritys[CHAMPION] != Priority.ROCK && Prioritys[CHAMPION] - Prioritys[CHALLANGER] == 1) ||
                     Prioritys[CHAMPION] == Priority.ROCK && Prioritys[CHAMPION] - Prioritys[CHALLANGER] == -2)
            {
                Winner = CHAMPION;
            }
            else
            {
                Winner = CHALLANGER;
            }
        }
        else
        {
            if (Prioritys[CHAMPION] > Prioritys[CHALLANGER])
            {
                Winner = CHAMPION;
            }
            else if (Prioritys[CHAMPION] < Prioritys[CHALLANGER])
            {
                Winner = CHALLANGER;
            }
            else
            {
                Winner = DROW;
            }
        }

    }
    void isRSPDrow()
    {
        if (UserStatus[CHAMPION].Down || UserStatus[CHAMPION].Defence)
            Winner = CHALLANGER;
        else if (UserStatus[CHALLANGER].Down || UserStatus[CHALLANGER].Defence)
            Winner = CHAMPION;
        else
            Winner = DROW;
    }
}/*
    IEnumerator GamePlay()
    {
        while (true)
        {
            UIOpen = false;
            //벽거리 세팅
            UserStatus[CHAMPION].WallDistance = WallPivot;
            UserStatus[CHALLANGER].WallDistance = WallMax - WallPivot;
            //선택 전부 초기화
            for (int i = 0; i < 2; i++)
            {
                UserState[i].checkState(UserStatus[i].Disable);
                AllSelect = false;
            }
            float Timer = 0;
            //입력을 받는다
            while (Timer < 5.0f && !AllSelect)
            {
                if (UserState[CHAMPION].Select && UserState[CHALLANGER].Select)
                    AllSelect = true;
                yield return null;
            }

            UIOpen = true;
            //판정
            CheckWinner();
            yield return new WaitForSeconds(1f);
        }
    }
    void CheckWinner()
    {
        int ChallangerState = StasisArr[(int)UserState[CHALLANGER].TempState];
        int ChampionState = StasisArr[(int)UserState[CHAMPION].TempState];
        if (ChallangerState > ChampionState)
        {
            SetWinner(CHALLANGER);
        }
        else if (ChallangerState < ChampionState)
        {
            SetWinner(CHAMPION);
        }
        else
        {
            if (ChallangerState == 2)//가위바위보일떄
            {
                int Value = (int)UserState[CHAMPION].TempState - (int)UserState[CHALLANGER].TempState;
                if (Mathf.Abs(Value) == 2)
                {
                    Value *= -1;
                }
                if (Value > 0)
                {
                    SetWinner(CHAMPION);
                    Debug.Log("챔피언승");
                }
                else if (Value == 0)
                {
                    if (UserStatus[CHALLANGER].Down || UserStatus[CHALLANGER].Defence)
                    {
                        SetWinner(CHAMPION);
                    }
                    else if (UserStatus[CHAMPION].Down || UserStatus[CHAMPION].Defence)
                    {
                        SetWinner(CHALLANGER);
                    }
                    else
                    {
                        SetWinner(-1);
                    }
                    Debug.Log("무승부");
                }
                else
                {
                    SetWinner(CHALLANGER);
                    Debug.Log("챌린저승");
                }
            }
        }

    }
    void SetWinner(int win)
    {
        if (win != -1)
        {
            object[] Objarr = new object[3];
            Objarr[0] = UserStatus[win];
            Objarr[1] = UserStatus[(win + 1) % 2];
            Objarr[2] = UserState[win];
            UserStatus[win].gameObject.SendMessage("ActionWiner", Objarr);
        }
        else
        {
            //무승부..
        }
    }
    public void PivotMove(int User,int MovePos)
    {
        if (User == CHALLANGER)
        {
            WallPivot += MovePos;
        }
        else
        {
            WallPivot -= MovePos;
        }
        if (WallPivot < 0)
            WallPivot = 0;
        if (WallPivot > WallMax)
            WallPivot = WallMax;
    }

}
*/