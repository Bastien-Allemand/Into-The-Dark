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
    [SerializeField] private bool wantToSprint = false;
    [SerializeField] private bool isCeilingAbove = false;



    PlayerAction controls;

    private void Awake()
    {
        controls = new PlayerAction();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }
    private void FixedUpdate()
    {
        UpdateState();
        Move();
    }

    void UpdateState()
    {
        //  Crouch
        if (!isCrouched)
            isCrouched = controls.GamePlay.Crouch.ReadValue<bool>();
        else
        {
            //  look isCeilingAbove

           
        }
        wantToSprint = controls.GamePlay.Sprint.ReadValue<bool>();
    }
    void Look()
    {
        Vector2 mouse = controls.GamePlay.Look.ReadValue<Vector2>() * sensitivity;

        targetRotation.x -= mouse.y;
        targetRotation.y += mouse.x;

        targetRotation.x = Mathf.Clamp(targetRotation.x, -XMaxAngle, XMaxAngle);

        currentRotation.x = Mathf.Lerp(currentRotation.x, targetRotation.x, 10f * Time.deltaTime);
        currentRotation.y = Mathf.Lerp(currentRotation.y, targetRotation.y, 10f * Time.deltaTime);

        _camera.transform.localRotation = Quaternion.Euler(currentRotation.x, 0, 0);
        transform.localRotation = Quaternion.Euler(0, currentRotation.y, 0);
    }

    void Move()
    {
        moveInput = controls.GamePlay.Move.ReadValue<Vector2>();
        if (moveInput == Vector2.zero)
            return;

        if (!isCrouched && controls.GamePlay.Crouch.ReadValue<bool>())
        {
            Run();
        }
        else
        {
            rb.MovePosition(
                rb.position +
                (transform.forward * moveInput.y + transform.right * moveInput.x)
                * speed * Time.fixedDeltaTime
            );
        }
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
