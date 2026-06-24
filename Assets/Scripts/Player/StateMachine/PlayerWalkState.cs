using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine stateMachine, Rigidbody rb, Transform transform) : base(stateMachine, rb, transform) { }

    public override void Enter()
    {
        if (stateMachine.debug) Debug.Log("Enter WALK");
    }

    public override void Update()
    {
        Move(stateMachine.walkSpeed);

      
        bool sprintInput = InputManager.controls.GamePlay.Sprint.ReadValue<float>() > 0.5f;
        bool crouchInput = InputManager.controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;

        if (stateMachine.moveInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else if (sprintInput && stateMachine.isOutOfStamina == false)
        {
            stateMachine.ChangeState(stateMachine.SprintState);
        }
        else if (crouchInput)
        {
            stateMachine.ChangeState(stateMachine.CrouchState);
        }
    }

    public override void Exit() { }
}