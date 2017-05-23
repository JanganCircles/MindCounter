using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Transform[] Character = null;
    public Transform MyTr;
    public Camera cam;
    private float MaxSize = 3f;
    public static bool LockPosition = false;
    public static int Winner = -1;
    float CamSize;
    //카메라 사이즈는 세로길이 / 2f
    // Use this for initialization
    void Reset()
    {
    }
    void Start()
    {
        LockPosition = false;
        MyTr = transform;
        cam = GetComponent<Camera>();
        CamSize = (Mathf.Abs(Character[0].position.x - Character[1].position.x) + 2.5f) / 16 * 9 / 2;

        StartCoroutine(MoveCamPos());
    }
    // Update is called once per frame
    void LateUpdate () {

        cam.orthographicSize = CamSize;
        if (cam.orthographicSize < MaxSize)
            cam.orthographicSize = MaxSize;        

    }

    IEnumerator MoveCamPos()
    {
        Vector3 CamPos = MyTr.position;
        float Multiple = 1f;
        while (true)
        {
            while (!LockPosition)
            {
                CamPos = MyTr.position;
                CamSize = (Mathf.Abs(Character[0].position.x - Character[1].position.x) + 2.5f) / 16 * 9 / 2 * Multiple;

                CamPos.x = Mathf.Lerp(Character[0].position.x, Character[1].position.x, 0.5f);
                MyTr.position = CamPos;
                yield return new WaitForEndOfFrame();
            }
            Multiple = 2f;
            CamSize = (Mathf.Abs(Character[0].position.x - Character[1].position.x) + 2.5f) / 16 * 9 / 2 * Multiple;
            while (LockPosition)
            {
                CamPos = MyTr.position;
                CamSize = (Mathf.Abs(Character[0].position.x - Character[1].position.x) + 2.5f) / 16 * 9 / 2 * Multiple;
                MyTr.position = CamPos;
                yield return new WaitForEndOfFrame();
            }
            float OneFrame = 0.01f;
            for (int i = 0; i < 100; i++)
            {
                CamPos.x = Mathf.Lerp(Character[0].position.x, Character[1].position.x, 0.5f);
                if(i < 50)
                Multiple -= 2*OneFrame;
                CamSize = (Mathf.Abs(Character[0].position.x - Character[1].position.x) + 2.5f) / 16 * 9 / 2 * Multiple;

                Vector3 Pos = MyTr.position;
                MyTr.position = Vector3.Lerp(Pos, CamPos, i * OneFrame);
                yield return null;
            }

            //
        }
    }

}
