using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    public void OnVolumeSliderChange()
    {
        ServiceLocator.AudioManager.UpdateGlobalVolume(GetComponent<Slider>().value);
    }
}
