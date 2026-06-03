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
    }

    // Update is called once per frame
    void Update()
    {
    }

}
