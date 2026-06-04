using UnityEngine;

public class PlayerWalkState : IState
{

    private PlayerStateMachine stateMachine;

    public PlayerWalkState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Player: Enter Mode WALK");
    }

    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("Player: Exit Mode WALK");
    }
}
