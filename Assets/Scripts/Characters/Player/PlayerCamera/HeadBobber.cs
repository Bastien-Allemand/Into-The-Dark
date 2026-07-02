using System;
using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    public event Action OnStepTaken;

    [Header("Références")]
    [SerializeField] private PlayerStateMachine stateMachine;

    [Header("Paramètres")]
    [SerializeField] private float frequenceMarche = 10f;
    [SerializeField] private float amplitudeMarche = 0.05f;
    [SerializeField] private float multiplicateurCourse = 1.5f;

    [Header("Paramètres d'Épuisement")]
    [SerializeField] private float frequenceEpuisement = 5f;
    [SerializeField] private float amplitudeEpuisement = 0.15f;
    private bool isExhausted = false;

    private float positionInitialeY;
    private float timer = 0f;
    private bool stepFired = false;
    private bool isSprinting = false;

    void Start()
    {
        positionInitialeY = transform.localPosition.y;
    }

    void OnEnable()
    {
        //if (stateMachine != null)
        //    stateMachine.OnSprintStatusChanged += HandleSprintStatus;
    }

    void OnDisable()
    {
        //if (stateMachine != null)
        //    stateMachine.OnSprintStatusChanged -= HandleSprintStatus;
    }

    private void HandleSprintStatus(bool sprinting)
    {
        isSprinting = sprinting;
    }

    private void HandleExhaustion(bool exhausted)
    {
        isExhausted = exhausted;
    }

    void Update()
    {
        bool isMoving = stateMachine.moveInput != Vector2.zero;

        if (isMoving || isExhausted)
        {
            float freq = frequenceMarche;
            float amp = amplitudeMarche;

            if (isExhausted)
            {
                freq = frequenceEpuisement;
                amp = amplitudeEpuisement;
            }
            else if (isSprinting)
            {
                freq = frequenceMarche * multiplicateurCourse;
                amp = amplitudeMarche * multiplicateurCourse;
            }

            timer += Time.deltaTime * freq;
            float sinValue = Mathf.Sin(timer);

            float nouvellePositionY = positionInitialeY + sinValue * amp;
            transform.localPosition = new Vector3(transform.localPosition.x, nouvellePositionY, transform.localPosition.z);

            if (isMoving && sinValue < -0.95f && !stepFired)
            {
                OnStepTaken?.Invoke();
                stepFired = true;
            }
            else if (sinValue > 0f)
            {
                stepFired = false;
            }
        }
        else
        {
            timer = 0f;
            Vector3 positionRepos = new Vector3(transform.localPosition.x, positionInitialeY, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, positionRepos, Time.deltaTime * 5f);
        }
    }
}