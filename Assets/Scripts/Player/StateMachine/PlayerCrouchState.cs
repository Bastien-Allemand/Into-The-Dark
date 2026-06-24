using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private CapsuleCollider playerCollider;
    public PlayerCrouchState(PlayerStateMachine stateMachine, Rigidbody rb, Transform transform, CapsuleCollider playerCollider)
        : base(stateMachine, rb, transform)
    {
        this.playerCollider = playerCollider;
    }
    public override void Enter()
    {
        if (stateMachine.debug)
            Debug.Log("Player: Enter Mode CROUCH");

        if (playerCollider != null)
        {
            playerCollider.height = stateMachine.crouchHeight;
            playerCollider.center = new Vector3(0f, stateMachine.crouchCenterY, 0f);
        }

        stateMachine.currentSpeed = stateMachine.walkSpeed * stateMachine.crouchMultiplier;
    }

    public override void Update()
    {
       
        Move(stateMachine.currentSpeed);

        bool crouchInput = InputManager.controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;

        if (stateMachine.moveInput == Vector2.zero && !crouchInput && !stateMachine.isCeilingAbove)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else if (!crouchInput && !stateMachine.isCeilingAbove)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
    public override void Exit()
    {
        
        if (stateMachine.isCeilingAbove)
            return;

        if (stateMachine.debug)
            Debug.Log("Player: Exit Mode CROUCH");

        if (playerCollider != null)
        {
            playerCollider.height = stateMachine.standHeight;
            playerCollider.center = new Vector3(0f, stateMachine.standCenterY, 0f);
        }
    }
}
