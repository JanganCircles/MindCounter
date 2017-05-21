using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustrationChanger : MonoBehaviour {
    public string[] CharaName;
    public string[] PlanetName;
    public Sprite[] Image;
    public GameObject[] Objs;
    public bool Trigger;
    public int TempIndex;
    private int PrevIndex = 5;
    public Text Planettxt;
    public Text Nametxt;
    // Use this for initialization
    void Start () {
        StartCoroutine(ImageChanger());

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void TriggerOn()
    {
        Trigger = true;
    }
    IEnumerator ImageChanger()
    {
        while (true)
        {

            while(!Trigger)
            {
                yield return null;
            }
            Trigger = false;
            Image OutImage = Objs[0].GetComponent<Image>();
            Image inImage = Objs[1].GetComponent<Image>();

            do
            {
                TempIndex = Random.Range(0, Image.Length);
            } while (PrevIndex == TempIndex);
            PrevIndex = TempIndex;
            inImage.sprite = Image[TempIndex];
            Nametxt.text = CharaName[TempIndex];
            Planettxt.text = PlanetName[TempIndex];
            
            Color c1 = OutImage.color;
            Color c2 = inImage.color;
            Vector3[] startPos = new Vector3[2] { Objs[0].transform.position, Objs[1].transform.position } ;
            Vector3[] EndPos = new Vector3[2] { Objs[1].transform.position, Objs[0].transform.position };

            for (float timer = 0; timer < 1; timer += Time.deltaTime * 4)
            {
                for (int i = 0; i < 2; i++)
                Objs[i].transform.position = Vector3.Lerp(startPos[i], EndPos[i], timer);
                c1.a = (1 - timer);
                c2.a = timer; 
                OutImage.color = c1;
                inImage.color = c2;
                
                yield return null;
            }
            Objs[0].transform.position = EndPos[0];
            Objs[1].transform.position = EndPos[1];
            GameObject gm = Objs[0];
            Objs[0] = Objs[1];
            Objs[1] = gm;
            yield return null;


        }
    }
}
