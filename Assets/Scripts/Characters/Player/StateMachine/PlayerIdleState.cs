using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    
    public PlayerIdleState(PlayerStateMachine sm, Rigidbody rb, Transform t) : base(sm, rb, t) { }
    public override void Enter()
    {
        if (stateMachine.debug)
            Debug.Log("Player: Enter Mode IDLE");
    }

    public override void Update()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }

        bool crouchInput = InputManager.controls.GamePlay.Crouch.ReadValue<float>() > 0.5f;
        bool walkInput = InputManager.controls.GamePlay.Movement.triggered;

        if (walkInput)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
        else if (crouchInput)
        {
            stateMachine.ChangeState(stateMachine.CrouchState);
        }
    }

    public override void Exit()
    {
        if (stateMachine.debug)
            Debug.Log("Player: Exit Mode IDLE");
    }
}
