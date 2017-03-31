using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoveImage : MonoBehaviour {
    public delegate void MoveBar();
    public static MoveBar BarMove = null;
    public Vector2 EndPosition;
    private Vector2 StartPosition;
    private Vector2 direction;
    private RectTransform RectTr;
    private Image Img;
    public float RunningTime;//목표도달까지 걸리는 시간
    public float OverTime;  //목표 도달 이후에 이동하는 시간
	// Use this for initialization
	void Start () {
        RunningTime = gameManager.PRIMETIME;
        OverTime = 5 - gameManager.PRIMETIME;

        Img = GetComponent<Image>();
        RectTr = GetComponent<RectTransform>();
        StartPosition = RectTr.position;
        EndPosition = transform.parent.GetComponent<RectTransform>().position;
        BarMove += Move;
    }

    public void Move()
    {
        StartCoroutine(iMove());
    }
    IEnumerator iMove()
    {
        Color C = Img.color;
        C.a = 1;
        Img.color = C;
        float TempTime = 0.0f;
        Vector2 SecToMovePosition = (EndPosition - StartPosition) / RunningTime;
        while (TempTime < RunningTime && gameManager.ins.TempStep == gameManager.STEP.KEYCHECK)
        {
            RectTr.position = StartPosition + (SecToMovePosition * TempTime);
            TempTime += Time.unscaledDeltaTime;
            yield return null;
        }
        RectTr.position = EndPosition;
        TempTime = 0.0f;
        while (TempTime < OverTime && gameManager.ins.TempStep == gameManager.STEP.KEYCHECK)
        {
            C.a = 1 - TempTime / OverTime;
            Img.color = C;
            RectTr.position = EndPosition + (SecToMovePosition * TempTime);
            TempTime += Time.unscaledDeltaTime;
            yield return null;
        }
        C.a = 0;
        Img.color = C;
    }

}
