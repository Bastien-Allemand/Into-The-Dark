using TMPro;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public enum PhoneState
    {
        Hidden,
        Idle,
        Camera
    }

    public class PhoneScript : MonoBehaviour
    {
        PlayerAction controls;

        [Header("References")]
        [SerializeField] private Transform phoneTransform;
        [SerializeField] private GameObject battery;
        [SerializeField] private Light phoneLight;
        //[SerializeField] private TextMeshProUGUI textBattery;
        [SerializeField] private BatteryScript batteryScript;
        [SerializeField] private GameObject screenPhone;
        [SerializeField] private RectTransform phoneCanvaRectTransform;

        [Header("Anchors")]
        [SerializeField] private Transform hiddenAnchor;
        [SerializeField] private Transform idleAnchor;
        [SerializeField] private Transform cameraAnchor;

        [Header("Animation")]
        [SerializeField] private float transitionSpeed = 8f;

        [Header("Battery")]
        [SerializeField] private float maxBattery = 100000000000f;
        [SerializeField] private float maxTime = 60f;
        [SerializeField] private float coeffBatteryLightUse = 5f;

        [SerializeField] private Material fogMaterial;

        [SerializeField] private bool isLookingCamera = false;

        public bool IsLookingCamera => isLookingCamera;
        private bool wasSwitchLastFrame = false;

        [SerializeField] private Vector2 moveInput;


        private float currentBattery;
        private float currentTimer;

        private PhoneState currentState = PhoneState.Hidden;

        [SerializeField] private float hideDistanceThreshold = 0.01f;
        private bool waitingForHide;

        public bool HaveBattery => currentBattery > 0;

        private void Start()
        {
            currentBattery = maxBattery;
            currentTimer = maxTime;

            screenPhone.SetActive(false);
            phoneTransform.gameObject.SetActive(false);

            batteryScript.SetMaxBattery((int)maxBattery);
            UpdateUI();

            phoneTransform.localPosition = hiddenAnchor.localPosition;
            phoneTransform.localRotation = hiddenAnchor.localRotation;
            phoneTransform.localScale = hiddenAnchor.localScale;
        }

        private void Awake()
        {
            controls = InputManager.controls;
        }

        private void Update()
        {
            HandleInputs();
            HandleBatteryDrain();
            HandleLight();
            UpdatePhoneTransform();
            UpdateUI();
            CheckBattery();
            FlashLightShaderUpdate();
        }

        private void HandleInputs()
        {
            //bool swapPhoneInput = controls.GamePlay.SwapPhone.triggered;

            //bool lookCameraInput = controls.GamePlay.LookCamera.triggered;
            //moveInput = controls.GamePlay.Move.ReadValue<Vector2>();
            bool isSwapThisFrame = Mathf.Abs(moveInput.x) > 0.5f;
            //if (swapPhoneInput == true)
            //{
            //    TogglePhone();
            //}
            //if (lookCameraInput == true)
            //{
            //    ToggleCameraMode();
            //}
            //if (moveInput != Vector2.zero && currentState == PhoneState.Camera && isSwapThisFrame && !wasSwitchLastFrame) 
            //{ 
            //    if(moveInput.x > 0)
            //    {
            //        Debug.Log("next cam");
            //        CamerasScript.instance.NextCamera();
            //    }
            //    else if(moveInput.x < 0)
            //    {
            //        Debug.Log("previous cam");
            //        CamerasScript.instance.PreviousCamera();
            //    }
            //}

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

            if (currentState == PhoneState.Camera)
            {
                phoneCanvaRectTransform.localPosition = new Vector3(-0.05f, -0.39f, -0.45f);
                phoneCanvaRectTransform.localRotation = Quaternion.Euler(0f, phoneCanvaRectTransform.localEulerAngles.y, 90f);
            }
            else
            {
                phoneCanvaRectTransform.localPosition = new Vector3(-0.05f, 0.47f, -0.365f);
                phoneCanvaRectTransform.localRotation = Quaternion.Euler(0f, phoneCanvaRectTransform.localEulerAngles.y, 0f);
            }
        }

        private void HandleLight()
        {
            bool isClick = controls.GamePlay.ActiveFlashlight.triggered;
            if (isClick == true
                && currentState == PhoneState.Idle
                && HaveBattery)
            {
                phoneLight.enabled = !phoneLight.enabled;
            }
        }

        private void HandleBatteryDrain()
        {
            if (currentState == PhoneState.Hidden)
                return;

            float drainRate = phoneLight.enabled
                ? coeffBatteryLightUse
                : 1f;

            currentTimer -= drainRate * Time.deltaTime;
            currentTimer = Mathf.Clamp(currentTimer, 0f, maxTime);

            currentBattery = currentTimer / maxTime * maxBattery;

            batteryScript.SetBattery((int)currentBattery);
        }

        private void UpdateUI()
        {
            //  textBattery.text = ((int)currentBattery) + "%"; 
        }

        private void CheckBattery()
        {
            if (currentTimer > 0)
                return;

            currentBattery = 0;
            phoneLight.enabled = false;
        }

        public bool CanWatchCamera()
        {
            if (!HaveBattery)
                return false;

            return currentState.Equals(PhoneState.Idle);
        }

        public PhoneState GetCurrentPhoneState()
        {
            return currentState;
        }

        private void FlashLightShaderUpdate()
        {
            if (fogMaterial == null || phoneLight == null)
                return;
            fogMaterial.SetVector("_FlashlightPos", phoneLight.transform.position);
            fogMaterial.SetVector("_FlashlightDir", phoneLight.transform.forward);
        }
    }
}