using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerAction controls;
    [SerializeField] private IState currentState;

    private Vector2 moveInput;

    [SerializeField] private bool isCeilingAbove = false;

    void Start()
    {
        ChangeState(new PlayerIdleState(this));
    }

    void Update()
    {
        moveInput = controls.GamePlay.Move.ReadValue<Vector2>();
        bool sprintInput = controls.GamePlay.Sprint.ReadValue<float>() > 0.5f;
        bool crouchInput = controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;
        bool isMoving = moveInput != Vector2.zero;

        if (crouchInput == true)
        {
            ChangeState(new PlayerCrouchState(this));
            return;
        }
        if (isMoving == false)
        {
            ChangeState(new PlayerIdleState(this));
        }
        else if (sprintInput == true)
        {
            ChangeState(new PlayerSprintState(this));
        }
        else
        {
            ChangeState(new PlayerWalkState(this));
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
