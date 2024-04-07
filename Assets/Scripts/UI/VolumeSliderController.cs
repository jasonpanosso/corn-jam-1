using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSliderController : MonoBehaviour
{
    private Slider slider;

    public void Awake()
    {
        slider = GetComponent<Slider>();
        var savedVolume = Settings.VolumeLevel;
        slider.value = savedVolume;
    }

    public void OnVolumeSliderChange()
    {
        Settings.VolumeLevel = slider.value;
    }
}
