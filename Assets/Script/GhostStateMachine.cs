using UnityEngine;

public class GhostStateMachine : MonoBehaviour
{
    private IState currentState;
    private Pathfinding pathfinding;

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        ChangeState(new GhostPatrolState(this,pathfinding));
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Collision.getComponent<GameObject>()
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
