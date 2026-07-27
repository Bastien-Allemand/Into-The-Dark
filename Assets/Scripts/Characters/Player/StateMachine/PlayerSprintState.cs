using System;
using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerStateMachine sm, Rigidbody rb, Transform t) : base(sm, rb, t) { }
    
    public override void Enter()
    {
        if (stateMachine.debug)
            Debug.Log("Sprint enter");
        stateMachine.currentSpeed = stateMachine.moveConfigs.walkSpeed * stateMachine.moveConfigs.sprintingMultiplier;
    }

    public override void Update()
    {
        Move(stateMachine.currentSpeed);
        DecreaseStamina();


        bool sprintInput = InputManager.controls.PlayerUniqueMove.Sprint.ReadValue<float>() > 0.5f;

        if (stateMachine.moveInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else if (sprintInput == false || stateMachine.staminaConfigs.isOutOfStamina == true)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
    void DecreaseStamina()
    {
        stateMachine.staminaConfigs.staminaTimer = 0f;
        stateMachine.staminaConfigs.staminaLeft -= Time.deltaTime;

        if (stateMachine.staminaConfigs.staminaLeft <= 0f)
        {
            stateMachine.staminaConfigs.staminaLeft = 0f;
            stateMachine.staminaConfigs.isOutOfStamina = true;
        }
    }

    public override void Exit()
    {
        stateMachine.currentSpeed = stateMachine.moveConfigs.walkSpeed;
        if (stateMachine.debug) Debug.Log("Player: Exit Mode SPRINT");
    }
}

