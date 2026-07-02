using Assets.Scripts.Items;
using System;
using UnityEngine;


[System.Serializable]
public class CrouchSettings
{
    public float standHeight;
    public float standCenterY;
    public float crouchHeight;
    public float crouchCenterY;
    public float ceilingCheckDistance;
    public bool isCeilingAbove;
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

    PlayerAction controls;
    private IState currentState;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerOnPhoneState OnPhoneState { get; private set; }
    public deathCause LastDeathCause { get; set; }



    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;

    [Space(5)]
    [Header("Sprint Settings")]
    [SerializeField] private MoveSettings moveSettings;
    public MoveSettings moveConfigs => moveSettings;

    public float currentSpeed;

    [Space(5)]
    [Header("Stamina Settings")]
    [SerializeField] private StaminaSettings staminaSettings;
    public StaminaSettings staminaConfigs => staminaSettings;


    [Space(5)]
    [Header("Crouch Settings")]
    [SerializeField] private CrouchSettings crouchSettings;
    public CrouchSettings crouchConfigs => crouchSettings;

    [Space(5)]

    [SerializeField] private LayerMask layerMask;

    public Vector2 moveInput;

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

        moveSettings.walkSpeed = 5f;
        moveSettings.sprintingMultiplier = 1.4f;
        moveSettings.crouchMultiplier = 0.6f;
        currentSpeed = moveSettings.walkSpeed;

        staminaSettings.staminaRegenDelay = 1.5f;
        staminaSettings.staminaLeft = staminaSettings.maxStamina;
        staminaSettings.staminaTimer = 0f;
        staminaSettings.isOutOfStamina = false;
    }

    void Update()
    {
        moveInput = InputManager.controls.GamePlay.Movement.ReadValue<Vector2>();

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

    //private void OnDrawGizmos()
    //{
    //    Vector3 origin = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
    //    Gizmos.DrawWireCube(origin, ceilingCheckSize * .75f);
    //}

    

    void RegenStamina()
    {
        if (staminaSettings.isOutOfStamina == true && staminaSettings.staminaLeft > 3f)
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
                staminaSettings.staminaLeft += (Time.deltaTime * 0.75f);
            }
        }
        else
        {
            staminaSettings.staminaLeft = staminaSettings.maxStamina;
        }

        

    //UpdateSprintUI();
    }

   public float StaminaRatio
   {
        get
        {
            if (staminaConfigs == null || staminaConfigs.staminaLeft <= 0) return 0f;
            return staminaConfigs.staminaLeft / staminaConfigs.maxStamina;
        }
    }

}

    

