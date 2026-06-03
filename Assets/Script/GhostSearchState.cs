using UnityEngine;
using UnityEngine.AI;

public class GhostSearchState : IState
{
    private GhostStateMachine stateMachine;
    private Pathfinding pathfinding;

    public GhostSearchState(GhostStateMachine stateMachine, Pathfinding pathfinding)
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
        if (!pathfinding.agent.pathPending && pathfinding.agent.remainingDistance < 0.5f)
        {
            pathfinding.agent.SetDestination(pathfinding.target.localPosition);
        }
    }

    public void Exit()
    {
        Debug.Log("Ghost: Exit Mode IDLE.");
    }
}
