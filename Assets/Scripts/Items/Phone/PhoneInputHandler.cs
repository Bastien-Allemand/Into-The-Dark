using Assets.Scripts.Items;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private PhoneLightScript phoneLightScript;

    private PlayerAction controls;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = InputManager.controls;
    }

    private void OnEnable()
    {

        controls.CameraMoves.NextCamera.started += NextCamera;
        controls.CameraMoves.PreviousCamera.started += PreviousCamera;
        controls.PlayerMoves.TakeHidePhone.started += OnTakeHidePhone;
        controls.PlayerMoves.CameraMode.started += OnLookAtCamera;
        controls.PlayerMoves.PhoneLight.started += HandleFlashlightInput;
    }

    private void OnDisable()
    {

        controls.CameraMoves.NextCamera.started -= NextCamera;
        controls.CameraMoves.PreviousCamera.started -= PreviousCamera;
        controls.PlayerMoves.TakeHidePhone.started -= OnTakeHidePhone;
        controls.PlayerMoves.CameraMode.started -= OnLookAtCamera;
        controls.PlayerMoves.PhoneLight.started -= HandleFlashlightInput;
    }

    private void Update()
    {

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

    private void PreviousCamera(InputAction.CallbackContext ctx)
    {
        if (phoneController == null || CamerasScript.instance == null) return;

        if (phoneController.GetCurrentPhoneState() != PhoneState.Camera) return;

        CamerasScript.instance.PreviousCamera();
        Debug.Log("Previous");
    }
    private void NextCamera(InputAction.CallbackContext ctx)
    {
        if (phoneController == null || CamerasScript.instance == null) return;

        if (phoneController.GetCurrentPhoneState() != PhoneState.Camera) return;

        CamerasScript.instance.NextCamera();
        Debug.Log("Next");
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
