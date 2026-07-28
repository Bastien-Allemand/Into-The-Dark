using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void SetAmbientVolume(float volume)
    {
        mixer.SetFloat("AmbientVolume", Mathf.Log10(volume) * 20);
    }
    public void SetUIVolume(float volume)
    {
        mixer.SetFloat("UIVolume", Mathf.Log10(volume) * 20);
    }
}