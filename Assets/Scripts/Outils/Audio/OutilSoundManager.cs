using System.ComponentModel;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class OutilSoundManager : MonoBehaviour
{
    public static OutilSoundManager instance;
    [SerializeField] private bool debug = true;
    private bool lastState;

    void Awake()
    {
#if !UNITY_EDITOR
    Destroy(gameObject);
    return;
#endif
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        lastState = false;
        AudioSource[] audioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.GetComponent<SoundOutil>() == null)
                audioSource.gameObject.AddComponent<SoundOutil>();
        }
    }
    private void FixedUpdate()
    {
        if (lastState != debug)
        {
            lastState = debug;

            SoundOutil[] scripts = FindObjectsByType<SoundOutil>(FindObjectsSortMode.None);

            foreach (SoundOutil script in scripts)
            {
                script.enabled = debug;
            }
        }
    }
}
