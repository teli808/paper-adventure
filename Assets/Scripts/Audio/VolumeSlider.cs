using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,
        MUSIC,
        SFX
    }

    [Header("Type")]

    [SerializeField] VolumeType volumeType;

    Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.Instance.MasterVolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = AudioManager.Instance.MusicVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.Instance.SFXVolume;
                break;
            default:
                Debug.LogWarning("Volume type not supported");
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.Instance.MasterVolume = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                AudioManager.Instance.MusicVolume = volumeSlider.value;
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SFXVolume = volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Volume type not supported");
                break;
        }
    }
}
