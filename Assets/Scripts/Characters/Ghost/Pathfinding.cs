using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class Room
{
    public Transform max;
    public Transform min;
}

public class Pathfinding : MonoBehaviour
{
    [SerializeField] public List<Room> rooms = new List<Room>();
    private Vector3 position = Vector3.zero;
    public Transform target;
    public NavMeshAgent agent { get; private set; }
    private bool attracted = false;
    private float attractionTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
