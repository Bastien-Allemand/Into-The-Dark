using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("State Settings")]

    [Space(5)]

    [Header("Move Settings")]
    [SerializeField] float speed;
    [Header("Debug")]
    [SerializeField] float currentSpeed;
    [SerializeField] private Vector2 moveInput;

    [Space(5)]

    [Header("Look Settings")]
    [SerializeField] Camera _camera;
    [SerializeField] float sensitivity = 100f;
    [SerializeField] float XMaxAngle = 60;
    [Header("Debug")]
    [SerializeField] private Vector2 targetRotation;
    [SerializeField] private Vector2 currentRotation;

    [Space(5)]

    [Header("Crouch Settings")]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float standCenterY = 0f;
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float crouchCenterY = -0.2f;
    [SerializeField] private float ceilingCheckDistance = 1.0f;

    //TO REMOVE
    private float crouchScale;
    private Vector3 initialScale;

    [Space(10)]

    [Header("Reference")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;

    [Space(10)]

    [Header("State (Debug)")]
    [SerializeField] private bool isCrouched = false;
    [SerializeField] private bool isCeilingAbove = false;



    PlayerAction controls;

    private void Awake()
    {
        controls = new PlayerAction();
    }
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
        //Vector2 move = controls.GamePlay.Look.ReadValue<Vector2>() * 100f * Time.deltaTime;

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
        moveInput = controls.GamePlay.Move.ReadValue<Vector2>();
        if (moveInput == Vector2.zero)
            return;

        rb.MovePosition(
            rb.position +
            (transform.forward * moveInput.y + transform.right * moveInput.x)
            * speed * Time.fixedDeltaTime
        );

        /*
        Vector3 moveTarget = (transform.forward * moveInputZ + transform.right * moveInputX) * speed;
        Vector3 currentVel = rb.linearVelocity;
        Vector3 desiredVel = new Vector3(moveTarget.x, currentVel.y, moveTarget.z);
        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, moveTarget, ref velocity, 0.05f);
         */
    }

    void Crouch()
    {
        if (isCrouched == false)
        {
            //To remove
            transform.localScale = new Vector3(initialScale.x, crouchScale, initialScale.z);
            //
            playerCollider.height = crouchHeight;
            playerCollider.center = new Vector3(0f, crouchCenterY, 0f);
            isCrouched = true;
        }
    }

    void Run()
    {

    }
}
