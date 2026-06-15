using UnityEngine;

public enum deathCause
{
    Caught,
    Insanity
}

public class PlayerDeathState : IState
{
    private PlayerStateMachine stateMachine;
    private Rigidbody rb;


    public PlayerDeathState(PlayerStateMachine stateMachine, Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.rb = rb;
    }
    public void Enter()
    {
        if (stateMachine.debug)
            Debug.Log("Player: Enter Mode DEATH");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver(deathCause.Insanity);
        }
    }

    public void Update()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    public void Exit() 
    {
        if (stateMachine.debug)
            Debug.Log("Player: Exit Mode DEATH");
    }
}
