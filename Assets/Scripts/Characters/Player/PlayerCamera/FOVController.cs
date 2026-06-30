using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FOVController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private PlayerStateMachine stateMachine;

    [Header("Paramètres")]
    [SerializeField] private float fovMarche = 60f;
    [SerializeField] private float fovCourse = 68f;
    [SerializeField] private float vitesseTransition = 8f;

    private Camera cam;
    private float fovCible;
    private bool isExhausted;

    void Awake()
    {
        cam = GetComponent<Camera>();
        fovCible = fovMarche;
    }

    void OnEnable()
    {
        if (stateMachine != null)
        {
            stateMachine.OnSprintStatusChanged += HandleSprintStatus;
            stateMachine.OnExhaustionChanged += HandleExhaustion;
        }
    }

    void OnDisable()
    {
        if (stateMachine != null)
        {
            stateMachine.OnSprintStatusChanged -= HandleSprintStatus;
            stateMachine.OnExhaustionChanged -= HandleExhaustion;
        }
    }

    private void HandleSprintStatus(bool isSprinting)
    {
        if (!isExhausted) fovCible = isSprinting ? fovCourse : fovMarche;
    }

    private void HandleExhaustion(bool exhausted)
    {
        isExhausted = exhausted;
        if (exhausted)
        {
            cam.fieldOfView = fovMarche;
            fovCible = fovMarche;
        }
    }

    void Update()
    {
        if (Mathf.Abs(cam.fieldOfView - fovCible) > 0.1f && !isExhausted)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fovCible, Time.deltaTime * vitesseTransition);
        }
    }
}