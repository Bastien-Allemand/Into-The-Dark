using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerStateMachine sm, Rigidbody rb, Transform t) : base(sm, rb, t) { }
    
    public override void Enter()
    {
        if (stateMachine.debug)
            Debug.Log("Sprint enter");
        stateMachine.currentSpeed = stateMachine.walkSpeed * stateMachine.sprintingMultiplier;
    }

    public override void Update()
    {
        Move(stateMachine.currentSpeed);
        DecreaseStamina();


        bool sprintInput = InputManager.controls.GamePlay.Sprint.ReadValue<float>() > 0.5f;

        if (stateMachine.moveInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else if (sprintInput  == false || stateMachine.isOutOfStamina == true)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
    void DecreaseStamina()
    {
        stateMachine.staminaTimer = 0f;
        stateMachine.staminaLeft -= Time.deltaTime;

        if (stateMachine.staminaLeft <= 0f)
        {
            stateMachine.staminaLeft = 0f;
            stateMachine.isOutOfStamina = true;
        }
    }

    public override void Exit()
    {
        stateMachine.currentSpeed = stateMachine.walkSpeed;
        if (stateMachine.debug) Debug.Log("Player: Exit Mode SPRINT");
    }
}

