using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioSliddersScripts : MonoBehaviour
{
    public Slider sliderMaster;
    public Slider sliderMusic;
    public Slider sliderUI;
    public Slider sliderSFX;

    public TextMeshProUGUI textMaster;
    public TextMeshProUGUI textMusic;
    public TextMeshProUGUI textSFX;
    public TextMeshProUGUI textUI;

    private void Start()
    {
        sliderSFX.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        sliderMaster.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        sliderUI.onValueChanged.AddListener(AudioManager.Instance.SetUIVolume);
        sliderMusic.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
    }

    void Update()
    {
        textMaster.SetText($"{sliderMaster.value.ToString("N0")}");
        textSFX.SetText($"{sliderSFX.value.ToString("N0")}");
        textMusic.SetText($"{sliderMusic.value.ToString("N0")}");
        textUI.SetText($"{sliderUI.value.ToString("N0")}");
    }
}
