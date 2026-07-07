using UnityEngine;
using UnityEngine.Rendering;

public class SpeedVisualEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStateMachine stateMachine;
    [SerializeField] private Volume speedVolume; // Volume pour la désaturation
    [SerializeField] private Material runMaterial;

    [Header("Settings")]
    [SerializeField] private float transitionSpeed = 5f;
    [SerializeField] private float maxBlurIntensity = 0.05f; // Intensité max du flou

    private float _targetWeight = 0f;
    private float _currentWeight = 0f;

    // Cache de l'ID de la propriété du shader (plus performant que d'utiliser un string dans le Update)
    private static readonly int BlurIntensityID = Shader.PropertyToID("_BlurIntensity");

    private void Start()
    {
        if (stateMachine == null)
            stateMachine = GetComponent<PlayerStateMachine>();

        //if (stateMachine != null)
        //    stateMachine.OnSprintStatusChanged += UpdateVisualTarget;

        // On réinitialise le shader au démarrage
        if (runMaterial != null)
            runMaterial.SetFloat(BlurIntensityID, 0f);
    }

    private void OnDestroy()
    {
        //if (stateMachine != null)
        //    stateMachine.OnSprintStatusChanged -= UpdateVisualTarget;

        // Sécurité : On nettoie le shader quand on quitte le jeu
        if (runMaterial != null)
            runMaterial.SetFloat(BlurIntensityID, 0f);
    }

    private void UpdateVisualTarget(bool isSprinting)
    {
        _targetWeight = isSprinting ? 1f : 0f;
    }

    private void Update()
    {
        // Interpolation fluide globale
        _currentWeight = Mathf.Lerp(_currentWeight, _targetWeight, Time.deltaTime * transitionSpeed);
        if (_currentWeight < 0.0001f) _currentWeight = 0f;

        // 1. Application sur le volume (Désaturation)
        if (speedVolume != null)
        {
            speedVolume.weight = _currentWeight;
        }

        // 2. Application sur le Fullscreen Shader (Flou Radial)
        if (runMaterial != null)
        {
            float currentBlur = _currentWeight * maxBlurIntensity;
            if (currentBlur < 0.0005f) currentBlur = 0f;
            runMaterial.SetFloat(BlurIntensityID, currentBlur);            
        }
    }
}