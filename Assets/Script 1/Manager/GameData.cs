using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameData : MonoBehaviour {
    public static GameData ins { get; set; }
    public int[] RedCharacter;
    public int[] BlueCharacter;
    public int RedIndex;
    public int BlueIndex;
    GameObject Round ;
    GameObject Number;
    GameObject Start;
	// Use this for initialization
    void Awake()
    {
        if (ins == null)
        {
            RedCharacter = new int[2];
            RedCharacter[0] = -1;
            RedCharacter[1] = -1;
            BlueCharacter = new int[2];
            BlueCharacter[0] = -1;
            BlueCharacter[1] = -1;
            RedIndex = 0;
            BlueIndex = 0;
            ins = this;
        }
        DontDestroyOnLoad(this);
    }
    public void PlayGame(ref int Red,ref int Blue)
    {
        Red = RedCharacter[RedIndex];
        Blue = BlueCharacter[BlueIndex];
        StartCoroutine(WaitTime());
    }

    public void Release()
    {
        ins = null;
        Destroy(gameObject);
    }

    IEnumerator WaitTime()
    {
         Round = GameObject.Find("Round");
         Number = GameObject.Find("Number");
         Start = GameObject.Find("Start");
        Round.SetActive(false);
        Number.SetActive(false);
        Start.SetActive(false);
        yield return new WaitForSeconds(1f);
        Round.SetActive(true);
        //라운드원
        yield return new WaitForSeconds(1f);
        Number.GetComponent<Text>().text = (RedIndex + BlueIndex + 1).ToString();
        Number.SetActive(true);
        //스타트
        yield return new WaitForSeconds(1f);
        Start.SetActive(true);
        yield return new WaitForSeconds(2f);

        Round.SetActive(false);
        Number.SetActive(false);
        Start.SetActive(false);
        //삭제후 바로 ㄱㄱ
        GameStarter.ins.Run = true;
    }
    public IEnumerator EndComent(int Winner)
    {
        Round.SetActive(true);
        Number.SetActive(true);
        Start.SetActive(true);
        Round.GetComponent<Text>().text = (BlueIndex+ RedIndex ) + "Round";
        if (Winner == Utillity.BLUEPLAYER)
            Number.GetComponent<Text>().text = "Blue";
        else
            Number.GetComponent<Text>().text = "Red";
        Number.GetComponent<Text>().text += " Player";
        Start.GetComponent<Text>().text = "Win";

        Round.SetActive(false);
        Number.SetActive(false);
        Start.SetActive(false);

        yield return new WaitForSeconds(1f);
        Round.SetActive(true);
        //라운드원
        yield return new WaitForSeconds(1f);
        Number.SetActive(true);
        //스타트
        yield return new WaitForSeconds(1f);
        Start.SetActive(true);
        yield return new WaitForSeconds(2f);

        Round.SetActive(false);
        Number.SetActive(false);
        Start.SetActive(false);
    }
}
