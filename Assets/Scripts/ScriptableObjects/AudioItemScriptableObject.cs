using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioItem", menuName = "Audio/AudioItem")]
public class AudioItemScriptableObject : ScriptableObject
{
    public AudioClip[] audioClips;
    public float volume = 1f;
    public float minPitch = 1f;
    public float maxPitch = 1f;

    public AudioItem ToAudioItem()
    {
        return new AudioItem
        {
            audioClips = audioClips,
            volume = volume,
            minPitch = minPitch,
            maxPitch = maxPitch,
        };
    }
}
