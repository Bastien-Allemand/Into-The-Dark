using UnityEngine;

public class GhostStateMachine : MonoBehaviour
{
    [SerializeField] public bool debug = false;
    [SerializeField] public IState currentState;
    private Pathfinding pathfinding;
    private MonsterVisionScript monsterVision;
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }

    [SerializeField] public Animator animator; 

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        monsterVision = GetComponent<MonsterVisionScript>();
        animator = GetComponentInChildren<Animator>();

        ChangeState(new GhostPatrolState(this,pathfinding));
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.speed = GameManager.Instance.activeDifficulty.speedGhost;
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision avec :" + other.tag );

        if (other.tag == "Player")
        {
            Debug.Log("Collision avec le joueur");
            ChangeState(new GhostDeathState(this, pathfinding));
            return;
        }

        if (other.tag != "Sound")
            return;

  
        pathfinding.target = other.transform;
        ChangeState(new GhostSearchState(this, pathfinding));
    }

    void HeardSound(Transform position)
    {

    }

    public void ChangeState(IState newState)
    {
        // 1. On quitte proprement l'ancien �tat s'il existe
        if (currentState != null)
        {
            currentState.Exit();
        }

        // 2. On attribue le nouvel �tat
        currentState = newState;

        // 3. On initialise le nouvel �tat
        currentState.Enter();
    }

}
