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
        if (stateMachine.debug)
            Debug.Log("Ghost: Mode SEARCH");

        if (pathfinding.target != null)
        {
            stateMachine.animator.SetInteger("State", 1); // Search

            pathfinding.agent.SetDestination(pathfinding.target.position);
        }
    }

    public void Update()
    {
        if (pathfinding.target == null)
            stateMachine.ChangeState(new GhostPatrolState(stateMachine, pathfinding));

        if (!pathfinding.agent.pathPending && pathfinding.agent.remainingDistance < 0.5f)

        {
            stateMachine.ChangeState(new GhostPatrolState(stateMachine, pathfinding));
            return;
        }

        pathfinding.agent.SetDestination(pathfinding.target.position);
    }

    public void Exit()
    {
        if (stateMachine.debug)
            Debug.Log("Ghost: Exit Mode SEARCH.");
    }
}
