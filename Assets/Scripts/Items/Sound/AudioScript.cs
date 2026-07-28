using UnityEngine;

public class AudioScript : MonoBehaviour
{
    private void Awake()
    {
    }

    public void PlaySound(AudioClip clip, AudioSource audioSource)
    {
        if (clip == null)
            return;

        GameObject emitter = new GameObject("Sound");

        AudioSource source = emitter.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = audioSource.volume;
        source.spatialBlend = audioSource.spatialBlend;
        source.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;

        source.Play();

        Destroy(emitter, clip.length);
    }
}