using UnityEngine;

public class BackgroundAudioPlayer : MonoBehaviour
{
    private void OnEnable()
    {

        var worldType = ServiceLocator.LevelManager.CurrentLevel.worldType;
        var musicKey = $"Music_{worldType}";
        var ambienceKey = $"AMB_{worldType}";
        ServiceLocator.AudioManager.PlayAudioItem(musicKey);
        ServiceLocator.AudioManager.PlayAudioItem(ambienceKey);
    }
}
