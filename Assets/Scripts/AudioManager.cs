using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioClip footstepAudio;
    void Awake()
    {
        instance = this;
    }
    public void FootStepAudioAudio()
    {
        audioSource.clip = footstepAudio;
        audioSource.Play();
        audioSource.loop=true;
    }
}
