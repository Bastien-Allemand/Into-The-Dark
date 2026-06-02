using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;
public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    private float moveInputX;
    private float moveInputZ;
   
    private Vector3 velocity = Vector3.zero;

    [Space(5)]

    [Header("Crouch Settings")]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float standCenterY = 0f;
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float crouchCenterY = -0.2f;
    [SerializeField] private float ceilingCheckDistance = 1.0f;
    [Space(10)]

    [Header("Reference")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private Transform cameraTransform;

    [Space(10)]
    [Header("State (Debug)")]
    [SerializeField] private bool isCrouched = false;
    [SerializeField] private bool isCeilingAbove = false;

    //TO REMOVE
    private float crouchScale;
    private Vector3 initialScale;

    LayerMask layerMask;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //cameraTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
        initialScale = transform.localScale;
        crouchScale = initialScale.y * 0.65f;
    }
    void Awake()
    {
        layerMask = LayerMask.GetMask("Ceiling");

        if (rb == null) rb = GetComponent<Rigidbody>();
        if (playerCollider == null) playerCollider = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        //cameraTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);

        moveInputX = 0f;
        moveInputZ = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                moveInputX = 1f;
            else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                moveInputX = -1f;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                moveInputZ = 1f;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                moveInputZ = -1f;

            if(Keyboard.current.leftCtrlKey.isPressed)
            {
                Crouch();
            }
            
            if(Keyboard.current.leftCtrlKey.wasReleasedThisFrame)
            {
                StandUp();
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 moveTarget = (transform.forward * moveInputZ + transform.right * moveInputX) * speed;
        Move(moveTarget);

        Vector3 origin = transform.position + new Vector3(0f, playerCollider.center.y, 0f);

        Color rayColor = Color.green;

        float castLength = (standHeight / 2f) + ceilingCheckDistance;

        if (Physics.Raycast(origin, Vector3.up, castLength, layerMask))
        {
            isCeilingAbove = true;
            rayColor = Color.red; 
        }
        else
        {
            isCeilingAbove = false;
        }

        Debug.DrawRay(origin, Vector3.up * castLength, rayColor);
    }

    void Move(Vector3 targetVel)
    {
        Vector3 currentVel = rb.linearVelocity;
        Vector3 desiredVel = new Vector3(targetVel.x, currentVel.y, targetVel.z);
        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVel, ref velocity, 0.05f);
    }

    void Crouch()
    {
        if(isCrouched == false)
        { 
           //To remove
           transform.localScale = new Vector3(initialScale.x, crouchScale, initialScale.z);
            //
            playerCollider.height = crouchHeight;
            playerCollider.center = new Vector3(0f, crouchCenterY, 0f);
            isCrouched = true;
        }
        
    }

    void StandUp()
    {
        if (isCeilingAbove) return;

        if (isCrouched == true)
        {
                //To remove
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            //
            playerCollider.height = standHeight;
            playerCollider.center = new Vector3(0f, standCenterY, 0f);
            isCrouched = false;
        }
        
    }
}

