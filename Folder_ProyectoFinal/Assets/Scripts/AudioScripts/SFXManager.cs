using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

public class SFXManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClipsSO walkSoundData;
    public AudioClipsSO damageSoundData; 
    public AudioClipsSO deathSoundData;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
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

    public void StopFootstepsSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    public void PlayDamageSound()
    {
        ConfigureAudioSource(damageSoundData);
        audioSource.Play();
    }

    public void PlayDeathSound()
    {
        ConfigureAudioSource(deathSoundData);
        audioSource.Play();
    }
}