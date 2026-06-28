using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private CapsuleCollider playerCollider;
    private Vector3 ceilingCheckSize = new Vector3(0.6f, 2f, 0.6f);
    private bool isCeilingAbove = false;
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
        CheckIsCeilingAbove();

        bool crouchInput = InputManager.controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;

        if (!crouchInput && !stateMachine.crouchConfigs.isCeilingAbove)
        {
            if (stateMachine.moveInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.WalkState);
            }
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

    void CheckIsCeilingAbove()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Color rayColor = Color.green;
        float castLength = stateMachine.crouchConfigs.ceilingCheckDistance;

        var result = Raycast.CheckBoxCast(transform, ceilingCheckSize, Vector3.up, Quaternion.identity, castLength);

        bool hasCeiling = (result != null);

        if (hasCeiling)
        {
            stateMachine.crouchConfigs.isCeilingAbove = true;
            rayColor = Color.red;
        }
        else
        {
            stateMachine.crouchConfigs.isCeilingAbove = false;
        }
        Debug.DrawRay(origin, Vector3.up * castLength, rayColor);
    }
}
