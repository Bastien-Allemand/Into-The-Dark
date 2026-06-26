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
            playerCollider.height = stateMachine.crouchConfigs.crouchHeight;
            playerCollider.center = new Vector3(0f, stateMachine.crouchConfigs.crouchCenterY, 0f);
        }

        stateMachine.currentSpeed = stateMachine.moveConfigs.walkSpeed * stateMachine.moveConfigs.crouchMultiplier;
    }

    public override void Update()
    {
       
        Move(stateMachine.currentSpeed);

        bool crouchInput = InputManager.controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;

        if (stateMachine.moveInput == Vector2.zero && !crouchInput && !stateMachine.crouchConfigs.isCeilingAbove)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else if (!crouchInput && !stateMachine.crouchConfigs.isCeilingAbove)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
    public override void Exit()
    {
        
        if (stateMachine.crouchConfigs.isCeilingAbove)
            return;

        if (stateMachine.debug)
            Debug.Log("Player: Exit Mode CROUCH");

        if (playerCollider != null)
        {
            playerCollider.height = stateMachine.crouchConfigs.standHeight;
            playerCollider.center = new Vector3(0f, stateMachine.crouchConfigs.standCenterY, 0f);
        }
    }
}
