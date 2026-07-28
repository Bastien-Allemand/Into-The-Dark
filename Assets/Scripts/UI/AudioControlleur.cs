using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        uiSlider.onValueChanged.AddListener(SetUIVolume);
    }

    void SetMasterVolume(float value)
    {
        mixer.SetFloat("Master", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat("Music", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
    }

    void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
    }

    void SetUIVolume(float value)
    {
        mixer.SetFloat("Menu", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
    }
}