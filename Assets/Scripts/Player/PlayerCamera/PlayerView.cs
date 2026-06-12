using UnityEngine;

public class PlayerView : MonoBehaviour
{
    PlayerAction controls;

    [Header("Reference")]
    [SerializeField] private Camera _camera;
    [SerializeField] private PickUpScript pickUpScript;

    [Space(5)]

    [Header("Camera Settings")]
    
    [SerializeField] private float sensitivity = 0.5f;
    [SerializeField] private float XMaxAngle = 75f;
    private Vector2 targetRotation;



    private void Awake()
    {
        controls = InputManager.controls;
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (pickUpScript != null && pickUpScript.IsRotating == true)
            return;
       Look();
    }

    private void FixedUpdate()
    {
        RotatePlayer();
    }

    void Look()
    {
        Vector2 mouse = controls.GamePlay.Look.ReadValue<Vector2>() * sensitivity;

        targetRotation.x -= mouse.y;
        targetRotation.y += mouse.x;
        targetRotation.x = Mathf.Clamp(targetRotation.x, -XMaxAngle, XMaxAngle);

        _camera.transform.localRotation = Quaternion.Euler(targetRotation.x, 0, 0);
    }

    void RotatePlayer()
    {
        transform.localRotation = Quaternion.Euler(0, targetRotation.y, 0);
    }
}
