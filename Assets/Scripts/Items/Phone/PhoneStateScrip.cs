using UnityEngine;

public enum PhoneState
{
    Hidden,
    Idle,
    Camera
}

public class PhoneStateScrip : MonoBehaviour
{
    [SerializeField] private bool isLookingCamera = false;
    public bool IsLookingCamera => isLookingCamera;

    PlayerAction controls;

    private PhoneState currentState = PhoneState.Hidden;

    [Header("References")]
    [SerializeField] private Transform phoneTransform;
    [SerializeField] private GameObject screenPhone;
    [SerializeField] private GameObject battery;
    [SerializeField] private Light phoneLight;


    [Header("Anchors")]
    [SerializeField] private Transform hiddenAnchor;
    [SerializeField] private Transform idleAnchor;
    [SerializeField] private Transform cameraAnchor;

    [Header("Animation")]
    [SerializeField] private float transitionSpeed = 8f;

    [Header("Script")]
    [SerializeField] private UIBattery uiBattery;


    private bool waitingForHide;
    private bool wasSwitchLastFrame = false;

    [SerializeField] private float hideDistanceThreshold = 0.01f;
    [SerializeField] private Vector2 moveInput;

    void Awake()
    {
        controls = InputManager.controls;
    }

    void Start()
    {
        screenPhone.SetActive(false);
        phoneTransform.gameObject.SetActive(false);

        phoneTransform.localPosition = hiddenAnchor.localPosition;
        phoneTransform.localRotation = hiddenAnchor.localRotation;
        phoneTransform.localScale = hiddenAnchor.localScale;
    }

    void Update()
    {
        HandleInputs();
        UpdatePhoneTransform();
        UpdateBattery();
    }

    void UpdateBattery()
    {
        if (currentState == PhoneState.Hidden)
            return;

        uiBattery.HandleBatteryDrain();
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

            phoneTransform.gameObject.SetActive(false);
            waitingForHide = false;
        }
    }
    private void HandleInputs()
    {
        bool swapPhoneInput = controls.GamePlay.TakeHidePhone.triggered;

        bool lookCameraInput = controls.GamePlay.Lookatcamera.triggered;
        moveInput = controls.GamePlay.Movement.ReadValue<Vector2>();
        bool isSwapThisFrame = Mathf.Abs(moveInput.x) > 0.5f;

        if (swapPhoneInput == true)
        {
            TogglePhone();
        }
        if (lookCameraInput == true)
        {
            ToggleCameraMode();
        }
        if (moveInput != Vector2.zero && currentState == PhoneState.Camera && isSwapThisFrame && !wasSwitchLastFrame)
        {
            if (moveInput.x > 0)
            {
                Debug.Log("next cam");
                CamerasScript.instance.NextCamera();
            }
            else if (moveInput.x < 0)
            {
                Debug.Log("previous cam");
                CamerasScript.instance.PreviousCamera();
            }
        }

        wasSwitchLastFrame = isSwapThisFrame;
    }

    private void TogglePhone()
    {
        if (currentState == PhoneState.Hidden)
        {
            phoneTransform.gameObject.SetActive(true);

            currentState = PhoneState.Idle;

            battery.SetActive(true);
            //textBattery.enabled = true;

            waitingForHide = false;
        }
        else
        {
            currentState = PhoneState.Hidden;

            battery.SetActive(false);
            //textBattery.enabled = false;
            phoneLight.enabled = false;
            screenPhone.SetActive(false);

            isLookingCamera = false;
            if (CamerasScript.instance != null)
            {
                CamerasScript.instance.SetCameraViewActive(false);
            }

            waitingForHide = true;
        }
    }

    private void ToggleCameraMode()
    {
        if (currentState == PhoneState.Hidden)
            return;

        screenPhone.SetActive(!screenPhone.activeSelf);
        isLookingCamera = !isLookingCamera;
        currentState = currentState == PhoneState.Camera ? PhoneState.Idle : PhoneState.Camera;

        if (CamerasScript.instance != null)
        {
            CamerasScript.instance.SetCameraViewActive(currentState == PhoneState.Camera);
        }
    }

    public bool CanWatchCamera()
    {
        if (!uiBattery.HaveBattery)
            return false;

        return currentState.Equals(PhoneState.Idle);
    }

    public PhoneState GetCurrentPhoneState()
    {
        return currentState;
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }
}
