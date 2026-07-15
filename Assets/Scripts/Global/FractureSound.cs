using UnityEngine;

public class FractureSound : MonoBehaviour
{
    public AudioClip breakSound;
    private float lastPlayTime;
    private const float Cooldown = 0.15f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1.5f && Time.time > lastPlayTime + Cooldown)
        {
            lastPlayTime = Time.time;

            float volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 8f);

            AudioSource.PlayClipAtPoint(breakSound, collision.contacts[0].point, volume);
        }
    }
}
