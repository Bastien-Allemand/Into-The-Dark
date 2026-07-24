using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    public void PlaySound(AudioClip clip, float duration)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.playOnAwake = false;
        source.Play();

        Destroy(gameObject, duration);
    }
}