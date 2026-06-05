using UnityEngine;

public class PlayerWalkState : IState
{
    private PlayerStateMachine stateMachine;
    private Transform transform;
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    public PlayerWalkState(PlayerStateMachine stateMachine, Rigidbody rb, Transform transform)
    {
        this.stateMachine = stateMachine;
        this.rb = rb;
        this.transform = transform;
    }

    public void Enter()
    {
        Debug.Log("Player: Enter Mode WALK");
    }

    public void Update()
    {
        Debug.Log("Player: Update Mode WALK");
        Vector3 targetVel = Vector3.zero;
        if (stateMachine.moveInput != Vector2.zero)
        {
            targetVel = (transform.forward * stateMachine.moveInput.y + transform.right * stateMachine.moveInput.x) * stateMachine.currentSpeed;
        }

        Vector3 currentVel = rb.linearVelocity;
        Vector3 desiredVel = new Vector3(targetVel.x, currentVel.y, targetVel.z);

        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, desiredVel, ref velocity, 0.05f, Mathf.Infinity, Time.fixedDeltaTime);
    }

    public void Exit()
    {
        Debug.Log("Player: Exit Mode WALK");
    }
}
