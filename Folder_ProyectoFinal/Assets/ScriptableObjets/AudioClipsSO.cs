using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewAudioClip", menuName = "Audio/AudioClip")]
public class AudioClipsSO : ScriptableObject
{
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;
    public float volume = 1f;
    public float pitch = 1f;
}
