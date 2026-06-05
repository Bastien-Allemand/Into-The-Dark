using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerAction controls;

    private IState currentState;

    [Header("State")]
    public bool isOutOfStamina = false;
    public float staminaLeft = 5f;
    public float staminaTimer = 0f;
    private float maxStamina = 5f;
    private float staminaRegenDelay = 1.5f;
    private float sprintBarInitialWidth;

    [Space(5)]
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private RectTransform sprintBarTransform;

    [Space(5)]
    [Header("Crouch Settings")]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float standCenterY = 0f;
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float crouchCenterY = -0.2f;
    [SerializeField] private float ceilingCheckDistance = 1.0f;

    [Space(5)]
    [Header("State Debug")]
    [SerializeField] private bool isCrouched = false;
    [SerializeField] private bool isCeilingAbove = false;

    public float currentSpeed;
    public Vector2 moveInput;
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
        SprintState = new PlayerSprintState(this, rb, transform);
        CrouchState = new PlayerCrouchState(this);
    }
    void Start()
    {
        currentState = IdleState;
        sprintBarInitialWidth = sprintBarTransform.rect.width;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Update()
    {

        HandleStamina();
        CheckState();
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    void CheckState()
    {
        moveInput = controls.GamePlay.Move.ReadValue<Vector2>();
        bool sprintInput = controls.GamePlay.Sprint.ReadValue<float>() > 0.5f;
        bool crouchInput = controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;
        bool isMoving = moveInput != Vector2.zero;

        if (crouchInput == true)
        {
            ChangeState(CrouchState);
            return;
        }
        if (isMoving == false)
        {
            ChangeState(IdleState);
        }
        else if (sprintInput == true && isOutOfStamina == false)
        {
            ChangeState(SprintState);
        }
        else
        {
            ChangeState(WalkState);
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
    }

    void HandleStamina()
    {
        if (isOutOfStamina == true && staminaLeft > 3f)
        {
            isOutOfStamina = false;
        }

        if (staminaLeft < maxStamina)
        {

            if (staminaTimer < staminaRegenDelay)
            {
                staminaTimer += Time.deltaTime;
            }
            else
            {
                staminaLeft += (Time.deltaTime * 0.75f);
            }
        }
        else
        {
            staminaLeft = maxStamina;
        }
        UpdateSprintUI();
    }

    void UpdateSprintUI()
    {
        if (sprintBarTransform == null) return;

        float percentLeft = staminaLeft / maxStamina;
        sprintBarTransform.sizeDelta = new Vector2(sprintBarInitialWidth * percentLeft, sprintBarTransform.rect.height);
    }
}
