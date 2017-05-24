using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManuSoundClipRun : MonoBehaviour {
    public AudioClip[] Musics;
    public int index = 0;
    public AudioClip TempClip;
    public AudioSource source;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        StartCoroutine(MusicRandomRunning());
    }
    IEnumerator MusicRandomRunning()
    {
        while (true)
        {

            index = Random.Range(0, Musics.Length);
            TempClip = Musics[index];
            source.clip = TempClip;
            source.Play();
            source.volume = 0.5f;
            yield return new WaitForSeconds(source.clip.length);

        }
    }
}
