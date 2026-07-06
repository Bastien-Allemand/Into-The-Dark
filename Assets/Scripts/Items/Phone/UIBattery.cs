using TMPro;
using UnityEngine;

public class UIBattery : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject battery;
    //[SerializeField] public TextMeshProUGUI textBattery;
    [SerializeField] private BatteryScript batteryScript;
    [SerializeField] private Light phoneLight;


    [Header("Battery")]
    [SerializeField] private float maxBattery = 100000000000f;
    [SerializeField] private float maxTime = 60f;

    PhoneController phoneStateScript;
    [SerializeField] private float coeffBatteryLightUse = 5f;


    private float currentBattery;
    private float currentTimer;

    public bool HaveBattery => currentBattery > 0;

    private void Start()
    {
        currentBattery = maxBattery;
        currentTimer = maxTime;

        batteryScript.SetMaxBattery((int)maxBattery);
        UpdateUI();
    }

    private void Awake()
    {
        phoneStateScript = GetComponent<PhoneController>();
    }

    private void Update()
    {
        UpdateUI();
        CheckBattery();

    }

    private void UpdateUI()
    {
        //textBattery.text = ((int)currentBattery) + "%"; 
    }

    private void CheckBattery()
    {
        if (currentTimer > 0)
            return;

        currentBattery = 0;
        phoneLight.enabled = false;
    }

    public void HandleBatteryDrain()
    {

        float drainRate = phoneLight.enabled
            ? coeffBatteryLightUse
            : 1f;

        currentTimer -= drainRate * Time.deltaTime;
        currentTimer = Mathf.Clamp(currentTimer, 0f, maxTime);

        currentBattery = (currentTimer / maxTime) * maxBattery;

        batteryScript.SetBattery((int)currentBattery);
    }
}