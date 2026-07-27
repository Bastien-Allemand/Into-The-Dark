using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerAction controls;

    [Header("Reference")]
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerStateMachine playerStateMachine;

    [Header("Camera Settings")]
    [SerializeField] private float sensitivity = 0.5f;
    [SerializeField] private float XMaxAngle = 75f;

    [Header("Camera Effects")]
    [SerializeField] private float sprintForwardOffset = 0.3f;
    [SerializeField] private float crouchCameraHeightOffset = -0.4f;
    [SerializeField] private float crouchCameraForwardOffset = 0.0f;
    [SerializeField] private float positionTransitionSpeed = 10f;

    [Header("Crouch Camera Clipping")]
    [SerializeField] private float crouchNearClipPlane = 0.01f;
    [SerializeField] private float defaultNearClipPlane = 0.1f;

    private Vector2 targetRotation;
    private Vector3 defaultCameraLocalPos;

    public bool canLook = true;

    private void Awake()
    {
        controls = InputManager.controls;

        if (_camera != null)
        {
            defaultCameraLocalPos = _camera.transform.localPosition;
            defaultNearClipPlane = _camera.nearClipPlane;
        }
    }

    private void Update()
    {
        Look();
        UpdateCameraPosition();
    }

    private void FixedUpdate()
    {
        RotatePlayer();
    }

    private void Look()
    {
        if (!canLook || _camera == null)
            return;

        Vector2 mouse = controls.GeneriqueMove.Look.ReadValue<Vector2>() * sensitivity;

        targetRotation.x -= mouse.y;
        targetRotation.y += mouse.x;

        targetRotation.x = Mathf.Clamp(targetRotation.x, -XMaxAngle, XMaxAngle);

        _camera.transform.localRotation = Quaternion.Euler(targetRotation.x, 0f, 0f);
    }

    private void RotatePlayer()
    {
        transform.localRotation = Quaternion.Euler(0f, targetRotation.y, 0f);
    }

    private void UpdateCameraPosition()
    {
        if (_camera == null || playerStateMachine == null)
            return;

        Vector3 targetPosition = defaultCameraLocalPos;

        bool isCrouching = playerStateMachine.currentState == playerStateMachine.CrouchState;

        // sprint effect
        if (playerStateMachine.currentState == playerStateMachine.SprintState)
        {
            targetPosition.z += sprintForwardOffset;
        }

        // crouch effect
        if (isCrouching)
        {
            targetPosition.y += crouchCameraHeightOffset;
            targetPosition.z += crouchCameraForwardOffset;

            _camera.nearClipPlane = Mathf.Lerp(
                _camera.nearClipPlane,
                crouchNearClipPlane,
                Time.deltaTime * positionTransitionSpeed
            );
        }
        else
        {
            _camera.nearClipPlane = Mathf.Lerp(
                _camera.nearClipPlane,
                defaultNearClipPlane,
                Time.deltaTime * positionTransitionSpeed
            );
        }

        _camera.transform.localPosition = Vector3.Lerp(
            _camera.transform.localPosition,
            targetPosition,
            Time.deltaTime * positionTransitionSpeed
        );
    }
}