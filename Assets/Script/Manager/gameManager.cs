﻿using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour
{
    public static gameManager ins = null;   //인스턴스

    public const int CHAMPION = 0;          //상수_챔피언
    public const int CHALLANGER = 1;        //상수_챌린저
    public const int DROW = -1;             //상수_비겼다.
    public const int PRIMETIME = 2;         //퍼펙트시간

    public const float PERPECT = 1.2f;      //퍼펙트_계수
    public const float GOOD = 1.0f;         //굿_계수
    public const float BAD = 0.7f;          //배드_계수
    
    public CharacterStatus[] UserStatus;    //캐릭터 스텟
    public SkillSlot[] UserSlot;            //스킬슬롯
    public float[] TimingWeight;            //잘맞았냐
    private CharacterAnim.AnimStasis[] AttackAnims;//공격할때 애니메이션들 세팅.



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
    public GameObject DamageObj;            //데미지

    public GameObject KeyController;        //키입력 받는 오브젝트


    public enum STEP : int                  //진행상황 열거자
    {
        START = 0, KEYCHECK = 1, DECISION = 2, HITDAMAGE = 3, END = 4
    }

    public enum KEYDECISION : int
    {
        PERPECT = 0, GOOD = 1, BAD = 2, MISS = 0
    }
    // Use this for initialization
    void Awake()
    {
        AttackAnims = new CharacterAnim.AnimStasis[6];
        Turn = 0;
        KeyAllClick = false;
        Winner = -1;
        Combo = 0;
        UserStatus = new CharacterStatus[2];
        UserSlot = new SkillSlot[2];
        GameObject[] players = new GameObject[2];
        players[CHALLANGER] = GameObject.Find("Challanger");
        players[CHAMPION] = GameObject.Find("Champion");
        for (int i = 0; i < 2; i++)
        {
            UserStatus[i] = players[i].GetComponent<CharacterStatus>();
            UserStatus[i].Controller = i;
            UserSlot[i] = players[i].GetComponent<SkillSlot>();
        }
        TimingWeight = new float[2];

        ins = this;
    }
    void OnEnable()
    {
        for (int i = 0; i < 2; i++)
        {
            UserSlot[i].UseItem = GameData.ins.PotionCode[i];
            UserSlot[i].EqulpmentItem = GameData.ins.EquipmentCode[i];
        }
    }
    void Start()
    {
        StartCoroutine(GamePlay());
    }
    // Update is called once per frame
    void Update()
    {
        //게임끝났냐
        if (DieCheck() && TotalWinner == -1)
        {
            TotalWinner = UserStatus[CHALLANGER].HP <= 0 ? CHAMPION : CHALLANGER;
            StopAllCoroutines();
            StartCoroutine(TotalWinnerAnimation());
            StartCoroutine(TotalLoserAnimation());
            SaveData.ins.ShowResultData();
        }

    }
    IEnumerator TotalWinnerAnimation()
    {
        CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.WIN, TotalWinner);
        yield return new WaitForSeconds(CharacterAnim.GetWinDuration(TotalWinner) );
        CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.WINNING, TotalWinner);
    }
    IEnumerator TotalLoserAnimation()
    {
        CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.LOSE, 1 - TotalWinner);
        yield return new WaitForSeconds(CharacterAnim.GetLoseDuration(1 - TotalWinner) - 0.25f);
        CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.LOSING, 1 - TotalWinner);
    }
    IEnumerator AnimationRun()
    {
        CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.SRUSH ,1f);
        //1번애니
        for (int i = 0; i < 3; i++)
        {
                CharacterAnim.ChangeAnimation(AttackAnims[CHALLANGER + i * 2], CHALLANGER);
            CharacterAnim.ChangeAnimation(AttackAnims[CHAMPION + i * 2], CHAMPION);
            yield return new WaitForSeconds(0.98f);
        }
    }
    public IEnumerator WinnerAudioRun(SkillSlot WinnerSlot)
    {
        string[] Character = {"", "고양이", "건틀릿", "암살자", "헐크" };
        int MotionNum = WinnerSlot.selectedSlot;
        if (MotionNum > 2 ) yield break;
        string Str = Character[GameData.ins.PlayerCharacter[Winner]] + " ";
        switch (MotionNum)
        {
            case 0:
                Str += "약";
                break;
            case 1:
                Str += "중";
                break;
            case 2:
                Str += "강";
                break;

        }
        switch (Str)
        {
            case "암살자 강": case "고양이 강": break;
            default:
                yield return new WaitForSeconds(1f);
                break;
        }
        Debug.Log(Str);
        SoundManager.ins.RunAudio(Str);
    }
    public void HitEffect(float Damage)
    {
        int Loser = 1 - Winner;
        if (UserStatus[Loser].Guard && Damage == 0)
        {
            SoundManager.ins.RunAudio("guard");
            DamageObj.SendMessage("OnDamage", DamageEffect.TargetImage.GUARD);
        }
        else if (Damage == 0)
        {
            SoundManager.ins.RunAudio("miss");
            DamageObj.SendMessage("OnDamage", DamageEffect.TargetImage.MISS);
        }
        else
        {
            DamageObj.SendMessage("OnDamage", DamageEffect.TargetImage.DMG);
        }
        Vector3 LoserPos = UserStatus[Loser].transform.position;
        LoserPos.y = 1;
        int index = UserSlot[Winner].GetPriority();
        if (UserStatus[Loser].Guard && Damage == 0)
            index = 0;
        string[] Names =  { "Guard","" ,"", "LowAttack", "MidAttack" , "HighAttack" };
        switch (index)
        {
            case 0:
                EffectManager.ins.EffectRun(UserStatus[Loser].transform, Vector3.one, Names[index], false);
                break;

            case 5:
                EffectManager.ins.EffectRun(LoserPos, Vector3.one, "ObjHighAttack", false);
                EffectManager.ins.EffectRun(LoserPos, Vector3.one, Names[index], false);
                break;
            case 3:
            case 4:
                EffectManager.ins.EffectRun(LoserPos, Vector3.one, Names[index], false);
                break;
                

        }
    }
    public void AnimationSetting( int step, CharacterAnim.AnimStasis stasis)
    {
        for (int i = 0; i < 2; i++)
            AnimationSetting(i, step, stasis);
    }
    public void AnimationSetting(int index, int step,CharacterAnim.AnimStasis stasis)
    {
        AttackAnims[index + step * 2] = stasis;
    }
    IEnumerator GamePlay()
    {
        DashAnim[] Dashs = new DashAnim[2]; // 돌진관련 변수
        for (int i = 0; i < 2; i++) { 
            Dashs[i] = UserStatus[i].gameObject.GetComponent<DashAnim>();// 돌진관련변수 - 초기화
        }
        InputController IControl = KeyController.GetComponent<OfflineController>();
        if (KeyController != null)
        {
            IControl = KeyController.GetComponent<OfflineController>();
            if (GameData.ins.isOnline)
            {
                Debug.Log("온라인모드");
                KeyController.GetComponent<OfflineController>().enabled = false;
                IControl = KeyController.GetComponent<OnlineController>();
            }
            else
            {
                Debug.Log("오프라인");
                KeyController.SetActive(true);
              //  KeyController.GetComponent<OnlineController>().enabled = false;
            }
        }
        WallManager.ins.SetPivot();//벽과의 거리 설정
        yield return new WaitForSeconds(0.1f);// 스타트 함수 대기
        GameData.ins.InitPlayer();            // 플레이어 초기화
        for (int i = 0; i < 2; i++)
        {
            CharacterStatusSetup.SetupStat(i, (int)GameData.ins.Characters[i]);
            if (!SkillManager.ins.CharacterSkillSetup(i))
            {
                Debug.Log("에러 - 제작되지 않은 캐릭터로 초기화");
                Debug.Break();
            }
        }
        yield return new WaitForSeconds(1f);// 게임 시작전 딜레이



        SkillManager.ins.RunPassives("GameStart");// 게임처음시작
        for (int i = 0; i < 2; i++)
        {
            UITextSet.UIList[i == 0 ? "Champion" : "Challanger" + "Potion"] = (UserSlot[i].GetSlot("아이템0").GetSkill() as StackSkill).TempStack.ToString();
        }
        while (true)// 게임시작 루프
        {
            isCounter = false;
            UIOpen = false;
            TempStep = STEP.START;// 실행단계
            SkillManager.ins.RunPassives("Start");// 시작시 패시브 발동

            CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.RUSH);
            Dash(Dashs);//이동!
            yield return new WaitForSeconds(0.25f);//입력대기
            UIOpen = false;
            TempStep = STEP.KEYCHECK;// 키확인단계
            UIMoveImage.BarMove();
            SkillManager.ins.RunPassives("KeyCheck");//키입력시 패시브 발동
            IControl.CheckingKey();
            float[] KeyCatchTime = IControl.GetCatchTime();
            // StartCoroutine(KeyCheck());

            float AnimationTimer = 1f;
            // while (!KeyAllClick)
            while (!IControl.CheckingRunEffect())
            {
                if (AnimationTimer > 0)
                {
                    AnimationTimer -= Time.unscaledDeltaTime;
                }
                else if (AnimationTimer < 0)
				{
					Time.timeScale = 0.05f;//슬로우
                    CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.SRUSH,5f);
                    AnimationTimer = 0;
                }
                    //키체크가 끝날때까지 무한 루프
                    yield return null;
            }
            float[] TimingArr= IControl.GetCatchTime();
            for (int i = 0; i < 2; i++)
                CatchTiming(TimingArr[i], i);
            Time.timeScale = 1;//평소대로
            //판정
            TempStep = STEP.DECISION;// 판정단계
            SkillManager.ins.RunPassives("Decision");//판정시 패시브 발동
            for (int i = 0; i < 2; i++)
            {
                UserStatus[i].MPProgressBar();
            }
            CheckingWinner();//승자체크 - 이떄부터 위너 사용 가능
            GuardCheck(Dashs, Winner);//가드시 이동그만.
            //CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.SRUSH);
            float[] Duration = { CharacterAnim.GetTempDuration(0), CharacterAnim.GetTempDuration(1) };
            yield return new WaitForSeconds(0.25f);//애니메이션 딜레이

            TempStep = STEP.HITDAMAGE;// 데미지피격단계
            UIOpen = true;
            AnimationSetting(2, CharacterAnim.AnimStasis.LAND);//막애니메이션은 밀쳐짐.
            if (Winner == DROW ||(Winner != DROW && UserStatus[Winner].DontDash))//무승부
            {
                SkillManager.ins.RunPassives("Drow");//비길때 패시브 발동
                StartCoroutine(AnimationRun());
                yield return new WaitForSeconds(0.95f);//애니메이션 딜레이
                for (int i = 0; i < 2; i++)
                {
                    //드로우면 서로 넉백
                    if (Mathf.Abs(UserStatus[CHAMPION].transform.position.x - UserStatus[CHALLANGER].transform.position.x) < 10)
                    {
                        AnimationSetting(1, CharacterAnim.AnimStasis.LAND);//막애니메이션은 밀쳐짐.
                        Dashs[i].Knockback((WallManager.ins.DashPivotX + 50f * CharacterDirection(i)) * 0.1f, 0.5f);//서로넉백
                    }
                }
                yield return new WaitForSeconds(0.05f);//애니메이션 딜레이
            }
            else
            {//판가름 남.
                CharacterAnim.SetOrderinLayer();
                SaveData.ins.AddData(SaveData.TYPE.STILL, Winner, 1);//데이터 저장

                int Loser = UserStatus[Winner].Enemy();//패자
                AnimationSetting(Loser, 1, CharacterAnim.AnimStasis.HIT);//막애니메이션은 밀쳐짐.
                CharacterAnim.ChangeAnimation(CharacterAnim.AnimStasis.HIT, Loser);
                SkillManager.ins.RunPassives("Attack", Winner);//승자 어택
                SkillManager.ins.RunPassives("Hit", Loser);//데미지

                DamageCalculator.ins.AddDamage(DamageCalculator.MULTIPLE_s, TimingWeight[Winner], "RhythmWeight");
                int damage = DamageCalculator.ins.Calculate();//데미지 계산

                Debug.Log(damage + "최종 데미지");
                Debug.Log((Winner == 0 ? "챔피언" : "챌린저") + "승");

                //yield return new WaitForSeconds(CharacterAnim.GetTempDuration(Winner));//애니메이션 딜레이
                StartCoroutine(AnimationRun());
                if(damage!=0)
                StartCoroutine(WinnerAudioRun(UserSlot[Winner])); // 공격자 소리 재생
                 yield return new WaitForSeconds(1f);//애니메이션 딜레이
                HitEffect(damage);
                UITextSet.UIList["Damage"] = damage.ToString();
                if (!UserStatus[Winner].DontDash) { 
                    CameraController.LockPosition = true; // 카메라 컨트롤러
                    CameraController.Winner = Winner;     // " 위와동일
                }
                SkillManager.ins.RunPassives("WallSetting");//벽이동 전 패시브 발동
                Shake(damage * 16);                                 //지진
                UserStatus[Loser].HpDown(damage);              //HP깎고
                UITextSet.UIList["Damage"] = damage.ToString();//최종 데미지 UI
                bool LoserConer = UserStatus[Loser].WallDistance == 0;
                float pMove = WallManager.ins.PivotMove;
                WallManager.ins.SetPivot();//벽과의 거리 설정
                //넉백되는구간
                float KnockPlus = 50f;
                if (pMove == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Dashs[i].Knockback((WallManager.ins.DashPivotX + 50f * CharacterDirection(i)) * 0.1f, 0.5f);//서로넉백
                    }
                }
                else if (LoserConer || UserStatus[Loser].isSuperArmor)
                {
                    if (UserStatus[Loser].isSuperArmor)
                        Debug.Log("맞은사람이 슈아임.");
                    KnockPlus = 85f;
                    Dashs[Winner].Knockback(((WallManager.ins.DashPivotX + KnockPlus * CharacterDirection(Winner)) * 0.1f), 0.5f);
                }
                else
					Dashs[1 - Winner].Knockback(((WallManager.ins.DashPivotX + KnockPlus * CharacterDirection(Loser)) * 0.1f * TimingWeight[Winner]), 0.5f);

            }
            TempStep = STEP.END;//스탭변경 - 종료
            SkillManager.ins.RunPassives("End");////종료

            Debug.Log("끝");
            yield return new WaitForSeconds(1f);
            CameraController.LockPosition = false;
            Debug.Log("한바퀴");
            Turn++;
            for (int i = 0; i < 2; i++)
            {
                UserSlot[i].SelectReset();
                UserStatus[i].TurnStart();
            }
        }
    }
    void CatchTiming(float _Time,int Index)
    {
        _Time = Mathf.Abs(_Time - PRIMETIME);
        if (_Time < 0.02f)
        {
            //Perfect
            TimingWeight[Index] = 1.2f;
        }
        else if (_Time < 0.25f)
        {
            //Good
            TimingWeight[Index] = 1f;
        }
        else if (_Time < 0.4f)
        {
            TimingWeight[Index] = 0.7f;
            //Bad
        }
        else
        {
            TimingWeight[Index] = 0;
            //실패
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
        }
    }//콤보, 20170330 사용x
    //이부분 바꿔야한다.
    IEnumerator KeyCheck()//키확인 함수
    {
        KeyAllClick = false;
        const float WaitTime = 5f;
        float Num = 0;
        bool[] CheckingOK = new bool[2];
        for (int i = 0; i < 2; i++)
        {
            CheckingOK[i] = false;
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
    } // 20170330 사용 x
    public void Dash(DashAnim[] dashs)//달려드는함수
    {
        float DashPivot =  WallManager.ins.DashPivotX / 10f;

        for (int i = 0; i < 2; i++)
        {
           // if (Mathf.Abs(dashs[i].tempX * 10 - WallManager.ins.WallPivot) < 10)
            // {
            //     Debug.Log("Wait실행됨");
            //     dashs[i].Wait(DashPivot + i == CHALLANGER ? 0.5f : -0.5f, 1);
            // }
            // else
            if(true)
            {
                Debug.Log("Dash실행됨");
                float DashCorrection = i == CHALLANGER ? 1.5f : -1.5f;
                if (DashCorrection < 0) DashCorrection = 0;
                 dashs[i].Dash(DashCorrection + DashPivot, 2);
            }
        }
    }
    public void GuardCheck(DashAnim[] Dashs, int Winner)//가드면 멈춰
    {
        if (Winner == DROW)
        {
            if (UserStatus[CHALLANGER].DontDash && UserStatus[CHAMPION].DontDash)
            {
                Dashs[CHALLANGER].Stop();
                Dashs[CHAMPION].Stop();
                //멈춰
            }
        }
        else
        {
            int Loser = UserStatus[Winner].Enemy();
            if (UserStatus[Winner].DontDash)
            {
                Dashs[Loser].Stop();
                Dashs[Winner].Knockback((WallManager.ins.DashPivotX + 100f * CharacterDirection(Winner)) * 0.1f, 0.5f);
            }
            else if (UserStatus[Loser].DontDash)
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
            if (!(Prioritys[i] >= Priority.SCISSOR && Prioritys[i] <= Priority.PAPER))
            {
                RSPCheck = false;
                break;
            }
        }
        if (RSPCheck)//가위바위보
        {
            if (Prioritys[CHAMPION] == Prioritys[CHALLANGER])
            {
                Winner = DROW;
            }
            else
            {
                isCounter = true;
                if ((Prioritys[CHAMPION] != Priority.SCISSOR && Prioritys[CHAMPION] - Prioritys[CHALLANGER] == 1) ||
                         Prioritys[CHAMPION] == Priority.SCISSOR && Prioritys[CHAMPION] - Prioritys[CHALLANGER] == -2)
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
}