using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerAction controls;

    private IState currentState;

    public enum PlayerState
    {
        IDLE,
        WALKING,
        CROUCHING,
        SPRINTING
    };

    [SerializeField] private PlayerState state;

    private Vector2 moveInput;

    [SerializeField] private bool isCeilingAbove = false;

    void Start()
    {
        ChangeState(new PlayerIdleState(this));
        state = PlayerState.IDLE;
    }

    void Awake()
    {
        controls = new PlayerAction();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    void Update()
    {
        moveInput = controls.GamePlay.Move.ReadValue<Vector2>();
        bool sprintInput = controls.GamePlay.Sprint.ReadValue<float>() > 0.5f;
        bool crouchInput = controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;
        bool isMoving = moveInput != Vector2.zero;

        if (crouchInput == true)
        {
            ChangeState(new PlayerCrouchState(this));
            state = PlayerState.CROUCHING;
            return;
        }
        if (isMoving == false)
        {
            ChangeState(new PlayerIdleState(this));
            state = PlayerState.IDLE;
        }
        else if (sprintInput == true)
        {
            ChangeState(new PlayerSprintState(this));
            state = PlayerState.SPRINTING;
        }
        else
        {
            ChangeState(new PlayerWalkState(this));
            state = PlayerState.WALKING;
        }


    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter();
    }
}
