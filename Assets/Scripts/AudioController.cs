using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image headphoneIcon;
    [SerializeField] private Button headphoneButton;

    [SerializeField] private Color fullVolumeColor = Color.white;
    [SerializeField] private Color mutedColor = Color.gray;

    private float currentVolume;
    private float previousVolume;
    private bool isMuted = false;
    private const string MUSIC_VOLUME_PARAM = "MusicVolume";


    void Start()
    {
        volumeSlider.value = 0.9f;
        currentVolume = volumeSlider.value;
        UpdateMixerVolume(currentVolume);

        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
        headphoneButton.onClick.AddListener(ToggleMute);

        UpdateHeadphoneColor(currentVolume);
    }

    void OnVolumeSliderChanged(float value)
    {
        currentVolume = value;

        if (currentVolume != 0f)
        {
            isMuted = false;

            previousVolume = currentVolume;
        }

        UpdateMixerVolume(currentVolume);
        UpdateHeadphoneColor(currentVolume);
    }

    void ToggleMute()
    {
        if (!isMuted)
        {
            isMuted = true;
            previousVolume = currentVolume;
            currentVolume = 0f;
            volumeSlider.value = 0f;
        }
        else
        {
            isMuted = false;
            currentVolume = previousVolume;
            volumeSlider.value = currentVolume;
        }

        UpdateMixerVolume(currentVolume);
        UpdateHeadphoneColor(currentVolume);
    }

    void UpdateMixerVolume(float sliderValue)
    {
        float dB = sliderValue > 0 ? Mathf.Lerp(-80f, 0f, sliderValue) : -80f;
        audioMixer.SetFloat(MUSIC_VOLUME_PARAM, dB);
    }

    void UpdateHeadphoneColor(float sliderValue)
    {
        headphoneIcon.color = Color.Lerp(mutedColor, fullVolumeColor, sliderValue);
    }

    void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeSliderChanged);
        headphoneButton.onClick.RemoveListener(ToggleMute);
    }
}
