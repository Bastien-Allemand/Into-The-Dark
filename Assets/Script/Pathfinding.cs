using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

struct Room
{
    public Vector3 max;
    public Vector3 min;
}

public class Pathfinding : MonoBehaviour
{
    private List<Room> rooms = new List<Room>();
    private bool moving = false;
    private Vector3 position = Vector3.zero;
    public Transform target;
    private NavMeshAgent agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Room room = new Room();
        room.max = new Vector3(-19.08f, 2.11f, 23.16f);
        room.min = new Vector3(-46.57f, 0f, 9.14f);
        rooms.Add(room);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(RandomPosition(rooms[0]));

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
    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            agent.SetDestination(RandomPosition(rooms[0]));

    }


}
