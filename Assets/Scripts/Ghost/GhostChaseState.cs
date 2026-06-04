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
        Debug.Log("Ghost: Mode CHASE");
    }

    public void Update()
    {
        if (pathfinding.target == null)
            stateMachine.ChangeState(new GhostPatrolState(stateMachine, pathfinding));

        pathfinding.agent.SetDestination(pathfinding.target.position);
    }

    public void Exit()
    {
        Debug.Log("Ghost: Exit Mode CHASE.");
    }
}
