using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerAction controls;

    private IState currentState;

    [Header("State")]

    [Space(5)]
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private Camera camera;
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
    [SerializeField] private float staminaRegenDelay = 1.5f;

    [Space(5)]
    [Header("Crouch Settings")]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float standCenterY = 0f;
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float crouchCenterY = -0.2f;
    [SerializeField] private float ceilingCheckDistance = 1.0f;
    [SerializeField] private float crouchMultiplier = 0.5f;

    [Space(5)]
    [Header("Physics & Raycast")]
    [SerializeField] private LayerMask layerMask;

    [Space(5)]
    [Header("State Debug")]
    [SerializeField] private bool isCrouched = false;
    [SerializeField] private bool isCeilingAbove = false;
    [SerializeField] private bool isOutOfStamina = false;

    public float currentSpeed;
    public Vector2 moveInput;
    private float staminaTimer = 0f;
    private float sprintBarInitialWidth;
    private Vector2 targetRotation;
    private Vector2 currentRotation;
    private Vector3 velocity = Vector3.zero;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }

    void Awake()
    {
        controls = new PlayerAction();

        IdleState = new PlayerIdleState(this, rb);
        WalkState = new PlayerWalkState(this, rb, transform);
        SprintState = new PlayerSprintState(this);
        CrouchState = new PlayerCrouchState(this, rb, transform);
    }
    void Start()
    {
        currentState = IdleState;
        currentSpeed = walkSpeed;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Update()
    {
        moveInput = controls.GamePlay.Move.ReadValue<Vector2>();
        bool sprintInput = controls.GamePlay.Sprint.ReadValue<float>() > 0.5f;
        bool crouchInput = controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;
        bool isMoving = moveInput != Vector2.zero;

        
        if (isMoving == false)
        {
            ChangeState(IdleState);
        }
        else if (sprintInput == true )
        {
            ChangeState(SprintState);
        }
        else if (crouchInput == true)
        {
            ChangeState(CrouchState);
        }
        else
        {
            ChangeState(WalkState);
        }
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void ChangeState(IState newState)
    {
        if (newState == null || currentState.GetType() == newState.GetType())
        {
            return;
        }
        else if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;

        currentState.Enter();

        if (currentState == null)
            return;

        currentState.Update();
    }


}
