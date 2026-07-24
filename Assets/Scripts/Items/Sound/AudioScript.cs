using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip consumeSound;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    public void PlayConsumeSound()
    {
        if (consumeSound == null)
            return;

        GameObject emitter = new GameObject("ConsumeSound");

        AudioSource newSource = emitter.AddComponent<AudioSource>();

        newSource.clip = consumeSound;
        newSource.volume = audioSource.volume;
        newSource.spatialBlend = audioSource.spatialBlend;
        newSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;

        newSource.Play();

        Destroy(emitter, consumeSound.length);
    }
}