using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFooling : MonoBehaviour {

    public static ObjectFooling instance;

    protected Dictionary<string, GameObject> RunningData;
    protected Dictionary<string,GameObject> ReleasedData;
    protected List<string> RunningDataName;
    protected List<string> ReleasedDataName;
    protected int Name_Index;
    public int MakeNum;
    public string ObjectName;
    public GameObject BaseObject;
    public Transform ParentTr;
    public float SleepDataPersent = 0;

    virtual public void Awake()
    {
        instance = this;
        ParentTr = transform;
        Name_Index = 0;
        RunningDataName = new List<string>();
        ReleasedDataName = new List<string>();
        RunningData = new Dictionary<string, GameObject>();
        ReleasedData = new Dictionary<string, GameObject>();
        StartCoroutine("MakeSleepData");
        
    }
    void Start()
    {
        if (MakeNum > 0)
        {
            string[] str = new string[MakeNum];
            for (int i = 0; i < MakeNum; i++)
            {
                str[i] = GetData().name;
            }
            for (int i = 0; i < MakeNum; i++)
            {
                ReleaseData(str[i]);
            }

        }
    }
    virtual public GameObject GetData()
    {
        GameObject gm;
        if (ReleasedDataName.Count == 0)
        {
            gm = ObjectMake();
        }
        else
        {
            string st = ReleasedDataName[0];
            gm = ReleasedData[st];
            ReleasedData.Remove(st);
            ReleasedDataName.Remove(ReleasedDataName[0]);
            gm.SetActive(true);
        }
        RunningData.Add(gm.name,gm);
        RunningDataName.Add(gm.name);
        return gm;
    }
    virtual public bool ReleaseData(string Name)
    {
        GameObject gobj = FindRunningData(Name);
        if (gobj == null)
        {
            return false;
        }
        RunningDataName.Remove(gobj.name);
        RunningData.Remove(gobj.name);
        gobj.SetActive(false);
        ReleasedDataName.Add(gobj.name);
        ReleasedData.Add(gobj.name, gobj);
        return true;
    }
    public GameObject FindRunningData(string Name)
    {
        if (RunningData.ContainsKey(Name))
        {
            return RunningData[Name];
        }
        else
           return null;
    }
    protected GameObject ObjectMake()
    {
        GameObject gm = Instantiate(BaseObject, Vector3.zero, Quaternion.AngleAxis(0, Vector3.up)) as GameObject;
        gm.transform.parent = ParentTr;
        gm.name = ObjectName + Name_Index;
        Name_Index++;
        return gm;
    }
    IEnumerator MakeSleepData()
    {
        while (true)
        {
            if (RunningData.Count * SleepDataPersent > ReleasedData.Count)
            {
                GameObject gm = ObjectMake();
                RunningDataName.Add(gm.name);
                RunningData.Add(gm.name,gm);
                ReleaseData(gm.name);
            }
            yield return new WaitForSeconds(1f);
        }
    }

}
