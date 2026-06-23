using UnityEngine;

public class ToRemove : MonoBehaviour
{
    private float speed = 5f;
    [SerializeField] private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    float dir = 1;
    void Start()
    {
        transform.position = new Vector3(0, 13, 25);     
    }

    void Update()
    {


        Vector3 targetVel = Vector3.zero;
        if (transform.position.x >= 15f)
        {
            dir = -dir;
        }
        else if (transform.position.x <= 0f)
        {
            dir = -dir;
        }

        targetVel = new Vector3(dir, 0, 0) * speed;

        Vector3 currentVel = rb.linearVelocity;
        Vector3 desiredVel = new Vector3(targetVel.x, currentVel.y, targetVel.z);

        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, desiredVel, ref velocity, 0.05f, Mathf.Infinity, Time.fixedDeltaTime);

        
    }
}
