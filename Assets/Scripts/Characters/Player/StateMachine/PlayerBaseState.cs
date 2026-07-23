using UnityEngine;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected Rigidbody rb;
    protected Transform transform;
    private Vector3 velocity = Vector3.zero;

    public PlayerBaseState(PlayerStateMachine stateMachine, Rigidbody rb, Transform transform)
    {
        this.stateMachine = stateMachine;
        this.rb = rb;
        this.transform = transform;
    }

    public virtual void Enter()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void Exit()
    {

    }

    protected void Move(float speed)
    {
        Vector3 targetVel = Vector3.zero;
        if (stateMachine.moveInput != Vector2.zero)
        {
            targetVel = (transform.forward * stateMachine.moveInput.y + transform.right * stateMachine.moveInput.x) * speed;
        }

        Vector3 currentVel = rb.linearVelocity;
        Vector3 desiredVel = new Vector3(targetVel.x, currentVel.y, targetVel.z);

        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, desiredVel, ref velocity, 0.05f, Mathf.Infinity, Time.deltaTime);

    }
}
