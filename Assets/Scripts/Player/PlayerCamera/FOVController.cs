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

    void Awake()
    {
        cam = GetComponent<Camera>();
        fovCible = fovMarche;
    }

    void OnEnable()
    {
        if (stateMachine != null)
            stateMachine.OnSprintStatusChanged += HandleSprintStatusChanged;
    }

    void OnDisable()
    {
        if (stateMachine != null)
            stateMachine.OnSprintStatusChanged -= HandleSprintStatusChanged;
    }

    private void HandleSprintStatusChanged(bool isSprinting)
    {
        fovCible = isSprinting ? fovCourse : fovMarche;
    }

    void Update()
    {
        if (Mathf.Abs(cam.fieldOfView - fovCible) > 0.1f)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fovCible, Time.deltaTime * vitesseTransition);
        }
    }
}