using UnityEngine;

[System.Serializable]
public class AudioItem
{
    public AudioClip[] audioClips;
    public float volume = 1f;
    public float minPitch = 1f;
    public float maxPitch = 1f;

    public AudioClip GetRandomAudioClip()
    {
        if (audioClips.Length == 0)
            return null;

        int index = Random.Range(0, audioClips.Length);
        return audioClips[index];
    }
}
