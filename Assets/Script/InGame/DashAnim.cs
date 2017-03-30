using UnityEngine;
using System.Collections;

public class DashAnim : MonoBehaviour {

    private Transform MyTr;
    public float tempX;
    public float TempTime = 0;
    public Vector3 MovePos;
    private int Controller;
    // Use this for initialization
    void Awake()
    {
        MyTr = transform;
        MovePos = MyTr.position;
        tempX = MyTr.position.x;

    }
	void Start () {
        Controller = GetComponent<CharacterStatus>().Controller;
        Controller = Controller == 0 ? -1 : 1;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        MyTr.position = new Vector3(Mathf.Clamp(transform.position.x, -60.5f, 60.5f),
                                        MyTr.position.y, MyTr.position.z);
        tempX = Mathf.Clamp(tempX, -60.5f,60.5f);
    }
    public void Stop()
    {
        StopAllCoroutines();
    }
    public void Dash(float Position, float _Time)
    {
        Debug.Log(Position);
        MovePos.x = Position;
        TempTime = _Time;
        StartCoroutine(Dash());
    }
    private IEnumerator Dash()
    {
        Vector3 PrevPos = MyTr.position;
        float Speed = (MovePos.x - PrevPos.x) / TempTime;
        Vector3 TempV = PrevPos;
        float MaxSpeed = Speed * 2;
        for (float i = 0.0f; i < TempTime; i += Time.deltaTime)
        {
            float LerpT = i / TempTime;
            float TempSpeed = Mathf.Lerp(MaxSpeed, 0, LerpT);
            PrevPos.x += TempSpeed * Time.deltaTime;
            MyTr.position = PrevPos;
            
            tempX = MyTr.position.x;
            yield return null;
        }
    }
    public void Wait(float Position, float _Time)
    {
        StopAllCoroutines();
        tempX = MyTr.position.x;
    }
    public void Knockback(float Position ,float  _Time)
    {
        MovePos.x = Position;
        TempTime = _Time;
        StartCoroutine(Dash());
    }
    private IEnumerator Knockback()
    {
        Vector3 PrevPos = MyTr.position;
        float Speed = (MovePos.x - PrevPos.x) / TempTime;
        Vector3 TempV = PrevPos;
        float MaxSpeed = Speed * 2;
        for (float i = 0.0f; i < TempTime; i += Time.deltaTime)
        {
            float LerpT = i / TempTime;
            float TempSpeed = Mathf.Lerp(MaxSpeed, 0, LerpT);
            PrevPos.x += TempSpeed * Time.deltaTime;
            MyTr.position = PrevPos;
            tempX = MyTr.position.x;
            yield return null;
        }
    }
}
