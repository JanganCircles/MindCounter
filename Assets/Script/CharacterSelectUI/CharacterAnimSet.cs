using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CharacterAnimSet : MonoBehaviour {

    SkeletonAnimation sklAnim;
    public SkeletonDataAsset[] Assets;
    int Index;
    int TempCharacter;
	// Use this for initialization
	void Start () {
        Index = name[0] == 'B' ? 0 : 1;
        sklAnim = GetComponent<SkeletonAnimation>();

    }
    // Update is called once per frame
	void Update () {
        if (GameData.ins.PlayerCharacter[Index] != 0 && GameData.ins.PlayerCharacter[Index] != TempCharacter)
        {
            sklAnim.skeletonDataAsset = Assets[GameData.ins.PlayerCharacter[Index] - 1];
            sklAnim.Initialize(true);
        }
        else if(GameData.ins.PlayerCharacter[Index] == 0)
        {
            sklAnim.skeletonDataAsset = Assets[Random.Range(0, 4)];
            sklAnim.Initialize(true);
        }
        TempCharacter = GameData.ins.PlayerCharacter[Index];

    }
}
