using UnityEngine;

public class PlayerCrouchState : IState
{
    private PlayerStateMachine stateMachine;
    private Transform transform;
    private CapsuleCollider playerCollider;
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    public PlayerCrouchState(PlayerStateMachine stateMachine, Rigidbody rb, Transform transform, CapsuleCollider playerCollider)
    {
        this.stateMachine = stateMachine;
        this.rb = rb;
        this.transform = transform;
        this.playerCollider = playerCollider;
    }

    public void Enter()
    {
        Debug.Log("Player: Enter Mode CROUCH");

        playerCollider.height = stateMachine.crouchHeight;
        playerCollider.center = new Vector3(0f, stateMachine.crouchCenterY, 0f);
        stateMachine.currentSpeed = stateMachine.walkSpeed * stateMachine.crouchMultiplier;
    }

    public void Update()
    {
        Move();
    }

    void Move()
    {
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
        if (stateMachine.isCeilingAbove == true)
            return;

        Debug.Log("Player: Exit Mode CROUCH");

        playerCollider.height = stateMachine.standHeight;
        playerCollider.center = new Vector3(0f, stateMachine.standCenterY, 0f);
    }
}
