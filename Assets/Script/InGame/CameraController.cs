using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Transform[] Character = null;
    public Transform MyTr;
    public Camera cam;
    private float MaxSize = 3f;
    // Use this for initialization
    void Start()
    {
        MyTr = transform;
        cam = GetComponent<Camera>();
        Character = new Transform[2];
        for (int i = 0; i < 2; i++)
        {
            Character[i] = gameManager.ins.UserStatus[i].transform;
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        float CamSize = Character[0].position.x - Character[1].position.x;
        CamSize = Mathf.Abs(CamSize);
        cam.orthographicSize = CamSize;
        if (cam.orthographicSize < MaxSize)
            cam.orthographicSize = MaxSize;

        Vector3 CamPos = MyTr.position;
        CamPos.x = Mathf.Lerp(Character[0].position.x,Character[1].position.x,0.5f);
        MyTr.position = CamPos;

    }
}
