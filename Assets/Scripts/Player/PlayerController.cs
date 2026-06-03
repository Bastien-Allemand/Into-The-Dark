using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("State Settings")]

    [Space(5)]

    [Header("Look Settings")]
    [SerializeField] Camera _camera;
    [SerializeField] float sensitivity = 100f;
    [SerializeField] float XMaxAngle = 60;
    [Header("Debug (just look)")]
    [SerializeField] private Vector2 targetRotation;
    [SerializeField] private Vector2 currentRotation;

    [Space(5)]

    [Header("Crouch Settings")]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float standCenterY = 0f;
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float crouchCenterY = -0.2f;
    [SerializeField] private float ceilingCheckDistance = 1.0f;

    [Space(10)]

    [Header("Reference")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;

    [Space(10)]

    [Header("State (Debug)")]
    [SerializeField] private bool isCrouched = false;
    [SerializeField] private bool isCeilingAbove = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;

        transform.Rotate(0, mouseX, 0);

        float bufferX = targetRotation.x;

        targetRotation.x -= mouseY;
        targetRotation.x = Mathf.Clamp(targetRotation.x, -90f, 90f);

        targetRotation.y += mouseX;

        float test = targetRotation.x * sensitivity / 10 + transform.rotation.x;

        if (test > XMaxAngle || test < -XMaxAngle)
            targetRotation.x = bufferX;

        currentRotation.x = Mathf.Lerp(currentRotation.x, targetRotation.x, 100f * Time.deltaTime);
        currentRotation.y = Mathf.Lerp(currentRotation.y, targetRotation.y, 100f * Time.deltaTime);

        _camera.transform.localRotation =
            Quaternion.Euler(currentRotation.x * sensitivity / 10, 0, 0);

        transform.localRotation =
            Quaternion.Euler(0, currentRotation.y * sensitivity / 10, 0);
    }

    void Move()
    {

    }
}
