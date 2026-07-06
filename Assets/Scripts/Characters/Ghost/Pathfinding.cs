using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Room
{
    public Vector3 max;
    public Vector3 min;
}

public class Pathfinding : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    private Vector3 position = Vector3.zero;
    public Transform target;
    public NavMeshAgent agent { get; private set; }
    private bool attracted = false;
    private float attractionTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Room room0 = new Room();
        room0.max = new Vector3(-23.83f, 4f, 5.11f);
        room0.min = new Vector3(-8.63f, 0f, 20.91f);
        rooms.Add(room0);

        Room room1 = new Room();
        room1.max = new Vector3(-7.16f, 4f, -0.84f);
        room1.min = new Vector3(6.03f, 0f, 12.09f);
        rooms.Add(room1);

        Room room2 = new Room();
        room2.max = new Vector3(7.32f, 4f, -0.02f);
        room2.min = new Vector3(19.75f, 0f, 12.08f);
        rooms.Add(room2);

        Room room3 = new Room();
        room3.max = new Vector3(7.3f, 4f, -16.48f);
        room3.min = new Vector3(25.36f, 0f, -1.72f);
        rooms.Add(room3);

        Room room4 = new Room();
        room4.max = new Vector3(-26.42f, 4f, -16.09f);
        room4.min = new Vector3(6.07f, 0f, -1.12f);
        rooms.Add(room4);

        Room room5 = new Room();
        room5.max = new Vector3(-25.72f, 4f, -2.58f);
        room5.min = new Vector3(-8.369f, 0f, 3.648f);
        rooms.Add(room5); 

        agent = GetComponent<NavMeshAgent>();
    }
    public void AttractToPlayer(Transform player, float duration)
    {
        target = player;
        attracted = true;
        attractionTimer = duration;
    }
    // Update is called once per frame
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
    }

}
