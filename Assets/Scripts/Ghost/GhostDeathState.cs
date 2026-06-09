using UnityEngine;

public class GhostDeathState : IState
{
    private GhostStateMachine stateMachine;
    private Pathfinding pathfinding;

    public GhostDeathState(GhostStateMachine stateMachine, Pathfinding pathfinding)
    {
        this.stateMachine = stateMachine;
        this.pathfinding = pathfinding;
    }

    public void Enter()
    {
        if (stateMachine.debug)
            Debug.Log("Ghost: Mode DEATH");
    }

    public void Update()
    {
    }

    public void Exit()
    {
        if (stateMachine.debug)
            Debug.Log("Ghost: Exit Mode DEATH.");
    }
}
