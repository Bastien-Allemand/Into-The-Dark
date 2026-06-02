using UnityEngine;

public class GhostSearchState : IState
{
    private GhostStateMachine stateMachine;

    public GhostSearchState(GhostStateMachine stateMachine)
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
