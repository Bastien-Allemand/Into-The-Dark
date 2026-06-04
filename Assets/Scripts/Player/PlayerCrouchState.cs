using UnityEngine;

public class PlayerCrouchState : IState
{

    private PlayerStateMachine stateMachine;

    public PlayerCrouchState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Player: Enter Mode Crouch");
    }


    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("Player: Exit Mode Crouch");
    }
}
