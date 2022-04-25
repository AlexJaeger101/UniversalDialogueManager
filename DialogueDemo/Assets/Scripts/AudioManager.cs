using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource mAS;
    public bool mIsPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        mAS = GetComponent<AudioSource>();
    }

    public void SpeakingOnLoop(AudioClip[] clips)
    {
        StartCoroutine(SpeakRoutine(clips));
    }

    IEnumerator SpeakRoutine(AudioClip[] clips)
    {
        mIsPlaying = true;
        while(mIsPlaying)
        {
            mAS.Stop();
            int rand = Random.Range(0, clips.Length - 1);
            mAS.clip = clips[rand];
            mAS.Play();

            while(mAS.isPlaying)
            {
                yield return null;
            }
        }
    }
}
