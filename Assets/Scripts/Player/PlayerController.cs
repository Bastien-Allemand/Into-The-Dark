using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[Header("State Settings")]

    private enum PlayerState
    {
        IDLE,
        WALKING,
        CROUCHING,
        SPRINTING,
    }

    //[Space(5)]
    [Header("State")]
    [SerializeField] private PlayerState currentState = PlayerState.IDLE;

    [Space(5)]
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private Camera _camera;
    [SerializeField] private RectTransform sprintBarTransform;

    [Space(5)]
    [Header("Look Settings")]
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float XMaxAngle = 60f;

    [Space(5)]
    [Header("Movement & Sprint Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintingMultiplier = 1.4f;
    [SerializeField] private float maxStamina = 5f;
    [SerializeField] private float staminaLeft = 5f;
    [SerializeField] private float staminaRegenDelay = 1.5f; // Remplace staminaIncreasingTotalTime

    [Space(5)]
    [Header("Crouch Settings")]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float standCenterY = 0f;
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float crouchCenterY = -0.2f;
    [SerializeField] private float ceilingCheckDistance = 1.0f;

    [Space(5)]
    [Header("Physics & Raycast")]
    [SerializeField] private LayerMask layerMask;

    [Space(5)]
    [Header("State Debug")]
    [SerializeField] private bool isCrouched = false;
    [SerializeField] private bool isCeilingAbove = false;
    [SerializeField] private bool isOutOfStamina = false;

    // Variables de calcul internes (privées)
    private float currentSpeed;
    private float staminaTimer = 0f;
    private float sprintBarInitialWidth;
    private Vector2 moveInput;
    private Vector2 targetRotation;
    private Vector2 currentRotation;
    private Vector3 velocity = Vector3.zero;
    private PlayerAction controls;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        controls = new PlayerAction();

        layerMask = LayerMask.GetMask("Ceiling");

        if (rb == null) rb = GetComponent<Rigidbody>();
        if (playerCollider == null) playerCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    void Update()
    {
        Look();
        HandleStamina();
    }
    private void FixedUpdate()
    {
        CheckIsCeilingAbove();
        UpdateState();
        Move();
    }

    void UpdateState()
    {
        moveInput = controls.GamePlay.Move.ReadValue<Vector2>();
        bool sprintInput = controls.GamePlay.Sprint.ReadValue<float>() > 0.5f;
        bool crouchInput = controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;
        bool isMoving = moveInput != Vector2.zero;

        if (crouchInput == true || (isCrouched == true && isCeilingAbove == true))
        {
            currentState = PlayerState.CROUCHING;
            Crouch();
            return;
        }
        else
        {
            StandUp();
        }

        if(isMoving == false)
        {
            currentState = PlayerState.IDLE;
            currentSpeed = walkSpeed;
        }
        else if(sprintInput == true && isOutOfStamina == false && isCrouched == false)
        {
            currentState = PlayerState.SPRINTING;
            currentSpeed = walkSpeed * sprintingMultiplier;
        }
        else
        {
            currentState = PlayerState.WALKING;
            currentSpeed = walkSpeed;
        }
    }
    void Look()
    {
        Vector2 mouse = controls.GamePlay.Look.ReadValue<Vector2>() * sensitivity;

        targetRotation.x -= mouse.y;
        targetRotation.y += mouse.x;

        targetRotation.x = Mathf.Clamp(targetRotation.x, -XMaxAngle, XMaxAngle);

        currentRotation.x = Mathf.Lerp(currentRotation.x, targetRotation.x, 25f * Time.deltaTime);
        currentRotation.y = Mathf.Lerp(currentRotation.y, targetRotation.y, 25f * Time.deltaTime);

        _camera.transform.localRotation = Quaternion.Euler(currentRotation.x, 0, 0);
        transform.localRotation = Quaternion.Euler(0, currentRotation.y, 0);
    }

    void Move()
    {
        if (moveInput == Vector2.zero) return;

        Vector3 targetVel = (transform.forward * moveInput.y + transform.right * moveInput.x) * currentSpeed;
        Vector3 currentVel = rb.linearVelocity;
        Vector3 desiredVel = new Vector3(targetVel.x, currentVel.y, targetVel.z);

        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, desiredVel, ref velocity, 0.05f);
    }

    void Crouch()
    {
        if (isCrouched == false)
        {
            playerCollider.height = crouchHeight;
            playerCollider.center = new Vector3(0f, crouchCenterY, 0f);
            isCrouched = true;
        }
    }

    void StandUp()
    {
        if (isCeilingAbove == true || isCrouched == false) return;

        playerCollider.height = standHeight;
        playerCollider.center = new Vector3(0f, standCenterY, 0f);
        isCrouched = false;
    }

    void HandleStamina()
    {
        if(currentState == PlayerState.SPRINTING)
        {
            staminaTimer = 0f;
            staminaLeft -= Time.deltaTime;

            if(staminaLeft <= 0f)
            {
                staminaLeft = 0f;
                isOutOfStamina = true;
            }
        }
        else
        {
            if(isOutOfStamina == true && staminaLeft > 3f)
            {
                isOutOfStamina = false;
            }

            if(staminaLeft < maxStamina)
            {
                if(staminaLeft > staminaRegenDelay)
                {
                    staminaTimer += Time.deltaTime;
                }
                else
                {
                    staminaTimer += (Time.deltaTime / 2f);
                }
            }
            else
            {
                staminaLeft = maxStamina;
            }
        }
        UpdateSprintUI();
    }

    void UpdateSprintUI()
    {
        if (sprintBarTransform == null) return;

        float percentLeft = staminaLeft / maxStamina;
        sprintBarTransform.sizeDelta = new Vector2(sprintBarInitialWidth * percentLeft, sprintBarTransform.rect.height);
    }

    void CheckIsCeilingAbove()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        Color rayColor = Color.green;

        float castLength = standHeight + ceilingCheckDistance;

        if (Physics.Raycast(origin, Vector3.up, castLength, layerMask))
        {
            isCeilingAbove = true;
            rayColor = Color.red;
        }
        else
        {
            isCeilingAbove = false;
        }

        Debug.DrawRay(origin, Vector3.up, rayColor);
    }
}
