using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utillity : MonoBehaviour {

    public static Utillity instance;

    public static WaitForFixedUpdate WaitFixUp = new WaitForFixedUpdate();
    public const int REDPLAYER  = 0;
    public const int BLUEPLAYER = 1;
    public const int ENEMYBULLET = 2;
    public const int NEUTRALITY = 3;
    public static WaitForSeconds secDotOne = new WaitForSeconds(0.05f);
    public static WaitForSeconds secOne = new WaitForSeconds(1f);
    public const float FIREDELAYONE = 0.028f;
	public const float SPEEDONE = 10f;
    public const float ITEMMOVESPEED = 850f;
    public const int HITPOINTONE = 50;
    public const float SCREENTOP = 1080 / 2f;
    public const float SCREENBOTTOM = 1080 / -2f;
    public const float SCREENRIGHT = 1920 / 2f;
    public const float SCREENLEFT = 1920 / -2f;
    public static Sprite[] BULLETIMAGE = Resources.LoadAll<Sprite>("Sprite/Bullet/Bulletimage");

    static public int GetSign(float Num)
    {
        if (Num == 0) return 0;
        return (int)(Num / Mathf.Abs(Num));
    }
    static public float getAngle(Vector3 A, Vector3 B)
    {
        float dx = B.x - A.x;
        float dy = B.y - A.y;

        float rad = Mathf.Atan2(dx, dy);
        float degree = (rad * 180) / Mathf.PI;
        return degree;
    }
    static public void Lookat(Transform Temp, Vector3 Pos)
    {
        Vector2 TempDirection;
        TempDirection.x = Temp.position.x - Pos.x;
        TempDirection.y = Temp.position.y - Pos.y;
        float Angle;
        Angle = -Utillity.getAngle(Vector3.zero, TempDirection);
        Temp.rotation = Quaternion.AngleAxis(Angle - 0.1f, Vector3.forward);
        Temp.Rotate(Vector3.back, 180);
    }
    static public void Lookat(Transform Temp, Transform Target)
    {
        Vector2 TempDirection;
        TempDirection.x = Temp.position.x - Target.position.x;
        TempDirection.y = Temp.position.y - Target.position.y;
        float Angle;
        Angle = -Utillity.getAngle(Vector3.zero, TempDirection);
        Temp.rotation = Quaternion.AngleAxis(Angle - 0.1f, Vector3.forward);
        Temp.Rotate(Vector3.back, 180);
    }
    static public float Lerp(float a, float b, float c)
    {
        float result = (b - a) * c + a;
        return result;
    }
    static public float Lerp2(float a, float b, float c)
    {
        float result = (1 - c) * a + c * b;
        return result;
    }
    static public void ChangeBulletImage(ref int TempIndex, int ChangeIndex)
    {
        if (BulletImgWeight(TempIndex) < BulletImgWeight(ChangeIndex))
        {
            TempIndex = ChangeIndex;
        }
    }
    static private int BulletImgWeight(int num)
    {
        int result = -1;
        switch (num)
        {
            case 0: result = 0; break;
            case 1: result = 9; break;
            case 2: result = 7; break;
            case 3: result = 10; break;
            case 4: result = 8; break;
            case 5: result = 11; break;
            case 6: result = 6; break;
            case 7: result = 12; break;
            case 8: result = 5; break;
            case 9: result = 2; break;
            case 10: result = 1; break;
            case 11: result = 4; break;
            case 12: result = 3; break;
        }
        return result;
    }
    static public void BulletImageSetting(int team, SpriteRenderer render,int Index)
    {
        int Red = 1;
        int Normal = 0;
        int Blue = 5;
        if (team == BLUEPLAYER)
        {
            render.sprite = BULLETIMAGE[Blue + (Index*16)];
        }
        else if (team == REDPLAYER)
        {
            render.sprite = BULLETIMAGE[Red + (Index * 16)];
        }
        else
        {
            render.sprite = BULLETIMAGE[Normal + (Index * 16)];
        }
    }
    static public int Pow3(int i)
    {
        if (i == 0)
            return 1;
        else
        {
            return 2 * Pow3(i - 1);
        }
    }
    static public string GetStr(int i)
    {
        if (i == REDPLAYER)
            return "Red";
        else
            return "Blue";
    }
}
