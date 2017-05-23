using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CharacterAnim : MonoBehaviour {

    private static CharacterAnim[] Anims = null;
    SkeletonAnimation SkelAnim;
    public string hit;
    public string strongAtk;
    public string middleAtk;
    public string weakAtk;
    public string guard;
    public string idle;
    public string land;
    public string mana;
    public string rush;
    public string slowRush;
    private string TempName;

    public SkeletonDataAsset[] Assets;
    // Use this for initialization
    public enum AnimStasis
    {
        HIT,LATK,MATK,SATK,GUARD,IDLE,LAND,MANA,RUSH,SRUSH
    }
    private void Reset()
    {
        hit = "hit";
        strongAtk = "Lat";
        middleAtk = "Mat";
        weakAtk = "Sat";
        guard = "guard";
        idle = "idle";
        land = "land";
        mana = "mana";
        rush = "rush";
        slowRush = "slow rush";
    }
    void Start () {
        if (Anims == null)
        {
            Anims = new CharacterAnim[2];
            Anims[0] = null;
            Anims[1] = null;
        }
        else if(Anims[0] && Anims[1])
        {
            Anims = new CharacterAnim[2];
            Anims[0] = null;
            Anims[1] = null;
        }
        int index = transform.parent.GetComponent<CharacterStatus>().Controller;
        SkeletonAnimation sklAnim = GetComponent<SkeletonAnimation>(); 
        int TempCharacter = GameData.ins.PlayerCharacter[index];
        sklAnim.skeletonDataAsset = Assets[GameData.ins.PlayerCharacter[index] - 1];
        sklAnim.Initialize(true);
        Anims[transform.parent.name == "Champion" ? 0 : 1] = this;
        SkelAnim =  gameObject.GetComponent<SkeletonAnimation>();
        TempName = idle;
    }
	
	// Update is called once per frame
	void Update () {
        SkelAnim.AnimationName = TempName;
    }
    public static void SetOrderinLayer()
    {
        if (gameManager.ins.Winner < 0) return;
        Anims[gameManager.ins.Winner].gameObject.GetComponent<MeshRenderer>().sortingOrder = 1; Anims[gameManager.ins.Winner].gameObject.GetComponent<MeshRenderer>().sortingOrder = 1;
        Anims[1 - gameManager.ins.Winner].gameObject.GetComponent<MeshRenderer>().sortingOrder = 0;

    }
    public static void ChangeAnimation(AnimStasis stasis)
    {
        for(int i = 0; i < 2; i++)
        ChangeAnimation(stasis, i);

    }
    public static void ChangeAnimation(AnimStasis stasis,float Scale)
    {
        for (int i = 0; i < 2; i++)
        {
            Anims[i].SetTimeScale(Scale);
            ChangeAnimation(stasis, i);
        }

    }
    public void SetTimeScale(float f)
    {
        SkelAnim.timeScale = f;
    }
    public static void ChangeAnimation(AnimStasis stasis,int index)
    {
        CharacterAnim ca = Anims[index];
        switch (stasis)
        {
            case AnimStasis.HIT:
                ca.TempName = ca.hit; break;
            case AnimStasis.LATK: ca.TempName = ca.strongAtk; break;
            case AnimStasis.MATK: ca.TempName = ca.middleAtk; break;
            case AnimStasis.SATK: ca.TempName = ca.weakAtk; break;
            case AnimStasis.GUARD: ca.TempName = ca.guard; break;
            case AnimStasis.IDLE: ca.TempName = ca.idle; break;
            case AnimStasis.LAND: ca.TempName = ca.land; break;
            case AnimStasis.MANA: ca.TempName = ca.mana; break;
            case AnimStasis.RUSH: ca.TempName = ca.rush; break;
            case AnimStasis.SRUSH: ca.TempName = ca.slowRush; break;
        }
    }
    public static float GetTempDuration(int index)
    {
        Debug.Log(Anims[index].SkelAnim.skeletonDataAsset.GetSkeletonData(true).FindAnimation(Anims[index].TempName).duration);
        return Anims[index].SkelAnim.skeletonDataAsset.GetSkeletonData(true).FindAnimation(Anims[index].TempName).duration;
    }
}
