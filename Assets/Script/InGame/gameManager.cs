using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour
{
    public static gameManager ins = null;   //인스턴스

    public const int CHAMPION = 0;          //상수_챔피언
    public const int CHALLANGER = 1;        //상수_챌린저
    public const int DROW = -1;             //상수_비겼다.

    public CharacterStatus[] UserStatus;    //캐릭터 스텟
    public SkillSlot[] UserSlot;            //스킬슬롯

    
    public bool UIOpen = false;             //유아이 보여주기
    public int Combo;                       //현재 콤보
    public int ComboContinues;              //현재 콤보중인 사람

    public STEP TempStep = STEP.START;      //현재 진행상황

    public bool isCounter = false;          //카운터 공격인가(미구현)
    public int Winner;                      //현재 승자
    public bool KeyAllClick = false;        //키입력확인(전부 들어왔는가)
    public int Turn = 0;                    //저장용_턴
    public int TotalWinner = -1;            //최종승자(챔피언or챌린저)
    public bool Simulate;                   //AI사용

    public enum STEP : int                  //진행상황 열거자
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
        //게임끝났냐
        if (DieCheck())
        {
            TotalWinner = UserStatus[CHALLANGER].HP <= 0 ? CHAMPION : CHALLANGER;
            SaveData.ins.ShowDebug();
            StopAllCoroutines();
        }

    }
    IEnumerator GamePlay()
    {
        DashAnim[] Dashs = new DashAnim[2]; // 돌진관련 변수
        for (int i = 0; i < 2; i++) { 
            Dashs[i] = UserStatus[i].gameObject.GetComponent<DashAnim>();// 돌진관련변수 - 초기화
        }
        
        WallManager.ins.SetPivot();//벽과의 거리 설정
        yield return new WaitForSeconds(0.1f);// 스타트 함수 대기
        GameData.ins.InitPlayer();            // 플레이어 초기화
        for (int i = 0; i < 2; i++)
        {
            CharacterData.SetupStat(i, (int)GameData.ins.Characters[i]);
            if (!SkillManager.ins.CharacterSkillSetup(i))
            {
                Debug.Log("에러 - 제작되지 않은 캐릭터로 초기화");
                Debug.Break();
            }
        }
        yield return new WaitForSeconds(1f);// 게임 시작전 딜레이

        while (true)// 게임시작 루프
        {
            isCounter = false;
            UIOpen = false;
            TempStep = STEP.START;// 실행단계
            SkillManager.ins.RunPassives("Start");// 시작시 패시브 발동

            for (int i = 0; i < 2; i++)// 방어체크 - 게임에 영향 x
            {
                if (UserStatus[i].Defence)
                {
                    SaveData.ins.AddData(SaveData.TYPE.STILL, (i + 1)%2, 1);
                    break;
                }
            }

            Dash(Dashs);//돌진애니메이션
            yield return new WaitForSeconds(0.25f);//입력대기
            Time.timeScale = 0.05f;//슬로우
            UIOpen = false;
            TempStep = STEP.KEYCHECK;// 키확인단계
            SkillManager.ins.RunPassives("KeyCheck");//키입력시 패시브 발동
            StartCoroutine(KeyCheck());

            while (!KeyAllClick)
            {
                //키체크가 끝날때까지 무한 루프
                yield return null;
            }
            Time.timeScale = 1;//평소대로
            //판정
            TempStep = STEP.DECISION;// 판정단계
            SkillManager.ins.RunPassives("Decision");//판정시 패시브 발동
            CheckingWinner();//승자체크 - 이떄부터 위너 사용 가능
            GuardCheck(Dashs, Winner);//가드시 이동그만.
            yield return new WaitForSeconds(0.5f);//애니메이션 딜레이

            TempStep = STEP.HITDAMAGE;// 데미지피격단계
            UIOpen = true;
            if (Winner == DROW)//무승부
            {
                SkillManager.ins.RunPassives("Drow");//비길때 패시브 발동
                for (int i = 0; i < 2; i++)
                {
                    DebuffManager.Cleaner(i); 
                    Dashs[i].Knockback((WallManager.ins.DashPivotX + 50f * CharacterDirection(i)) * 0.1f, 0.5f);//서로넉백
                }
            }
            else
            {//판가름 남.
                int Loser = UserStatus[Winner].Enemy();//패자
                SkillManager.ins.RunPassives("Attack", Winner);//승자 어택
                SkillManager.ins.RunPassives("Hit", Loser);//데미지
                DebuffManager.Cleaner(Winner);             //승자 디버프 제거(방어,다운)

                int damage = DamageCalculator.ins.Calculate();//데미지 계산

                //
                Debug.Log(damage + "최종 데미지");
                Debug.Log((Winner == 0 ? "챔피언" : "챌린저") + "승");

                Shake(damage);                                 //지진
                UserStatus[Loser].HpDown(damage);              //HP깎고
                UITextSet.UIList["Damage"] = damage.ToString();//최종 데미지 UI

                ComboFunc(Winner);                             //콤보설정

                DebuffManager.ins.SetDebuff(Loser);//디버프설정
                WallManager.ins.SetPivot();//벽과의 거리 설정

                float KnockPlus;
                if (UserStatus[Loser].Down || UserStatus[Loser].Disable || UserStatus[Loser].Defence)
                    KnockPlus = 0f;
                else
                    KnockPlus = 25f;
                Dashs[Loser].Knockback((WallManager.ins.DashPivotX+ KnockPlus * CharacterDirection(Loser)) * 0.1f, 0.5f);

            }
            TempStep = STEP.END;//스탭변경 - 종료
            SkillManager.ins.RunPassives("End");////종료

            Debug.Log("끝");
            yield return new WaitForSeconds(0.25f);
            Debug.Log("한바퀴");
            Turn++;
            for (int i = 0; i < 2; i++)
            {
                UserSlot[i].SelectReset();
            }
        }
    }
    
     void ComboFunc(int winner)
    {
        int enemy = UserStatus[winner].Enemy();
        if (!UserStatus[enemy].Guard && !UserStatus[winner].Guard)
        {
            if (ComboContinues != winner)
            {
                ComboContinues = winner;
                Combo = 1;
            }
            else
            {
                Combo++;
            }
        }
        if (Combo == 8)
        {
            UserStatus[winner].RSPSet(UserStatus[winner].RSPMaxCount[0], UserStatus[winner].RSPMaxCount[1], UserStatus[winner].RSPMaxCount[2]);
        }
    }
    IEnumerator KeyCheck()//키확인 함수
    {
        KeyAllClick = false;
        const float WaitTime = 5f;
        float Num = 0;
        bool[] CheckingOK = new bool[2];
        for (int i = 0; i < 2; i++)
        {
            CheckingOK[i] = UserStatus[i].Disable;
            if (UserStatus[i].RSPTempCount[0] == 0 && UserStatus[i].RSPTempCount[2] == 0 && UserStatus[i].RSPTempCount[1] == 0 &&
                UserStatus[i].Down)
            {
                CheckingOK[i] = true;
                for (int j = 0; j < 3; j++)
                    UserStatus[i].RSPTempCount[j] += 1;
            }
        }
        while (Num < WaitTime && !(CheckingOK[CHAMPION] && CheckingOK[CHALLANGER]))
        {
            for (int i = 0; i < 2; i++)
            {
                if (!CheckingOK[i])
                {
                    CheckingOK[i] = UserSlot[i].KeyCheck();//두 플레이어 키 체크

                    if (CheckingOK[i])
                        UserSlot[i].RunActive();
                }
            }
            Num += Time.unscaledDeltaTime;
            yield return null;
        }
        KeyAllClick = true;
    }
    public void Dash(DashAnim[] dashs)//달려드는함수
    {
        float DashPivot = WallManager.ins.DashPivotX / 10f;

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
    public void GuardCheck(DashAnim[] Dashs, int Winner)//가드면 멈춰
    {
        if (Winner == DROW)
        {
            if (UserStatus[CHALLANGER].Guard && UserStatus[CHAMPION].Guard)
            {
                Dashs[CHALLANGER].Stop();
                Dashs[CHAMPION].Stop();
                //멈춰
            }
        }
        else
        {
            int Loser = UserStatus[Winner].Enemy();
            if (UserStatus[Winner].Guard)
            {
                Dashs[Loser].Stop();
                Dashs[Winner].Knockback((WallManager.ins.DashPivotX + 100f * CharacterDirection(Winner)) * 0.1f, 0.5f);
            }
            else if (UserStatus[Loser].Guard)
            {
                Dashs[Loser].Stop();
                //멈춰
                Dashs[Winner].Dash(Dashs[Loser].tempX + 0.5f * CharacterDirection(Winner), 0.5f);
            }
        }
    }
    float CharacterDirection(int User)//캐릭터들의 방향확인.
    {
        return User == CHALLANGER ? 1f : -1f;
    }
    void Shake(int Power)
    {

        if (Power > 100)
        {
            CameraEffectmanager.ins.Shake(SHAKEPOWER.BIG);
        }
        else if (Power > 50)
        {
            CameraEffectmanager.ins.Shake(SHAKEPOWER.MIDDLE);
        }
        else if (Power > 0)
        {
            CameraEffectmanager.ins.Shake(SHAKEPOWER.SMALL);
        }
    }
    bool DieCheck()
    {
        for (int i = 0; i < 2; i++)
        {
            UserStatus[i].CheckDie();
            if (UserStatus[i].Life == 0)
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
        if (RSPCheck)//가위바위보
        {
            if (Prioritys[CHAMPION] == Prioritys[CHALLANGER])
            {
                isRSPDrow();//같은거냄
            }
            else
            {
                isCounter = true;
                if ((Prioritys[CHAMPION] != Priority.ROCK && Prioritys[CHAMPION] - Prioritys[CHALLANGER] == 1) ||
                         Prioritys[CHAMPION] == Priority.ROCK && Prioritys[CHAMPION] - Prioritys[CHALLANGER] == -2)
                {
                    Winner = CHAMPION;//카운터
                }
                else
                {
                    Winner = CHALLANGER;//너도
                }
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
}