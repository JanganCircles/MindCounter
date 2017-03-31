using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageCalculator : MonoBehaviour {

    public const int MULTIPLE = 0;
    public const int PLUS = 100;
    public const string MULTIPLE_s = "Multiple";
    public const string PLUS_s = "Plus";
    public static DamageCalculator ins = null;
    public int TempDamage;
    public float[] PlusDamage;
    public Dictionary<string, int> StringToIndex;
    // Use this for initialization

    /*
     * 모든 데미지 연산은 여기에 들어와야함.
     * TempDamage = 기본데미지
     *
     */
    void Awake()
    {
        PlusDamage = new float[200];
        for (int i = 0; i < 200; i++)
            PlusDamage[i] = float.MaxValue;
        DamageReset();
        if (ins == null)
            ins = this;
        StringToIndex = new Dictionary<string, int>();
    }
    public void SetDamage(int damage)
    {
        TempDamage = damage;
    }
    public void AddDamage(string Type,float Damage,string DamageName)
    {
        int indexer_0 = 0;
        int indexer_1 = 0;
        switch (Type)
        {
            case "Multiple":
                indexer_0 = MULTIPLE;
                break;
            case "Plus":
                indexer_0 = PLUS;
                break;
            default:
                indexer_0 = -1;
                break;
        }
        indexer_1 = GetIndex(indexer_0);
        PlusDamage[indexer_1] = Damage;
        if (StringToIndex.ContainsKey(DamageName))
        {
            StringToIndex[DamageName] = indexer_1;
        }
        else
            StringToIndex.Add(DamageName, indexer_1);
    }
    void DamageReset()
    {
        TempDamage = 0;
        for (int i = 0; i < 200; i++)
        {
            PlusDamage[i] = float.MaxValue;
        }

    }
    int GetIndex(int index)
    {
        for (int i = index; i < 200; i++)
        {
            if (PlusDamage[i] == float.MaxValue)
                return i;
        }
        return -1;
    }
    public int Calculate()//이거쓰면됨, 최종데미지 나옴
    {
        Debug.Log(TempDamage);
        int Result = TempDamage;
        int PlusIndex = GetIndex(PLUS);
        int MultipleIndex = GetIndex(MULTIPLE);
        for (int i = PLUS; i < PlusIndex; i++)
        {
            Debug.Log((int)PlusDamage[i] + " + ");
            Result += (int)PlusDamage[i];
        }
        float ResultMultiple = 1;
        for (int i = MULTIPLE; i < MultipleIndex; i++)
        {
            Debug.Log(PlusDamage[i] + " * ");
            ResultMultiple *= PlusDamage[i];
        }
        Result = (int)((float)Result * ResultMultiple);
        DamageReset();
        return Result;

    }
}
