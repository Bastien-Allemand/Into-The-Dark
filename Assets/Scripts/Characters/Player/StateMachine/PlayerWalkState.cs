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
        Move(stateMachine.moveConfigs.walkSpeed);

      
        bool sprintInput = InputManager.controls.PlayerMoves.Sprint.ReadValue<float>() > 0.5f;
        bool crouchInput = InputManager.controls.PlayerMoves.Crouch.ReadValue<float>() > 0.5f;

        if (stateMachine.moveInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else if (sprintInput && stateMachine.staminaConfigs.isOutOfStamina == false)
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