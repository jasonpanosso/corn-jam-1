using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioItemData", menuName = "Audio/AudioItemData")]
public class AudioItemData : ScriptableObject
{
    public AudioClip[] audioClips;
    public float volume = 1f;
    public uint lowerSemitoneOffset = 0;
    public uint upperSemitoneOffset = 0;
    public bool loop = false;

    public AudioItem ToAudioItem() =>
        new()
        {
            audioClips = audioClips,
            volume = volume,
            lowerSemitoneOffset = lowerSemitoneOffset,
            upperSemitoneOffset = upperSemitoneOffset,
            loop = loop
        };
}
