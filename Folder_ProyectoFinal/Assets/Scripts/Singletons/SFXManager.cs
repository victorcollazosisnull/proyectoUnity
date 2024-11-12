using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    private AudioSource audioSource;
    public AudioClipsSO walkSoundData;
    public AudioClipsSO runSoundData;

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

    private void ConfigureAudioSource(AudioClipsSO clipData)
    {
        audioSource.clip = clipData.clip;
        audioSource.outputAudioMixerGroup = clipData.mixerGroup;
        audioSource.volume = clipData.volume;
        audioSource.pitch = clipData.pitch;
        audioSource.loop = true;
    }

    public void PlayWalkingSound()
    {
        ConfigureAudioSource(walkSoundData);
        audioSource.Play();
    }

    public void PlayRunningSound()
    {
        ConfigureAudioSource(runSoundData);
        audioSource.Play();
    }

    public void StopFootstepsSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}