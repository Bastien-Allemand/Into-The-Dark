using UnityEngine;

public class GhostChaseState : IState
{
    private GhostStateMachine stateMachine;

    public GhostChaseState(GhostStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("Ghost: Mode IDLE");
    }

    public void Update()
    {
    }

    public void Exit()
    {
        Debug.Log("Ghost: Exit Mode IDLE.");
    }
}
