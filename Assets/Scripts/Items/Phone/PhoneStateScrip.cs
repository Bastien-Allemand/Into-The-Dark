using UnityEngine;
using UnityEngine.InputSystem;

public enum PhoneState
{
    Hidden,
    Idle,
    Camera
}

public class PhoneStateScrip : MonoBehaviour
{
    [SerializeField] public bool isLookingCamera = false;
    public bool IsLookingCamera => isLookingCamera;

    private PlayerAction controls;
    private PhoneState currentState = PhoneState.Hidden;

    [Header("References")]
    [SerializeField] private Transform phoneTransform;
    [SerializeField] private MeshRenderer phoneMeshRenderer;
    [SerializeField] private GameObject screenPhone;
    [SerializeField] private GameObject battery;
    [SerializeField] private Light phoneLight;
    [SerializeField] private UIBattery uiBattery;

    [Header("Anchors")]
    [SerializeField] private Transform hiddenAnchor;
    [SerializeField] private Transform idleAnchor;
    [SerializeField] private Transform cameraAnchor;

    [Header("Animation")]
    [SerializeField] private float transitionSpeed = 8f;
    [SerializeField] private float hideDistanceThreshold = 0.01f;

    private bool waitingForHide;
    private bool wasSwitchLastFrame;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = InputManager.controls;
    }

    private void Start()
    {
        screenPhone.SetActive(false);
        battery.SetActive(false);
        phoneLight.enabled = false;

        phoneMeshRenderer.enabled = false;

        phoneTransform.localPosition = hiddenAnchor.localPosition;
        phoneTransform.localRotation = hiddenAnchor.localRotation;
        phoneTransform.localScale = hiddenAnchor.localScale;
    }

    private void Update()
    {
        UpdatePhoneTransform();
        UpdateBattery();
    }

    private void OnEnable()
    {
        controls.GamePlay.TakeHidePhone.started += OnTakeHidePhone;
        controls.GamePlay.Lookatcamera.started += OnLookAtCamera;
    }

    private void OnDisable()
    {
        controls.GamePlay.TakeHidePhone.started -= OnTakeHidePhone;
        controls.GamePlay.Lookatcamera.started -= OnLookAtCamera;
    }

    private void OnTakeHidePhone(InputAction.CallbackContext ctx)
    {
        TogglePhone();
    }

    private void OnLookAtCamera(InputAction.CallbackContext ctx)
    {
        ToggleCameraMode();
    }

    private void UpdateBattery()
    {
        if (currentState != PhoneState.Hidden)
        {
            uiBattery.HandleBatteryDrain();
        }
    }

    private Transform GetTargetAnchor()
    {
        return currentState switch
        {
            PhoneState.Hidden => hiddenAnchor,
            PhoneState.Camera => cameraAnchor,
            _ => idleAnchor
        };
    }

    private void UpdatePhoneTransform()
    {
        Transform target = GetTargetAnchor();

        phoneTransform.localPosition = Vector3.Lerp(
            phoneTransform.localPosition,
            target.localPosition,
            transitionSpeed * Time.deltaTime);

        phoneTransform.localRotation = Quaternion.Slerp(
            phoneTransform.localRotation,
            target.localRotation,
            transitionSpeed * Time.deltaTime);

        phoneTransform.localScale = Vector3.Lerp(
            phoneTransform.localScale,
            target.localScale,
            transitionSpeed * Time.deltaTime);

        if (waitingForHide && Vector3.Distance(phoneTransform.localPosition, target.localPosition) < hideDistanceThreshold)
        {
            phoneTransform.localPosition = target.localPosition;
            phoneTransform.localRotation = target.localRotation;
            phoneTransform.localScale = target.localScale;

            waitingForHide = false;
        }
    }

    private void HandleInputs()
    {
        moveInput = controls.GamePlay.Movement.ReadValue<Vector2>();
        bool isSwapThisFrame = Mathf.Abs(moveInput.x) > 0.5f;

        if (moveInput != Vector2.zero &&
            currentState == PhoneState.Camera &&
            isSwapThisFrame &&
            !wasSwitchLastFrame)
        {
            if (moveInput.x > 0)
                CamerasScript.instance.NextCamera();
            else if (moveInput.x < 0)
                CamerasScript.instance.PreviousCamera();
        }

        wasSwitchLastFrame = isSwapThisFrame;
    }

    private void TogglePhone()
    {
        if (currentState == PhoneState.Hidden)
        {
            currentState = PhoneState.Idle;

            phoneMeshRenderer.enabled = true;
            battery.SetActive(true);
            //uiBattery.textBattery.enabled = true;

            waitingForHide = false;
        }
        else
        {
            currentState = PhoneState.Hidden;

            phoneMeshRenderer.enabled = false;
            battery.SetActive(false);
            //uiBattery.textBattery.enabled = false;
            phoneLight.enabled = false;
            screenPhone.SetActive(false);

            isLookingCamera = false;

            if (CamerasScript.instance != null)
                CamerasScript.instance.SetCameraViewActive(false);

            waitingForHide = true;
        }
    }

    private void ToggleCameraMode()
    {
        if (currentState == PhoneState.Hidden)
            return;

        screenPhone.SetActive(!screenPhone.activeSelf);
        isLookingCamera = !isLookingCamera;

        currentState = currentState == PhoneState.Camera
            ? PhoneState.Idle
            : PhoneState.Camera;

        if (CamerasScript.instance != null)
            CamerasScript.instance.SetCameraViewActive(currentState == PhoneState.Camera);
    }

    public bool CanWatchCamera()
    {
        return uiBattery.HaveBattery && currentState == PhoneState.Idle;
    }

    public PhoneState GetCurrentPhoneState()
    {
        return currentState;
    }
}