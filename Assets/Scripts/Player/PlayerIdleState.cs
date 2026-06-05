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
        Debug.Log("Player: Enter Mode IDLE");
    }

    public void Update()
    {
        Debug.Log("Player: Update Mode IDLE");
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    public void Exit()
    {
        Debug.Log("Player: Exit Mode IDLE");
    }
}
