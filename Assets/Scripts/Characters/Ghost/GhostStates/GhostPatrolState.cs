using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class GhostPatrolState : IState
{    
    private GhostStateMachine stateMachine;
    private Pathfinding pathfinding;
    private float swapRoom = 20f;
    private int choiceroom = 0;

    public GhostPatrolState(GhostStateMachine stateMachine, Pathfinding pathfinding)
    {
        this.stateMachine = stateMachine;
        this.pathfinding = pathfinding;
    }

    public void Enter()
    {
        if (stateMachine.debug)
            Debug.Log("Ghost: Mode PATROL");

        stateMachine.animator.SetBool("Patrol", true);
    }

    public void Update()
    {

        Debug.Log("MovePlayer Update");

        swapRoom -= Time.deltaTime;
        if (swapRoom < 0f)
        {
            swapRoom = 20f;
            choiceroom = Random.Range(0, pathfinding.rooms.Count);
            if (stateMachine.debug)
                Debug.Log("Room :" + choiceroom);
        }
        if (!pathfinding.agent.pathPending && pathfinding.agent.remainingDistance < 0.5f)
        {
            Vector3 pos = RandomPosition(pathfinding.rooms[choiceroom]);
            pathfinding.agent.SetDestination(pos);
            if (stateMachine.debug)
                Debug.Log("Pos :" + pos);
        }
    }

    public void Exit()
    {
       if (stateMachine.debug)
            Debug.Log("Ghost: Exit Mode PATROL.");

        stateMachine.animator.SetInteger("State", 0); // Patrol
    }

        Vector3 RandomPosition(Room _room)
    {
        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(_room.min.position.x, _room.max.position.x);
        pos.y = Random.Range(_room.min.position.y, _room.max.position.y);
        pos.z = Random.Range(_room.min.position.z, _room.max.position.z);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(pos, out hit, 5f, NavMesh.AllAreas))
        {
            pos = hit.position;
        }
        if (stateMachine.debug)
            Debug.Log("Pos Random :" + pos);
        return pos;
    }
}