using UnityEngine;

public class PlayerSprintState : IState
{

    private PlayerStateMachine stateMachine;

    public PlayerSprintState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
    public void Enter()
    {
        Debug.Log("Player: Enter Mode SPRINT");
    }

    public void Update()
    {
       
    }

    public void Exit()
    {
        Debug.Log("Player: Exit Mode SPRINT");
    }
}

