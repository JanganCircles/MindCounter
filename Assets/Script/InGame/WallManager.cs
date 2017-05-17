using UnityEngine;
using System.Collections;

public class WallManager : MonoBehaviour {

    public static WallManager ins;
    public int WallPivot;
    public int PivotMove;
    public const int WallMax = 1200;
    public float DashPivotX;
    // Use this for initialization
    void Reset()
    {
        WallPivot = WallMax / 2;
    }
     void Awake () {
        ins = this;
        PivotMove = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        DashPivotX = WallManager.ins.WallPivot - (WallManager.WallMax / 2);
    }
    public void ResetPivot()
    {
        PivotMove = 0;
    }
    public void Move(int num)
    {
        PivotMove = num;
    }
    public void Move(int num, int Player)
    {
        switch (Player)
        {
            case gameManager.CHALLANGER:
                {
                    PivotMove += num;
                }
                break;
            case gameManager.CHAMPION:
                {
                    PivotMove -= num;
                }
                break;
        }
    }
    public void SetPivot()
	{
		WallPivot += PivotMove;
		PivotMove = 0;
		if (WallPivot > WallMax)
			WallPivot = WallMax;
		else if (WallPivot < 0)
			WallPivot = 0;
		gameManager.ins.UserStatus [gameManager.CHAMPION].WallDistance = WallPivot;
		gameManager.ins.UserStatus [gameManager.CHALLANGER].WallDistance = WallMax - WallPivot;
		DashPivotX = WallManager.ins.WallPivot - (WallManager.WallMax / 2);

		for (int i = 0; i < 2; i++) {
			string ProgressBarName = (i == 0 ? "Champion" : "Challanger") + "Distance";
			Vector2 DistanceBar = new Vector2 ();
			DistanceBar.x = gameManager.ins.UserStatus [i].WallDistance;
			DistanceBar.y = WallMax;
			UIProgressBar.SetData (ProgressBarName, DistanceBar);
		}
	}
}
