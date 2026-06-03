using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.UI.Image;
public class MovePlayer : MonoBehaviour
{
    private float initialSpeed;
    private float sprintTime;
    [SerializeField] private float staminaLeft = 5f;
    [SerializeField] private float speed;
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private float sprintingMultiplier;
    
    private bool isOutOfStamina = false;
    private bool canSprint = false;
    [SerializeField] private float staminaIncreasingTotalTime = 1.5f;
    private float staminaIncreasingCurrentTime = 0f;

    private bool movingX = false;
    private bool movingZ = false;
    
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
    [SerializeField] private RectTransform sprintBarTransform;
    private float sprintBarInitialWidth;

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
        initialSpeed = 5f;
        sprintingMultiplier = 1.4f;
        speed = initialSpeed;
        sprintTime = 5f;
        staminaLeft = sprintTime;
        sprintBarInitialWidth = sprintBarTransform.rect.width;
    }
    void Awake()
    {
        layerMask = LayerMask.GetMask("Ceiling");

        if (rb == null) rb = GetComponent<Rigidbody>();
        if (playerCollider == null) playerCollider = GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
           
    }
    void Update()
    {
        //cameraTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
        moveInputX = 0f;
        moveInputZ = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                moveInputX = 1f;
                movingX = true;
            }
            else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                moveInputX = -1f;
                movingX = true;
            }
            else
                movingX = false;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                moveInputZ = 1f;
                movingZ = true;
            }
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                moveInputZ = -1f;
                movingZ = true;
            }
            else
                movingZ = false;

            if (movingX || movingZ)
                canSprint = true;
            else
            {
                canSprint = false;
                StopSprinting();
            }

            if (Keyboard.current.cKey.isPressed)
            {
                Crouch();
                canSprint = false;
                StopSprinting();
            }
            
            if(Keyboard.current.cKey.wasReleasedThisFrame)
            {
                StandUp();
                canSprint = true;
            }

            if(Keyboard.current.leftShiftKey.isPressed)
            {
                StartSprinting();
            }

            if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
            {
                StopSprinting();
            }

            if (isSprinting)
            {

               
                staminaIncreasingCurrentTime = 0f;
                DecreaseStamina();
                if (staminaLeft <= 0f)
                {
                    staminaLeft = 0f;
                    StopSprinting();
                    isOutOfStamina = true;
                }
            }

            if (isSprinting == false)
            {
                if (isOutOfStamina)
                {
                    if (staminaLeft > 3f)
                        isOutOfStamina = false;
                }
                if (staminaLeft < 5f)
                {
                    if (staminaIncreasingCurrentTime < staminaIncreasingTotalTime)
                        staminaIncreasingCurrentTime += Time.deltaTime;
                    else if (staminaIncreasingCurrentTime >= staminaIncreasingTotalTime)
                        IncreaseStamina();
                        
                }
                if (staminaLeft >= 5f)
                {
                    staminaLeft = 5f;
                }
            }

                
        }
    }

    void FixedUpdate()
    {
        Vector3 moveTarget = (transform.forward * moveInputZ + transform.right * moveInputX) * speed;
        Move(moveTarget);

        CheckIsCeilingAbove();
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
    void CheckIsCeilingAbove()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        Color rayColor = Color.green;

        float castLength = standHeight + ceilingCheckDistance;

        if (Physics.Raycast(origin, Vector3.up, castLength, layerMask))
        {
            isCeilingAbove = true;
            rayColor = Color.red;
        }
        else
        {
            isCeilingAbove = false;
        }

        Debug.DrawRay(origin, Vector3.up, rayColor);
    }

    void StartSprinting()
    {
        if (canSprint == false)
            return;
        if (isOutOfStamina)
            return;
        if ( isSprinting == false)
        {
            speed = initialSpeed * sprintingMultiplier;
            isSprinting = true;
        }
    }

    void StopSprinting()
    {
        if (isSprinting == true)
        {
            speed = initialSpeed;
            isSprinting = false;
        }
    }

    void DecreaseStamina()
    {
        staminaLeft -= Time.deltaTime;
        float purcentLeft = (staminaLeft / sprintTime);
        sprintBarTransform.sizeDelta = new Vector2( sprintBarInitialWidth * purcentLeft, sprintBarTransform.rect.height);
    }

    void IncreaseStamina()
    {
        staminaLeft += Time.deltaTime / 2;
        float purcentLeft = (staminaLeft / sprintTime);
        sprintBarTransform.sizeDelta = new Vector2( sprintBarInitialWidth * purcentLeft, sprintBarTransform.rect.height);
    }
}

