using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundAudioPlayer : MonoBehaviour
{
    private void OnEnable()
    {
        // zz disgusting
        if (SceneManager.GetActiveScene().name.ToLower() == "menu")
            PlayMenuBackgroundAudio();
        else
            PlayLevelBackgroundAudio();
    }

    private void PlayMenuBackgroundAudio()
    {
        Debug.LogWarning("TODO: Menu background audio");
    }

    private void PlayLevelBackgroundAudio()
    {
        var worldType = ServiceLocator.LevelManager.CurrentLevel.worldType;
        var musicKey = $"Music_{worldType}";
        var ambienceKey = $"AMB_{worldType}";
        ServiceLocator.AudioManager.PlayAudioItem(musicKey);
        ServiceLocator.AudioManager.PlayAudioItem(ambienceKey);
    }
}
