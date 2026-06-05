using TMPro;
using UnityEngine;

public enum PhoneState
{
    Hidden,
    Idle,
    Camera
}

public class PhoneScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform phoneTransform;
    [SerializeField] private GameObject battery;
    [SerializeField] private Light phoneLight;
    [SerializeField] private TextMeshProUGUI textBattery;
    [SerializeField] private BatteryScript batteryScript;
    [SerializeField] private GameObject screenPhone;

    [Header("Anchors")]
    [SerializeField] private Transform hiddenAnchor;
    [SerializeField] private Transform idleAnchor;
    [SerializeField] private Transform cameraAnchor;

    [Header("Animation")]
    [SerializeField] private float transitionSpeed = 8f;

    [Header("Battery")]
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float maxTime = 60f;
    [SerializeField] private float coeffBatteryLightUse = 5f;

    private float currentBattery;
    private float currentTimer;

    private PhoneState currentState = PhoneState.Hidden;

    public bool HaveBattery => currentBattery > 0;

    private void Start()
    {
        currentBattery = maxBattery;
        currentTimer = maxTime;

        screenPhone.SetActive(false);

        batteryScript.SetMaxBattery((int)maxBattery);
        UpdateUI();

        phoneTransform.localPosition = hiddenAnchor.localPosition;
        phoneTransform.localRotation = hiddenAnchor.localRotation;
        phoneTransform.localScale = hiddenAnchor.localScale;
    }

    private void Update()
    {
        HandleInputs();
        HandleBatteryDrain();
        HandleLight();
        UpdatePhoneTransform();
        UpdateUI();
        CheckBattery();
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TogglePhone();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleCameraMode();
        }
    }

    private void TogglePhone()
    {
        if (currentState == PhoneState.Hidden)
        {
            currentState = PhoneState.Idle;

            battery.SetActive(true);
            textBattery.enabled = true;
        }
        else
        {
            currentState = PhoneState.Hidden;

            battery.SetActive(false);
            textBattery.enabled = false;
            phoneLight.enabled = false;
            screenPhone.SetActive(false);
        }
    }

    private void ToggleCameraMode()
    {
        if (currentState == PhoneState.Hidden)
            return;

        screenPhone.SetActive(!screenPhone.activeSelf);

        currentState = currentState == PhoneState.Camera ? PhoneState.Idle : PhoneState.Camera;
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
    }

    private void HandleLight()
    {
        if (Input.GetMouseButtonDown(0)
            && currentState != PhoneState.Hidden
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

        currentBattery = (currentTimer / maxTime) * maxBattery;

        batteryScript.SetBattery((int)currentBattery);
    }

    private void UpdateUI()
    {
        textBattery.text = ((int)currentBattery) + "%";
    }

    private void CheckBattery()
    {
        if (currentTimer > 0)
            return;

        currentBattery = 0;
        phoneLight.enabled = false;
    }
}