using UnityEngine;

public class RadioScript : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private string targetTag = "Ghost";
    private bool isActivated = false;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip radioSound;

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.clip = radioSound;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Ghost entered detection zone");

            isActivated = true;

            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Ghost left detection zone");

            isActivated = false;

            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void Update()
    {
        if (isActivated)
        {
            Debug.Log("Radio is activated");
        }
    }
}