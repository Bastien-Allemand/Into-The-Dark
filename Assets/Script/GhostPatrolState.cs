using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class GhostPatrolState : IState
{
    private GhostStateMachine stateMachine;
    private Pathfinding pathfinding;

    public GhostPatrolState(GhostStateMachine stateMachine, Pathfinding pathfinding)
    {
        this.stateMachine = stateMachine;
        this.pathfinding = pathfinding;
    }

    public void Enter()
    {
        Debug.Log("Ghost: Mode PATROL");
    }

    public void Update()
    {
        if (!pathfinding.agent.pathPending && pathfinding.agent.remainingDistance < 0.5f)
        {
            pathfinding.agent.SetDestination(RandomPosition(pathfinding.rooms[pathfinding.actualRoom]));
            Debug.Log("Room : " + pathfinding.actualRoom);
        }
        else
            pathfinding.actualRoom = (int)(Random.value % pathfinding.rooms.Count);
    }

    public void Exit()
    {
        Debug.Log("Ghost: Exit Mode PATROL.");
    }

    Vector3 RandomPosition(Room _room)
    {
        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(_room.min.x, _room.max.x);
        pos.y = Random.Range(_room.max.y, _room.min.y);
        pos.z = Random.Range(_room.min.z, _room.max.z);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(pos, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return pos;
    }
}