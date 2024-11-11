using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public AudioMixerGroup musicAudioMixerGroup;
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = musicAudioMixerGroup;
        PlayMenuMusic();
    }
    public void PlayMenuMusic()
    {
        if (audioSource.clip != menuMusic)
        {
            audioSource.Stop();
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    public void PlayGameMusic()
    {
        if (audioSource.clip != gameMusic)
        {
            audioSource.Stop();
            audioSource.clip = gameMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    public void StopAllMusic()
    {
        audioSource.Stop();
    }
}