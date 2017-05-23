using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    private AudioSource ThisSource;
    public AudioClip[] ClipList;
    private Dictionary<string, AudioClip> dicClip;
    public static SoundManager ins = null;
	// Use this for initialization
	void Awake () {
        if (ins == null)
        {
            DontDestroyOnLoad(gameObject);
            ins = this;
        }
        else
        {
            AddClip(ClipList);
            Destroy(gameObject);
        }
        ThisSource = GetComponent<AudioSource>();

        dicClip = new Dictionary<string, AudioClip>();
        for (int i = 0; i < ClipList.Length; i++)
        {
            dicClip.Add(ClipList[i].name, ClipList[i]);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddClip(AudioClip clip)
    {
        if (!dicClip.ContainsKey(clip.name))
        {
            dicClip.Add(clip.name, clip);
        }
    }
     static void AddClip(AudioClip[] clips)
    {
        for(int i = 0; i < clips.Length;i++)
            SoundManager.ins.AddClip(clips[i]);
    }
    public void RunAudio(string Name)
    {
        if (dicClip.ContainsKey(Name)) 
        ThisSource.PlayOneShot(dicClip[Name]);

    }
}
