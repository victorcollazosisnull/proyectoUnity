using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("AudioManager Settings")]
    public static AudioManager instance;
    public AudioMixer audioMixer;

    private static float currentMasterVolume = 0.6f;
    private static float currentMusicVolume = 0.6f;
    private static float currentSFXVolume = 0.6f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            LoadAudioSettings(); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void LoadAudioSettings()
    {
        SetMasterVolume(currentMasterVolume);
        SetMusicVolume(currentMusicVolume);
        SetSFXVolume(currentSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        currentMasterVolume = volume; 
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20); 
    }

    public void SetMusicVolume(float volume)
    {
        currentMusicVolume = volume; 
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20); 
    }

    public void SetSFXVolume(float volume)
    {
        currentSFXVolume = volume; 
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20); 
    }

    public void ResetAudioSettings()
    {
        currentMasterVolume = 0.6f;
        currentMusicVolume = 0.6f;
        currentSFXVolume = 0.6f;
        LoadAudioSettings(); 
    }
}