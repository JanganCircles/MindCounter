using UnityEngine;
using System.Collections;

public class UIprogress : MonoBehaviour {
    private CharacterStatus status;
    public GameObject CharHPred, CharHPblank;
    public int nowHP = 0;
    public int maxHp = 0;
    public int Cost = 0;
    public float floatHP = 0f;
    public float nowDistance = 0;
    public int setDistanceWarning = 200; // 벽 남은거리에 따른 경고 설정 
    public GameObject warning_img;
    public GameObject countBoxMax , skillCount , skillCountClone;
    int[] countBoxMaxArg;
    int count = 0;



    void Awake()
    {
        status = gameObject.GetComponent<CharacterStatus>();// 왜 이렇게 해주는거임?
        
    }
    void Start ()
    {
        nowDistance = status.WallDistance; // 벽의 거리 저장
        
        

      /*int[] countBoxMaxArg = new int[maxBox]; // 카운트 최대 개수로 배열 만들어서 그만큼 오브젝트 만들꺼임  
      for (int i =0; i< maxBox; i++)
      {
          countBoxMaxArg[i] += 1; // 배열에 전부 1씩 넣어줌 1은 칸이 채워져 있고 0은 공백
          Debug.Log(countBoxMaxArg[i]);  
      }
      */

        Vector3 ChamCountVect = new Vector3(-275, -46, 0); // 왼쪽 챔피언 박스 위치 28씩 증가 
        Vector3 ChallCountVect = new Vector3(270, -46, 0); //오른쪽 챌린저 박스 위치 28씩 감소함 
        Vector3 test = new Vector3(242, -46, 0);
        Quaternion zero = new Quaternion(0, 0, 0, 0);
        skillCountClone = (GameObject)Instantiate(skillCount, test, zero);
        
        /* 2017_02_21 에러로 주석처리
         *for (int i=0; i <= status.RSPMaxCount.Length; i++)
         *{
         *    skillCountClone =(GameObject)Instantiate(skillCount, ChamCountVect, skillCount.transform.rotation);
         *    ChamCountVect.x += 28;
         *    Debug.Log(i + "번째ChamCountVect는"+ ChamCountVect.x);
         *}
         *for (int i = 0; i <= status.RSPMaxCount.Length; i++)
         *{
         *    Instantiate(countBoxMax).transform.TransformVector(ChallCountVect);//얘도 장애다 
         *    ChallCountVect.x -= 28;
         *    Debug.Log(i + "번째ChallCountVect는" + ChallCountVect.x);
         *}
         */
        nowHP = status.HP;
        maxHp = 800;//status.MaxHP;//속 0이라서 일단 임의의 값 줬음.

        warning_img.SetActive(false);
        
        //----코루틴---
        StartCoroutine("setHP");
        //StartCoroutine("Countbox");


    }
	
	// Update is called once per frame
	void Update () {
        // nowHP = status.HP;
        nowDistance = status.WallDistance;// 벽거리 계속 연산
        floatHP = (float)nowHP / (float)maxHp;

        WallDistance();// 남은 벽거리가 setDistanceWarning 이하가 되면 warning_img 활성화 

        

    }
    
    
    IEnumerator setHP()
    {
        
        while (true)
        {
            nowHP = status.HP;
            CharHPred.transform.localScale = new Vector3(floatHP, CharHPred.transform.localScale.y, CharHPred.transform.localScale.z);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Countbox()
    {

        yield break;
       /*
        * 2017_02_21 에러로인해 주석처리(RSPTempCount 삭제함)
        * 
        *while (true)
        *{
        *    for (int i = 0; i < status.RSPMaxCount.Length; i++)
        *    {
        *        if (status.RSPTempCount[i] == 0)
        *        {
        *            skillCount.SetActive(false);
        *        }
        *        //Debug.Log(countBoxMaxArg[i]);
        *    }
        *    yield return new WaitForSeconds(0.1f);
        *}
        */
    }
    void WallDistance()
    {
        if (nowDistance <= setDistanceWarning)
        {
            warning_img.SetActive(true);
        }
        if (nowDistance >= setDistanceWarning)
        {
            warning_img.SetActive(false);
        }
    } 
}
