using UnityEngine;

public class PlayerView : MonoBehaviour
{
    PlayerAction controls;

    [Header("Reference")]
    [SerializeField] private Camera _camera;

    [Header("Camera Settings")]
    [SerializeField] private float sensitivity = 0.5f;
    [SerializeField] private float XMaxAngle = 75f;

    private Vector2 targetRotation;

    public bool canLook = true; 

    [Header("Sprint Camera Effect")]
    [SerializeField] private PlayerStateMachine playerStateMachine;
    [SerializeField] private float sprintForwardOffset = 0.5f;
    [SerializeField] private float positionTransitionSpeed = 10f;

    private Vector3 defaultCameraLocalPos;

    private void Awake()
    {
        controls = InputManager.controls;
        if (_camera != null)
        {
            defaultCameraLocalPos = _camera.transform.localPosition;
        }
    }

    void Update()
    {
        Look();
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        if (playerStateMachine != null && _camera != null)
        {
            Vector3 targetPosition = defaultCameraLocalPos;
            if (playerStateMachine.currentState == playerStateMachine.SprintState)
            {
                targetPosition.z += sprintForwardOffset;
            }
            _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, targetPosition, Time.deltaTime * positionTransitionSpeed);
        }
    }

    private void FixedUpdate()
    {
        RotatePlayer();
    }

    void Look()
    {
        if (!canLook) return;

        Vector2 mouse = controls.GeneriqueMove.Look.ReadValue<Vector2>() * sensitivity;

        targetRotation.x -= mouse.y;
        targetRotation.y += mouse.x;

        targetRotation.x = Mathf.Clamp(targetRotation.x, -XMaxAngle, XMaxAngle);

        _camera.transform.localRotation = Quaternion.Euler(targetRotation.x, 0, 0);
    }

    void RotatePlayer()
    {
        transform.localRotation = Quaternion.Euler(0, targetRotation.y, 0);
    }
}