using UnityEngine;

public class GhostPatrolState : IState
{
    private GhostStateMachine stateMachine;

    public GhostPatrolState(GhostStateMachine stateMachine)
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
