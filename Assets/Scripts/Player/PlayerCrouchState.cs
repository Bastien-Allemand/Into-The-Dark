using UnityEngine;

public class PlayerCrouchState : IState
{
    private PlayerStateMachine stateMachine;
    private Transform transform;
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    public PlayerCrouchState(PlayerStateMachine stateMachine, Rigidbody rb, Transform transform)
    {
        this.stateMachine = stateMachine;
        this.rb = rb;
        this.transform = transform;
    }

    public void Enter()
    {
        Debug.Log("Player: Enter Mode CROUCH");

        transform.localScale = new Vector3(1f, 0.5f, 1f);
    }


    public void Update()
    {
        Debug.Log("Player: Update Mode CROUCH");
        Vector3 targetVel = Vector3.zero;
        if (stateMachine.moveInput != Vector2.zero)
        {
            float crouchSpeed = stateMachine.currentSpeed * 0.5f;

            targetVel = (transform.forward * stateMachine.moveInput.y + transform.right * stateMachine.moveInput.x) * crouchSpeed;
        }

        Vector3 currentVel = rb.linearVelocity;
        Vector3 desiredVel = new Vector3(targetVel.x, currentVel.y, targetVel.z);

        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, desiredVel, ref velocity, 0.05f, Mathf.Infinity, Time.fixedDeltaTime);
    }

    public void Exit()
    {
        Debug.Log("Player: Exit Mode CROUCH");

        transform.localScale = Vector3.one;
    }
}
