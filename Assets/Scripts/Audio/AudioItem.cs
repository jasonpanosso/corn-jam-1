using UnityEngine;

[System.Serializable]
public class AudioItem
{
    public AudioClip[] audioClips;
    public float volume = 1f;
    public uint lowerSemitoneOffset = 0;
    public uint upperSemitoneOffset = 0;

    public AudioClip GetRandomAudioClip()
    {
        if (audioClips.Length == 0)
            return null;

        int index = Random.Range(0, audioClips.Length);
        return audioClips[index];
    }

    public float GetRandomPitch()
    {
        int semitoneOffset = Random.Range(-(int)lowerSemitoneOffset, (int)upperSemitoneOffset + 1);
        // 1 semitone = twelfth of an octave
        return Mathf.Pow(2f, semitoneOffset / 12f);
    }
}
