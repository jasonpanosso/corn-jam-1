using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSliderController : MonoBehaviour
{
    private Slider slider;

    public void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = Settings.VolumeLevel;
    }

    public void OnVolumeSliderChange() => Settings.VolumeLevel = slider.value;
}
