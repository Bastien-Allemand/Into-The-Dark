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
    //[SerializeField] private TextMeshProUGUI textBattery;
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
        if (Input.GetKeyDown(KeyCode.U))
        {
            TogglePhone();
        }

    }

    private void TogglePhone()
    {
        if (currentState == PhoneState.Hidden)
        {
            phoneTransform.gameObject.SetActive(true);

            currentState = PhoneState.Idle;

            battery.SetActive(true);
           // textBattery.enabled = true;

            waitingForHide = false;
        }
        else
        {
            currentState = PhoneState.Hidden;

            battery.SetActive(false);
          //  textBattery.enabled = false;
            phoneLight.enabled = false;
            screenPhone.SetActive(false);

            waitingForHide = true;
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

        // 2. Vérification de la distance par rapport à la CIBLE actuelle
        if (waitingForHide && Vector3.Distance(phoneTransform.localPosition, target.localPosition) < hideDistanceThreshold)
        {
            // On force les valeurs exactes de la cible pour éviter les décalages de micro-pixels
            phoneTransform.localPosition = target.localPosition;
            phoneTransform.localRotation = target.localRotation;
            phoneTransform.localScale = target.localScale;

            // Désactivation propre
            phoneTransform.gameObject.SetActive(false);
            waitingForHide = false;
        }
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
}