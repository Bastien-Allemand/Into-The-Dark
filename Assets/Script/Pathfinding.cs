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
        room.max = new Vector3(-15.02f, 4f, 1.81f);
        room.min = new Vector3(-5.89f, 0f, 11.17f);
        rooms.Add(room);

        room.max = new Vector3(-4.58f, 4f, -1.67f);
        room.min = new Vector3(3.48f, 0f, 5.67f);
        rooms.Add(room);

        room.max = new Vector3(4.98f, 4f, -1.52f);
        room.min = new Vector3(12.37f, 0f, 5.73f);
        rooms.Add(room);

        room.max = new Vector3(4.87f, 4f, -12.26f);
        room.min = new Vector3(16.61f, 0f, -3.04f);
        rooms.Add(room);

        room.max = new Vector3(-16.67f, 4f, -12.54f);
        room.min = new Vector3(-3.22f, 0f, -4.19f);
        rooms.Add(room);

        room.max = new Vector3(-16.67f, 4f, -3.45f);
        room.min = new Vector3(-6.14f, 0f, 0f);
        rooms.Add(room); 

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
    }

}
