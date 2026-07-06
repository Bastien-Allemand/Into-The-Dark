using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepAudio : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private HeadBobber headBobber;

    [Header("Paramètres")]
    [SerializeField] private AudioClip[] bruitsDePas;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (headBobber != null)
            headBobber.OnStepTaken += JouerSonDePas;
    }

    void OnDisable()
    {
        if (headBobber != null)
            headBobber.OnStepTaken -= JouerSonDePas;
    }

    private void JouerSonDePas()
    {
        if (bruitsDePas.Length > 0)
        {
            int index = Random.Range(0, bruitsDePas.Length);
            audioSource.PlayOneShot(bruitsDePas[index]);
            audioSource.pitch = Random.Range(0.9f, 1.1f);
        }
    }
}