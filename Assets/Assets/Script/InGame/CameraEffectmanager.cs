using UnityEngine;
using System.Collections;

public class CameraEffectmanager : MonoBehaviour {
    public static CameraEffectmanager ins;
	Vector3 self;
	Vector3 movv;
    public int Power;
    public float Time;
    // Use this for initialization
    void Awake()
    {
        ins = this;
        self = GetComponent<Transform>().position;
    }
    void Start ()
    {
    }
    void Reset()
    {
        Power = 10;
        Time = 0.02f;
    }
    public void MoveCam(Vector3 v)
    {
        transform.position = transform.position + v;
    }
    public void MoveSmoothsCam(Vector3 v)
    {
    }
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Q)) { //테스트 한다고 써놓은것들
			StartCoroutine( ShakeCam (Power, Time));
		}
		if (Input.GetKeyDown(KeyCode.W) ) {
			StartCoroutine( ShakeCam (Power * 5, Time));
		}
		if (Input.GetKeyDown(KeyCode.E) ) {
			StartCoroutine( ShakeCam (Power * 10, Time));
        }
        if (self.x != transform.position.x)
        {
            Vector3 v = Vector3.zero;
            v.x = transform.position.x;
            self = v;
        }

    }
    public void Shake(int Power)
    {
        StartCoroutine(ShakeCam(Power, Time));
    }
	IEnumerator ShakeCam(float dmg, float shTime){
		//self = GetComponent<Transform> ().position;
		int count;
		if (dmg <= 30) {
			 count = 3;
		} else if (dmg <= 80) {
			 count = 4;
		} else {
			 count = 7;
		}
		dmg = dmg * 0.01f;
		float randir = Random.Range (1.1f, 4.9f);
		int dir = (int)randir / 1;
		switch (dir) {
		case 1: 
			for (int i = 0; i < count; i++) {
				float ran1 = Random.Range (dmg - 0.2f, dmg);
				float ran2 = Random.Range (dmg - 0.2f, dmg);

				movv = new Vector3 (ran1, ran2, 0);
				GetComponent<Transform> ().position += movv;
				yield return new WaitForSeconds (shTime);
				GetComponent<Transform> ().position = self;

				GetComponent<Transform> ().position -= movv;
				yield return new WaitForSeconds (shTime);
				GetComponent<Transform> ().position = self;
				if (count == 3)
					dmg -= 0.03f;
				else if (count == 5)
					dmg -= 0.1f;
				else
					dmg -= 0.17f;
			}
			break;
		case 2:
			for (int i = 0; i < count; i++) {
				float ran1 = Random.Range (dmg - 0.2f, dmg);
				float ran2 = Random.Range (dmg - 0.2f, dmg);
				movv = new Vector3 (ran1, -ran2, 0);
				GetComponent<Transform> ().position += movv;
				yield return new WaitForSeconds (shTime);
				GetComponent<Transform> ().position = self;
				GetComponent<Transform> ().position -= movv;
				yield return new WaitForSeconds (shTime);
				GetComponent<Transform> ().position = self;
				if (count == 3)
					dmg -= 0.03f;
				else if (count == 5)
					dmg -= 0.1f;
				else
					dmg -= 0.17f;
			}
			break;
		case 3:
			for (int i = 0; i < count; i++) {
				float ran1 = Random.Range (dmg - 0.2f, dmg);
				float ran2 = Random.Range (dmg - 0.2f, dmg);
				movv = new Vector3 (-ran1, -ran2, 0);
				GetComponent<Transform> ().position += movv; // 올려주고
				yield return new WaitForSeconds (shTime); // 기다리고
				GetComponent<Transform> ().position = self; // 다시 돌려주고
				GetComponent<Transform> ().position -= movv; //내려주고
				yield return new WaitForSeconds (shTime); // 기다리고
				GetComponent<Transform> ().position = self; // 다시돌려주고
				if (count == 3)
					dmg -= 0.03f;
				else if (count == 5)
					dmg -= 0.1f;
				else
					dmg -= 0.17f;
			}
			break;
		case 4:
			for (int i = 0; i < count; i++) {
				float ran1 = Random.Range (dmg - 0.2f, dmg);
				float ran2 = Random.Range (dmg - 0.2f, dmg);
				movv = new Vector3 (-ran1, ran2, 0);
				GetComponent<Transform> ().position += movv;
				yield return new WaitForSeconds (shTime);
				GetComponent<Transform> ().position = self;
				GetComponent<Transform> ().position -= movv;
				yield return new WaitForSeconds (shTime);
				GetComponent<Transform> ().position = self;
				if (count == 3)
					dmg -= 0.03f;
				else if (count == 5)
					dmg -= 0.1f;
				else
					dmg -= 0.17f;
			}
			break;
        }
        self = GetComponent<Transform>().position;
    }
}

public class SHAKEPOWER
{
    public const int SMALL = 10;
    public const int MIDDLE = 50;
    public const int BIG = 100;
}