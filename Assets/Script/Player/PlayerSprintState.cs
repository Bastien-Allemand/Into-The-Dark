using UnityEngine;

public class PlayerSprintState : IState
{
    private PlayerStateMachine stateMachine; 
    private Transform transform;
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    public PlayerSprintState(PlayerStateMachine stateMachine, Rigidbody rb, Transform transform)
    {
        this.stateMachine = stateMachine;
        this.rb = rb;
        this.transform = transform;
    }
    
    public void Enter()
    {
        if (stateMachine.debug)
            Debug.Log("Player: Enter Mode SPRINT");
        stateMachine.currentSpeed = stateMachine.walkSpeed * stateMachine.sprintingMultiplier;
    }

    public void Update()
    {
        Move();
        DecreaseStamina();
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
    void DecreaseStamina()
    {
        stateMachine.staminaTimer = 0f;
        stateMachine.staminaLeft -= Time.deltaTime;

        if (stateMachine.staminaLeft <= 0f)
        {
            stateMachine.staminaLeft = 0f;
            stateMachine.isOutOfStamina = true;
        }
    }

    public void Exit()
    {
        if (stateMachine.debug)
            Debug.Log("Player: Exit Mode SPRINT");
    }
}

