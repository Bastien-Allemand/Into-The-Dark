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
        if (stateMachine.debug)
            Debug.Log("Ghost: Mode CHASE");

        stateMachine.animator.SetInteger("State", 2); // Chase
    }

    public void Update()
    {
        if (pathfinding == null)
            return;
        if (pathfinding.target == null)
        {
            Debug.Log("Target is null, switching to patrol state.");
            stateMachine.ChangeState(new GhostPatrolState(stateMachine, pathfinding));
            return;
        }
        pathfinding.agent.SetDestination(pathfinding.target.position);
    }

    public void Exit()
    {
        if (stateMachine.debug)
            Debug.Log("Ghost: Exit Mode CHASE.");
    }
}
