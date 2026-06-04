using UnityEngine;

public class GhostChaseState : IState
{
    private GhostStateMachine stateMachine;
    private Pathfinding pathfinding;

    public GhostChaseState(GhostStateMachine stateMachine, Pathfinding pathfinding)
    {
        this.stateMachine = stateMachine;
        this.pathfinding = pathfinding;
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
