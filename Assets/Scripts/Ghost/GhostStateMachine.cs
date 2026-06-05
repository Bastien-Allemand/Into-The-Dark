using UnityEngine;

public class GhostStateMachine : MonoBehaviour
{
    private IState currentState;
    private Pathfinding pathfinding;
    private MonsterVisionScript monsterVision;

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        monsterVision = GetComponent<MonsterVisionScript>();
        ChangeState(new GhostPatrolState(this,pathfinding));
    }

    void Update()
    {
        if (currentState is GhostDeathState)
        {
            return;
        }
        if (monsterVision.PlayerFound && currentState is not GhostChaseState)
        {
            ChangeState(new GhostChaseState(this, pathfinding));
        }
        if (monsterVision.targetObj)
        {
            if (monsterVision.PlayerInFrustum() && monsterVision.RayConnectToPlayer())
            {
                pathfinding.target = monsterVision.targetObj.transform;
            }
        }
        if (!monsterVision.PlayerFound)
        {
            ChangeState(new GhostPatrolState(this, pathfinding));
        }
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision détectée avec : " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Sound"))
        {
            Debug.Log("Collision avec un son");
            pathfinding.target = collision.transform;
            ChangeState(new GhostSearchState(this, pathfinding));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger détecté avec : " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision avec le joueur");
            ChangeState(new GhostDeathState(this, pathfinding));
        }
    }

    public void ChangeState(IState newState)
    {
        // 1. On quitte proprement l'ancien état s'il existe
        if (currentState != null)
        {
            currentState.Exit();
        }

        // 2. On attribue le nouvel état
        currentState = newState;

        // 3. On initialise le nouvel état
        currentState.Enter();
    }

}
