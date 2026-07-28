using System;
using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CrouchSettings
{
    public float standHeight;
    public float standCenterY;

    public float crouchHeight;
    public float crouchCenterY;

    public float ceilingCheckDistance;
    public bool isCeilingAbove;

    [Header("Collider Offset")]
    public float crouchCenterZ = 1f;
}

[System.Serializable]
public class MoveSettings
{
    public float walkSpeed;
    public float sprintingMultiplier;
    public float crouchMultiplier;
}

[System.Serializable]
public class StaminaSettings
{
    public float staminaLeft;
    public float staminaTimer;
    public float maxStamina;
    public float staminaRegenDelay;
    public bool isOutOfStamina;
}

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] public bool debug = false;

    private PlayerAction controls;


    public enum state
    {
        NONE,
        IDLE,
        WALK,
        SPRINT
    }


    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerOnPhoneState OnPhoneState { get; private set; }


    public deathCause LastDeathCause { get; set; }


    [Header("State")]
    [SerializeField] public IState currentState;


    [Header("References")]
    public Transform activePlayerTransform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private Animator animator;


    [Header("Movement Settings")]
    [SerializeField] private MoveSettings moveSettings;
    public MoveSettings moveConfigs => moveSettings;

    public float currentSpeed;


    [Header("Stamina Settings")]
    [SerializeField] private StaminaSettings staminaSettings;
    public StaminaSettings staminaConfigs => staminaSettings;


    [Header("Crouch Settings")]
    [SerializeField] private CrouchSettings crouchSettings;
    public CrouchSettings crouchConfigs => crouchSettings;


    [Header("Collision")]
    [SerializeField] private LayerMask layerMask;


    public Vector2 moveInput;


    private void Awake()
    {
        controls = InputManager.controls;

        RefreshActivePlayer();

        IdleState = new PlayerIdleState(this, rb, activePlayerTransform);
        WalkState = new PlayerWalkState(this, rb, activePlayerTransform);
        SprintState = new PlayerSprintState(this, rb, activePlayerTransform);
        CrouchState = new PlayerCrouchState(this, rb, activePlayerTransform, playerCollider);
        OnPhoneState = new PlayerOnPhoneState(this);
    }

    public void RefreshActivePlayer()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                activePlayerTransform = child;
                
                rb = child.GetComponent<Rigidbody>();
                if (rb == null) rb = child.GetComponentInChildren<Rigidbody>();
                
                playerCollider = child.GetComponent<CapsuleCollider>();
                if (playerCollider == null) playerCollider = child.GetComponentInChildren<CapsuleCollider>();
                
                break;
            }
        }

        if (activePlayerTransform == null)
        {
            activePlayerTransform = transform;
        }

        RefreshAnimator();
    }

    private void Start()
    {
        currentState = IdleState;

        // Movement
        moveSettings.walkSpeed = 5f;
        moveSettings.sprintingMultiplier = 1.4f;
        moveSettings.crouchMultiplier = 0.6f;

        currentSpeed = moveSettings.walkSpeed;

        // Stamina
        staminaSettings.maxStamina = 100f;
        staminaSettings.staminaRegenDelay = 1.5f;
        staminaSettings.staminaLeft = staminaSettings.maxStamina;
        staminaSettings.staminaTimer = 0f;
        staminaSettings.isOutOfStamina = false;

        // Crouch
        crouchSettings.standHeight = 2f;
        crouchSettings.standCenterY = 0f;
        crouchSettings.crouchCenterZ = 0.15f;

        crouchSettings.crouchHeight = 1.2f;
        crouchSettings.crouchCenterY = -0.2f;

        crouchSettings.ceilingCheckDistance = 0.6f;
    }
    private void OnEnable()
    {
        InputManager.controls.Global.MoveForward.performed +=Forward;
        InputManager.controls.Global.MoveForward.canceled += ForwardCanceled;
        InputManager.controls.Global.MoveBackward.performed += Backward;
        InputManager.controls.Global.MoveBackward.canceled += BackwardCanceled;

        InputManager.controls.Global.MoveToLeft.performed += ToLeft;
        InputManager.controls.Global.MoveToLeft.canceled += ToLeftCanceled;

        InputManager.controls.Global.MoveToRight.performed += ToRight;
        InputManager.controls.Global.MoveToRight.canceled += ToRightCanceled;
    }

    private void OnDisable()
    {
        InputManager.controls.Global.MoveForward.performed -= Forward;
        InputManager.controls.Global.MoveForward.canceled -= ForwardCanceled;

        InputManager.controls.Global.MoveBackward.performed -= Backward;
        InputManager.controls.Global.MoveBackward.canceled -= BackwardCanceled;

        InputManager.controls.Global.MoveToLeft.performed -= ToLeft;
        InputManager.controls.Global.MoveToLeft.canceled -= ToLeftCanceled;

        InputManager.controls.Global.MoveToRight.performed -= ToRight;
        InputManager.controls.Global.MoveToRight.canceled -= ToRightCanceled;
    }

    private void Forward(InputAction.CallbackContext ctx)
    {
        moveInput.y = InputManager.controls.Global.MoveForward.ReadValue<float>();
    }
    private void ForwardCanceled(InputAction.CallbackContext ctx)
    {
        if (InputManager.controls.Global.MoveBackward.IsPressed())
            return;
        moveInput.y = 0f;
    }
    private void Backward(InputAction.CallbackContext ctx)
    {
        moveInput.y = -InputManager.controls.Global.MoveBackward.ReadValue<float>();
    }
    private void BackwardCanceled(InputAction.CallbackContext ctx)
    {
        if (InputManager.controls.Global.MoveForward.IsPressed())
            return;
        moveInput.y = 0f;
    }
    private void ToLeft(InputAction.CallbackContext ctx)
    {
        moveInput.x = -InputManager.controls.Global.MoveToLeft.ReadValue<float>();
    }
    private void ToLeftCanceled(InputAction.CallbackContext ctx)
    {

        if (InputManager.controls.Global.MoveToRight.IsPressed())
            return;
        moveInput.x =0;
    }
    private void ToRight(InputAction.CallbackContext ctx)
    {
        moveInput.x = InputManager.controls.Global.MoveToRight.ReadValue<float>();
    }
    private void ToRightCanceled(InputAction.CallbackContext ctx)
    {
        if (InputManager.controls.Global.MoveToLeft.IsPressed())
            return;
        moveInput.x = 0;
    }
    
    public void RefreshAnimator()
    {
        animator = null;

        foreach (Animator anim in GetComponentsInChildren<Animator>(true))
        {
            if (anim.gameObject.activeInHierarchy)
            {
                animator = anim;
                break;
            }
        }
    }
    void Update()
    {
        float speedMultiplier = currentState == SprintState ? moveSettings.sprintingMultiplier : 1f;
        if (currentState != SprintState)
        {
            RegenStamina();
        }

        UpdateAnimator();
        animator.SetFloat("Speed", moveInput.magnitude * speedMultiplier);

        currentState?.Update();
    }

    private void UpdateAnimator()
    {
        if (animator == null)
            return;

        bool isCrouching = currentState == CrouchState;

        animator.SetBool("IsCrouch", isCrouching);

        float speedMultiplier = 1f;

        if (currentState == SprintState)
        {
            speedMultiplier = moveSettings.sprintingMultiplier;
        }
        else if (currentState == CrouchState)
        {
            speedMultiplier = moveSettings.crouchMultiplier;
        }

        animator.SetFloat("Speed", moveInput.magnitude * speedMultiplier);
    }

    public void ChangeState(IState newState)
    {
        if (newState == null)
            return;

        if (currentState != null &&
            currentState.GetType() == newState.GetType())
        {
            return;
        }

        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter();
    }

    private void RegenStamina()
    {
        if (staminaSettings.isOutOfStamina &&
            staminaSettings.staminaLeft > 3f)
        {
            staminaSettings.isOutOfStamina = false;
        }

        if (staminaSettings.staminaLeft < staminaSettings.maxStamina)
        {
            if (staminaSettings.staminaTimer < staminaSettings.staminaRegenDelay)
            {
                staminaSettings.staminaTimer += Time.deltaTime;
            }
            else
            {
                staminaSettings.staminaLeft += Time.deltaTime * 0.75f;
            }
        }
        else
        {
            staminaSettings.staminaLeft = staminaSettings.maxStamina;
        }
    }
    public float StaminaRatio
    {
        get
        {
            if (staminaConfigs == null ||
                staminaConfigs.maxStamina <= 0)
            {
                return 0f;
            }

            return staminaConfigs.staminaLeft / staminaConfigs.maxStamina;
        }
    }
}