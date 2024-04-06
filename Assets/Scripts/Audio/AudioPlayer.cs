using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public void Play(string sfxKey) => ServiceLocator.AudioManager.PlayAudioItem(sfxKey);
}
