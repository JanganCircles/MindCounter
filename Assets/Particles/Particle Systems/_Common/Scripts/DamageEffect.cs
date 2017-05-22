using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageEffect : MonoBehaviour {

    public Image[] Dmgimg;
    public Image MissImg;
    public Image GuardImg;
    private Vector3 PrevPos;
    public enum TargetImage
    {
        DMG,MISS,GUARD
    }
         
    private void Awake()
    {
        
        PrevPos = transform.position;
        for (int i = 0; i < Dmgimg.Length; i++)
        {
            Dmgimg[i].color = Color.white;
            Dmgimg[i].gameObject.SetActive(false);
        }
        MissImg.color = Color.white;
        MissImg.gameObject.SetActive(false);
        GuardImg.color = Color.white;
        GuardImg.gameObject.SetActive(false);
    }
    // Use this for initialization
	// Use this for initialization
    public void OnDamage(object img)
    {
        Debug.Log("ㄴㄴ");
        Image[] ImgArr = null;
        switch ((TargetImage)img)
        {
            case TargetImage.DMG:
                ImgArr = Dmgimg;
                break;
            case TargetImage.MISS:
                ImgArr = new Image[1] { MissImg };
                break;
            case TargetImage.GUARD:
                ImgArr = new Image[1] { GuardImg };
                break;
            default:
                break;
        }
        for (int i = 0; i < ImgArr.Length; i++)
        {
            ImgArr[i].color = Color.white;
            ImgArr[i].gameObject.SetActive(true);
            StartCoroutine(ActiveClose(ImgArr));
        }
    }
    IEnumerator ActiveClose(Image[] ImgArr)
    {
        Color c;
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 20);
            yield return null;
        }
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 20);
            for (int j = 0; j < ImgArr.Length; j++)
            {
                c = ImgArr[j].color;
                c.a = 1 - i / 1;
                ImgArr[j].color = c;
            }
            yield return null;
        }
        transform.position = PrevPos;
        for (int i = 0; i < ImgArr.Length; i++)
        {
            ImgArr[i].gameObject.SetActive(false);
        }
    }
    private void Update()
    {
    }

}
