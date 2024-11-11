using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    public AudioMixerGroup sfxAudioMixerGroup;
    private AudioSource audioSource;
    [Header("Lara Croft Sounds")]
    public AudioClip LaraCroftWalkSound;
    public AudioClip LaraCroftRunSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxAudioMixerGroup;
        audioSource.loop = true;
    }
    public void PlayWalkingSound()
    {
        if (!audioSource.isPlaying || audioSource.clip != LaraCroftWalkSound)
        {
            audioSource.clip = LaraCroftWalkSound;
            audioSource.Play();
            audioSource.pitch = 0.7f; 
        }
    }

    public void PlayRunningSound()
    {
        if (!audioSource.isPlaying || audioSource.clip != LaraCroftRunSound)
        {
            audioSource.clip = LaraCroftRunSound;
            audioSource.Play();
            audioSource.pitch = 3f; 
        }
    }

    public void StopFootstepsSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
