using System;
using TMPro;
using UnityEngine;

public class PhoneScript : MonoBehaviour
{
    public GameObject phone; // The phone (GameObject)
    public GameObject battery; // The UI battery (GameObject)
    public Light phoneLight; // The flashlight of the phone
    public TextMeshProUGUI textBattery; // The text percentage of battery left
    public BatteryScript batteryScript; // The script of the battery 

    public float maxBattery = 100f; // Max Battery (percentage)
    public float currentBattery = 0f; // Current Battery (percentage)
    public float maxTime = 60f; // Max battery time without flash (seconds)
    public float currentTimer = 0f; // Current Timer (indicate the battery left in seconds)
    public float coeffBatteryLightUse = 5f; // Coeff to deplete battery faster when light is active
    public bool HaveBattery => currentBattery > 0; // Indication if the phone have battery left or not

    void Start()
    {
        currentBattery = maxBattery;
        currentTimer = maxTime;
        textBattery.text = ((int)currentBattery).ToString() + "%";

        batteryScript.SetMaxBattery((int)maxBattery);
    }

    void Update()
    {
        HandleInputs();
        HandleBatteryDrain();
        HandleLight();
        UpdateUI();
        CheckBattery();
    }

    private void CheckBattery()
    {
        if (currentTimer <= 0)
        {
            currentBattery = 0;
            phoneLight.enabled = false;
            textBattery.enabled = false;
        }
    }

    private void UpdateUI()
    {
        textBattery.text = ((int)currentBattery).ToString() + "%";
    }

    private void HandleLight()
    {
        if (Input.GetMouseButtonDown(0) && phone.activeSelf && HaveBattery)
        {
            phoneLight.enabled = !phoneLight.enabled;
        }
    }

    private void HandleBatteryDrain()
    {
        if (!phone.activeSelf)
            return;

        float drainRate = phoneLight.enabled ? coeffBatteryLightUse : 1f;

        currentTimer -= drainRate * Time.deltaTime;
        currentTimer = Mathf.Clamp(currentTimer, 0f, maxTime);

        currentBattery = currentTimer / maxTime * maxBattery;
        batteryScript.SetBattery((int)currentBattery);
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TogglePhone();
        }
    }

    private void TogglePhone()
    {
        bool isActive = !phone.activeSelf;

        phone.SetActive(isActive);
        battery.SetActive(isActive);
        textBattery.enabled = isActive;

        if (!isActive)
        {
            phoneLight.enabled = false;
        }
    }
}