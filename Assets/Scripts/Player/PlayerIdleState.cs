using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerStateMachine stateMachine;
    


    public PlayerIdleState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Player: Enter Mode IDLE");
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Player: Exit Mode IDLE");
    }
}
