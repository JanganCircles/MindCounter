using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour {

    public AudioSource MusicSource;
    public float[] Samples = new float[512];
    public float[] _freqBand = new float[8];
    // Use this for initialization
    void Reset()
    {
        Samples = new float[512];
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AudioListener.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
        SetSpectrumData();
    }
    void SetSpectrumData()
    {
        int count = 0;
        for(int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += Samples[count] * (count + 1);
                count++;
            }
            average /= count;
            _freqBand[i] = average * 10;
        }
    }
}
