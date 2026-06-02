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
    public int actualRoom;
    public Transform target;
    public NavMeshAgent agent { get; private set; }

    private Vector3 position = Vector3.zero;
    private bool moving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actualRoom = 0;
        Room room = new Room();
        room.max = new Vector3(-19.08f, 2.11f, 23.16f);
        room.min = new Vector3(-46.57f, 0f, 9.14f);
        rooms.Add(room);
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
    }

}
