using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerAction controls;
    private IState currentState;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private RectTransform sprintBarTransform;


    [Space(5)]

    [Header("Sprint Settings")]
    public float walkSpeed = 5f;
    public float sprintingMultiplier = 1.4f;
    public float crouchMultiplier = 0.6f;
    public float staminaLeft = 5f;
    public float staminaTimer = 0f;
    public float currentSpeed;
    public bool isOutOfStamina = false;

    [Space(5)]

    [Header("Crouch Settings")]
    public float standHeight = 2f;
    public float standCenterY = 0f;
    public float crouchHeight = 1.2f;
    public float crouchCenterY = -0.2f;
    public float ceilingCheckDistance = 1.0f;

    public Vector2 moveInput;

    private float maxStamina = 5f;
    private float staminaRegenDelay = 1.5f;
    private float sprintBarInitialWidth;


    void Awake()
    {
        controls = new PlayerAction();

        IdleState = new PlayerIdleState(this, rb);
        WalkState = new PlayerWalkState(this, rb, transform);
        SprintState = new PlayerSprintState(this, rb, transform);
        CrouchState = new PlayerCrouchState(this, rb, transform, playerCollider);

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

    void ChangeState(IState newState)
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

    void CheckState()
    {
        moveInput = controls.GamePlay.Move.ReadValue<Vector2>();
        bool sprintInput = controls.GamePlay.Sprint.ReadValue<float>() > 0.5f;
        bool crouchInput = controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;
        bool isMoving = moveInput != Vector2.zero;

        
        if (isMoving == false)
        {
            if (crouchInput == true)
            {
                ChangeState(CrouchState);
            }
            else
            {
                ChangeState(IdleState);
            }
                
            
        }
        else if (crouchInput == true)
        {
            ChangeState(CrouchState);
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
