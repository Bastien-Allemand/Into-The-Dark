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
    }

    public void Update()
    {
        swapRoom -= Time.deltaTime;
        if (swapRoom < 0f)
        {
            swapRoom = 20f;
            choiceroom = Random.Range(0, pathfinding.rooms.Count);
            if (stateMachine.debug)
                Debug.Log("Room :" + choiceroom);
        }
        //if (!pathfinding.agent.pathPending && pathfinding.agent.remainingDistance < 0.5f)
        //{ 
        //    Vector3 pos = RandomPosition(pathfinding.rooms[choiceroom]);
        //    pathfinding.agent.SetDestination(pos);
        //    if (stateMachine.debug)
        //        Debug.Log("Pos :" +  pos);
        //}
    }

    public void Exit()
    {
       if (stateMachine.debug)
            Debug.Log("Ghost: Exit Mode PATROL.");
    }

    Vector3 RandomPosition(Room _room)
    {
        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(_room.min.x, _room.max.x);
        pos.y = Random.Range(_room.min.y, _room.max.y);
        pos.z = Random.Range(_room.min.z, _room.max.z);
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