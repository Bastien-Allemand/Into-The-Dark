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
        Debug.Log("Ghost: Mode DEATH");
    }

    public void Update()
    {
        Debug.Log("Ghost: Update Mode DEATH.");
    }

    public void Exit()
    {
        Debug.Log("Ghost: Exit Mode DEATH.");
    }
}
