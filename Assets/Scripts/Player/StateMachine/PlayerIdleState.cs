using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerStateMachine stateMachine;
    private Rigidbody rb;

    
    public PlayerIdleState(PlayerStateMachine stateMachine, Rigidbody rb)
    {
        this.stateMachine = stateMachine;
        this.rb = rb;
    }
    public void Enter()
    {
        if (stateMachine.debug)
            Debug.Log("Player: Enter Mode IDLE");
    }

    public void Update()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    public void Exit()
    {
        if (stateMachine.debug)
            Debug.Log("Player: Exit Mode IDLE");
    }
}
