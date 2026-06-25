using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] public bool debug = false;

    PlayerAction controls;
    private IState currentState;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }

    public deathCause LastDeathCause { get; set; }

    public PlayerOnPhoneState OnPhoneState { get; private set; }

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private PhoneScript phoneScript;

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
    public float ceilingCheckDistance = .6f;
    [SerializeField] public bool isCeilingAbove = false;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 ceilingCheckSize = new Vector3(0.6f, 2f, 0.6f);


    public Vector2 moveInput;

    private float maxStamina = 5f;
    private float staminaRegenDelay = 1.5f;
    public float MaxStamina => maxStamina;



    void Awake()
    {
        controls = InputManager.controls;

        //        controls = new PlayerAction();

        IdleState = new PlayerIdleState(this, rb, transform);
        WalkState = new PlayerWalkState(this, rb, transform);
        SprintState = new PlayerSprintState(this, rb, transform);
        CrouchState = new PlayerCrouchState(this, rb, transform, playerCollider);
        OnPhoneState = new PlayerOnPhoneState(this);
    }
    void Start()
    {
        currentState = IdleState;
    }

    //private void OnEnable() => controls.Enable();
    //private void OnDisable() => controls.Disable();

    void Update()
    {
        moveInput = InputManager.controls.GamePlay.Move.ReadValue<Vector2>();

        if (currentState != SprintState)
        {
            RegenStamina();
        }

        currentState?.Update();
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

    private void OnDrawGizmos()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        Gizmos.DrawWireCube(origin, ceilingCheckSize * .75f);
    }
    void CheckIsCeilingAbove()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y+.5f, transform.position.z);
        Color rayColor = Color.green;
        float castLength = ceilingCheckDistance;

        
        if (Physics.BoxCast(origin, ceilingCheckSize * .75f, Vector3.up, Quaternion.identity, castLength, layerMask))
        {
            isCeilingAbove = true;
            rayColor = Color.red;
        }
        else
        {
            isCeilingAbove = false;
        }

        Debug.DrawRay(origin, Vector3.up * castLength, rayColor);
    }

    void RegenStamina()
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
        //UpdateSprintUI();
    }

    
}
