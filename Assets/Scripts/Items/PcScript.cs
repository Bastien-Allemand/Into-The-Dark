using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PcScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] public bool debug;

    [SerializeField] private CanvasGroup uiElement;

    [SerializeField] private GameObject pcScreen ;
    [SerializeField] private GameObject player ;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera pcCamera;

    public PlayerAction controls;
    public bool OnPC = false;
    public bool ExecutableCamera = false;

    void Awake()
    {
        controls = InputManager.controls;
        uiElement.alpha = 0;
        pcScreen.SetActive(false);

        pcCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (!OnPC && Input.GetKeyDown(KeyCode.T))
                ActivatePC();
            else if (Input.GetKeyDown(KeyCode.Y))
                DesactivatePC();
        }
        if (!OnPC)
            return;

        if (!ExecutableCamera)
            return;

    }
    void ActivatePC()
    {
        controls.GamePlay.Disable();
        controls.PcInteract.Enable();
        player.GetComponent<PlayerView>().canLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Rigidbody rigidbody = player.GetComponent<Rigidbody>();

        playerCamera.enabled = !playerCamera.enabled;
        pcCamera.enabled = !pcCamera.enabled;

        OnPC = true;

    }
    void DesactivatePC()
    {

        controls.PcInteract.Disable();
        controls.GamePlay.Enable();
        player.GetComponent<PlayerView>().canLook = true;
        OnPC = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera.enabled = !playerCamera.enabled;
        pcCamera.enabled = !pcCamera.enabled;
    }

    private void OnInteractLeft(InputAction.CallbackContext context)
    {
        CamerasScript.instance.PreviousCamera();
    }

    private void OnInteractRight(InputAction.CallbackContext context)
    {
        CamerasScript.instance.NextCamera();
    }

    public void ActivateExe()
    {
        uiElement.alpha = 1;
        pcScreen.SetActive(true);

        CamerasScript.instance.SetCameraViewActive(true);
    }
    public void DeactivateExe()
    {
        uiElement.alpha = 0;
        pcScreen.SetActive(false);
        CamerasScript.instance.SetCameraViewActive(false);
    }

    private void OnEnable()
    {
        controls.PcInteract.SwitchCameraLeft.performed += OnInteractLeft;
        controls.PcInteract.SwitchCameraRight.performed += OnInteractRight;
    }
    private void OnDisable()
    {
        controls.PcInteract.SwitchCameraLeft.performed -= OnInteractLeft;
        controls.PcInteract.SwitchCameraRight.performed -= OnInteractRight;
    }
}
