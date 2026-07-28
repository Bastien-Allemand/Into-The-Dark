using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 120f;

    Vector2 moveInput = Vector2.zero;

    private PlayerAction controls;

    private void Awake()
    {
        controls = InputManager.controls;
    }

    private void OnEnable()
    {
        InputManager.controls.Global.MoveForward.performed += Forward;
        InputManager.controls.Global.MoveForward.canceled += ForwardCanceled;
        InputManager.controls.Global.MoveBackward.performed += Backward;
        InputManager.controls.Global.MoveBackward.canceled += BackwardCanceled;

        InputManager.controls.Global.MoveToLeft.performed += ToLeft;
        InputManager.controls.Global.MoveToLeft.canceled += ToLeftCanceled;

        InputManager.controls.Global.MoveToRight.performed += ToRight;
        InputManager.controls.Global.MoveToRight.canceled += ToRightCanceled;
    }

    private void OnDisable()
    {
        InputManager.controls.Global.MoveForward.performed -= Forward;
        InputManager.controls.Global.MoveForward.canceled -= ForwardCanceled;

        InputManager.controls.Global.MoveBackward.performed -= Backward;
        InputManager.controls.Global.MoveBackward.canceled -= BackwardCanceled;

        InputManager.controls.Global.MoveToLeft.performed -= ToLeft;
        InputManager.controls.Global.MoveToLeft.canceled -= ToLeftCanceled;

        InputManager.controls.Global.MoveToRight.performed -= ToRight;
        InputManager.controls.Global.MoveToRight.canceled -= ToRightCanceled;
    }

    private void Forward(InputAction.CallbackContext ctx)
    {
        moveInput.y = InputManager.controls.Global.MoveForward.ReadValue<float>();
    }
    private void ForwardCanceled(InputAction.CallbackContext ctx)
    {
        if (InputManager.controls.Global.MoveBackward.IsPressed())
            return;
        moveInput.y = 0f;
    }
    private void Backward(InputAction.CallbackContext ctx)
    {
        moveInput.y = -InputManager.controls.Global.MoveBackward.ReadValue<float>();
    }
    private void BackwardCanceled(InputAction.CallbackContext ctx)
    {
        if (InputManager.controls.Global.MoveForward.IsPressed())
            return;
        moveInput.y = 0f;
    }
    private void ToLeft(InputAction.CallbackContext ctx)
    {
        moveInput.x = -InputManager.controls.Global.MoveToLeft.ReadValue<float>();
    }
    private void ToLeftCanceled(InputAction.CallbackContext ctx)
    {

        if (InputManager.controls.Global.MoveToRight.IsPressed())
            return;
        moveInput.x = 0;
    }
    private void ToRight(InputAction.CallbackContext ctx)
    {
        moveInput.x = InputManager.controls.Global.MoveToRight.ReadValue<float>();
    }
    private void ToRightCanceled(InputAction.CallbackContext ctx)
    {
        if (InputManager.controls.Global.MoveToLeft.IsPressed())
            return;
        moveInput.x = 0;
    }

    private void Update()
    {

        transform.position += transform.forward * moveInput.y * moveSpeed * Time.deltaTime;

        transform.Rotate(0f, moveInput.x * rotationSpeed * Time.deltaTime, 0f);

        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }
}