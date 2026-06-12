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
    private Vector3 position = Vector3.zero;
    public Transform target;
    private NavMeshAgent agent;
    private float swapRoom = 20f;
    private int choiceroom = 0;
    private bool attracted = false;
    private float attractionTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Room room = new Room();
        room.max = new Vector3(-23.83f, 4f, 5.11f);
        room.min = new Vector3(-8.63f, 0f, 20.91f);
        rooms.Add(room);

        room.max = new Vector3(-7.16f, 4f, -0.84f);
        room.min = new Vector3(6.03f, 0f, 12.09f);
        rooms.Add(room);

        room.max = new Vector3(7.32f, 4f, -0.02f);
        room.min = new Vector3(19.75f, 0f, 12.08f);
        rooms.Add(room);

        room.max = new Vector3(7.3f, 4f, -16.48f);
        room.min = new Vector3(25.36f, 0f, -1.72f);
        rooms.Add(room);

        room.max = new Vector3(-26.42f, 4f, -16.09f);
        room.min = new Vector3(6.07f, 0f, -1.12f);
        rooms.Add(room);

        room.max = new Vector3(-25.72f, 4f, -2.58f);
        room.min = new Vector3(-8.369f, 0f, 3.648f);
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

    public void AttractToPlayer(Transform player, float duration)
    {
        target = player;
        attracted = true;
        attractionTimer = duration;
    }

    void Update()
    {
        if (attracted)
        {
            attractionTimer -= Time.deltaTime;

            agent.SetDestination(target.position);

            if (attractionTimer <= 0f)
            {
                attracted = false;
            }

            return;
        }

        swapRoom -= Time.deltaTime;

        if (swapRoom < 0f)
        {
            swapRoom = 20f;
            choiceroom = Random.Range(0, 5);
            Debug.Log("Room :" + choiceroom);
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            agent.SetDestination(RandomPosition(rooms[choiceroom]));
    }


}
