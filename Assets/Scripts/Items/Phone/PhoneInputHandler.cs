using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private PhoneLightScript phoneLightScript;

    private PlayerAction controls;
    private Vector2 moveInput;
    private bool wasSwitchLastFrame;

    private void Awake()
    {
        controls = InputManager.controls;
    }

    private void OnEnable()
    {
        controls.PlayerInteraction.TakeHidePhone.started += OnTakeHidePhone;
        controls.PlayerInteraction.Lookatcamera.started += OnLookAtCamera;
        controls.PlayerInteraction.PhoneLight.started += HandleFlashlightInput;
    }

    private void OnDisable()
    {
        controls.PlayerInteraction.TakeHidePhone.started -= OnTakeHidePhone;
        controls.PlayerInteraction.Lookatcamera.started -= OnLookAtCamera;
        controls.PlayerInteraction.PhoneLight.started -= HandleFlashlightInput;
    }

    private void Update()
    {
        HandleCameraSwitchInput();
    }

    private void OnTakeHidePhone(InputAction.CallbackContext ctx)
    {
        if (phoneController == null) return;
        phoneController.TogglePhone();
        
    }

    private void OnLookAtCamera(InputAction.CallbackContext ctx)
    {
        if (phoneController == null) return;

        phoneController.ToggleCameraMode();
    }
    private void HandleCameraSwitchInput()
    {
        if (phoneController == null || CamerasScript.instance == null) return;

        if (phoneController.GetCurrentPhoneState() != PhoneState.Camera) return;

        moveInput = controls.GeneriqueMove.Movement.ReadValue<Vector2>();
        bool isSwapThisFrame = Mathf.Abs(moveInput.x) > 0.5f;

        if (isSwapThisFrame && !wasSwitchLastFrame)
        {
            if (moveInput.x > 0f)
            {
                CamerasScript.instance.NextCamera();
            }
            else if (moveInput.x < 0f)
            {
                CamerasScript.instance.PreviousCamera();
            }
        }

        wasSwitchLastFrame = isSwapThisFrame;
    }

    private void HandleFlashlightInput(InputAction.CallbackContext ctx)
    {
        if (phoneController == null || phoneLightScript == null) return;

        if (phoneController.GetCurrentPhoneState() == PhoneState.Idle)
        {
            phoneLightScript.ToggleFlashlight();
        }
    }
}                                         
