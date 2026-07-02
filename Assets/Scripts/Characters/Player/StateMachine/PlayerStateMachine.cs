<<<<<<< HEAD
using UnityEditor;
=======
using Assets.Scripts.Items;
using System;
>>>>>>> origin/DEV
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



    [SerializeField] public IState currentState;
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

<<<<<<< HEAD
    public state STATE = state.NONE;

    private float maxStamina = 5f;
    private float staminaRegenDelay = 1.5f;
    private float sprintBarInitialWidth;


=======
>>>>>>> origin/DEV
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

        //staminaSettings.maxStamina = 5f;
        //staminaSettings.staminaRegenDelay = 1.5f;
        //staminaSettings.staminaLeft = 5f;
        //staminaSettings.staminaTimer = 0f;
        //staminaSettings.isOutOfStamina = false;
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
<<<<<<< HEAD
                moveInput = new Vector2(0, 0);
            }
            return;
        }

        if (isMoving == false)
        {
            if (crouchInput == true || isCeilingAbove == true)
            {
                ChangeState(CrouchState);
                STATE = state.NONE;
            }
            else if (isCeilingAbove == false)
            {
                ChangeState(IdleState);
                STATE = state.IDLE;
            }
                
            
        }
        else if (crouchInput == true)
        {
            ChangeState(CrouchState);
            STATE= state.NONE;
        }
        else if (sprintInput == true && isOutOfStamina == false)
        {
            ChangeState(SprintState);
            STATE = state.SPRINT;
        }
        else if (crouchInput == false && sprintInput == false)
        {
            if (isCeilingAbove == true)
            {
                ChangeState(CrouchState);
                STATE = state.NONE;
                return;
            }

            ChangeState(WalkState);
            STATE = state.WALK;
            
        }
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
=======
                staminaSettings.staminaTimer += Time.deltaTime;
>>>>>>> origin/DEV
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

    

