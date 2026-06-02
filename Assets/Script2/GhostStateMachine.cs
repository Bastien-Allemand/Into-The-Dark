using UnityEngine;

public class GhostStateMachine : MonoBehaviour
{
    private IState currentState;

    void Start()
    {
        ChangeState(new GhostIdleState(this));
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
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
