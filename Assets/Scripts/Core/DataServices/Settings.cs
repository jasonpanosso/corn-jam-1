using UnityEngine;

public static class Settings
{
    private const float defaultVolumeLevel = 1.0f;
    private const string volumeKey = "VolumeLevel";

    public static float VolumeLevel
    {
        get { return PlayerPrefs.GetFloat(volumeKey, defaultVolumeLevel); }
        set
        {
            PlayerPrefs.SetFloat(volumeKey, value);
            PlayerPrefs.Save();
            ServiceLocator.AudioManager.UpdateGlobalVolume(value);
        }
    }
}
