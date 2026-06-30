using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class ExhaustionFeedback : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private PlayerStateMachine stateMachine;

    [Header("Audio")]
    [SerializeField] private AudioClip grosseRespiClip;
    private AudioSource audioSource;

    [Header("Visuel (Post-Processing)")]
    [Tooltip("Met ici ton Volume global ou local qui contient le flou/vignette")]
    [SerializeField] private Volume volumeEpuisement;
    [SerializeField] private float vitesseTransitionVisuel = 2f;

    private float poidsVolumeCible = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = grosseRespiClip;
        audioSource.loop = true;
    }

    void OnEnable()
    {
        if (stateMachine != null)
            stateMachine.OnExhaustionChanged += HandleExhaustion;
    }

    void OnDisable()
    {
        if (stateMachine != null)
            stateMachine.OnExhaustionChanged -= HandleExhaustion;
    }

    private void HandleExhaustion(bool isExhausted)
    {
        if (isExhausted)
        {
            audioSource.Play();
            poidsVolumeCible = 1f;
        }
        else
        {
            audioSource.Stop();
            poidsVolumeCible = 0f;
        }
    }

    void Update()
    {
        if (volumeEpuisement != null && volumeEpuisement.weight != poidsVolumeCible)
        {
            volumeEpuisement.weight = Mathf.MoveTowards(volumeEpuisement.weight, poidsVolumeCible, Time.deltaTime * vitesseTransitionVisuel);
        }
    }
}