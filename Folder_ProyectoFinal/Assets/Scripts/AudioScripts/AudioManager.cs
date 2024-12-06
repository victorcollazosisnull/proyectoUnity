using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("AudioManager Settings")]
    public AudioMixer audioMixer;
    private AudioSource audioSource;
    private static bool isMasterMuted = false;
    private static bool isMusicMuted = false;
    private static bool isSFXMuted = false;

    private static float currentMasterVolume = 0.6f;
    private static float currentMusicVolume = 0.6f;
    private static float currentSFXVolume = 0.6f;

    public static event Action OnMasterMute;
    public static event Action OnMasterUnmute;
    public static event Action OnMusicMute;
    public static event Action OnMusicUnmute;
    public static event Action OnSFXMute;
    public static event Action OnSFXUnmute;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        LoadAudioSettings();
        UnmuteMusicVolume(); 
    }

    private void LoadAudioSettings()
    {
        SetMasterVolume(currentMasterVolume);
        if (isMusicMuted)
        {
            MuteMusicVolume();
        }
        else
        {
            SetMusicVolume(currentMusicVolume);
        }

        if (isSFXMuted)
        {
            MuteSFXVolume();
        }
        else
        {
            SetSFXVolume(currentSFXVolume);
        }
    }

    public void SetMasterVolume(float volume)
    {
        if (isSFXMuted)
        {
            return;
        }
        currentMasterVolume = volume;
        audioMixer.SetFloat("Master", volume > 0 ? Mathf.Log10(volume) * 20 : -80f);
    }

    public void SetMusicVolume(float volume)
    {
        if (isSFXMuted)
        {
            return;
        }
        currentMusicVolume = volume;
        audioMixer.SetFloat("Music", volume > 0 ? Mathf.Log10(volume) * 20 : -80f);
    }

    public void SetSFXVolume(float volume)
    {
        if (isSFXMuted)
        {
            return;
        }
        currentSFXVolume = volume;
        audioMixer.SetFloat("SFX", volume > 0 ? Mathf.Log10(volume) * 20 : -80f);
    }

    public void ResetAudioSettings()
    {
        currentMasterVolume = 0.6f;
        currentMusicVolume = 0.6f;
        currentSFXVolume = 0.6f;

        isMasterMuted = false;
        isMusicMuted = false;
        isSFXMuted = false;

        LoadAudioSettings();
    }

    public void MuteMasterVolume()
    {
        isMasterMuted = true;
        audioMixer.SetFloat("Master", -80f);
        OnMasterMute?.Invoke();
    }

    public void UnmuteMasterVolume()
    {
        isMasterMuted = false;
        SetMasterVolume(currentMasterVolume);
        OnMasterUnmute?.Invoke();
    }

    public void MuteMusicVolume()
    {
        isMusicMuted = true;
        audioMixer.SetFloat("Music", -80f);
        OnMusicMute?.Invoke();
    }

    public void UnmuteMusicVolume()
    {
        isMusicMuted = false;
        SetMusicVolume(currentMusicVolume);
        OnMusicUnmute?.Invoke();
    }

    public void MuteSFXVolume()
    {
        isSFXMuted = true;
        audioMixer.SetFloat("SFX", -80f);
        OnSFXMute?.Invoke();
    }

    public void UnmuteSFXVolume()
    {
        isSFXMuted = false;
        SetSFXVolume(currentSFXVolume);
        OnSFXUnmute?.Invoke();
    }
}