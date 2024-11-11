using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public AudioClipsSO menuMusicData;
    public AudioClipsSO gameMusicData;
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
        PlayMenuMusic();
    }

    private void ConfigureAudioSource(AudioClipsSO clipData)
    {
        audioSource.clip = clipData.clip;
        audioSource.outputAudioMixerGroup = clipData.mixerGroup;
        audioSource.volume = clipData.volume;
        audioSource.pitch = clipData.pitch;
        audioSource.loop = true;
    }

    public void PlayMenuMusic()
    {
        ConfigureAudioSource(menuMusicData);
        audioSource.Play();
    }

    public void PlayGameMusic()
    {
        ConfigureAudioSource(gameMusicData);
        audioSource.Play();
    }

    public void StopAllMusic()
    {
        audioSource.Stop();
    }
}