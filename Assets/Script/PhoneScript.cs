using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneScript : MonoBehaviour
{
    public GameObject phone;
    public GameObject battery;
    public Light phoneLight;
    public TextMeshProUGUI textBattery;
    public BatteryScript batteryScript;

    public float maxBattery = 100; // Max Battery (percentage)
    public float currentBattery = 0; // Current Battery (percentage)
    public float maxTime = 60; // Time in seconds
    public float currentTimer = 0; // Current Timer  
    public bool haveBattery = true;

    void Start()
    {
        currentBattery = maxBattery;
        currentTimer = maxTime;
        textBattery.text = ((int)currentBattery).ToString() + "%";

        batteryScript.SetMaxBattery((int)maxBattery);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            phone.SetActive(!phone.activeSelf);
            battery.SetActive(!battery.activeSelf);
            textBattery.enabled = !textBattery.enabled;

            if (!phone.activeSelf || haveBattery == false)
            {
                phoneLight.enabled = false;
            }
        }

        if (phoneLight.enabled == true)
        {
            currentTimer -= 5 * Time.deltaTime;

            currentBattery = currentTimer * maxBattery / maxTime;
            batteryScript.SetBattery((int)currentBattery);
        }
        else if (phone.activeSelf)
        {
            currentTimer -= Time.deltaTime;

            currentBattery = currentTimer * maxBattery / maxTime;
            batteryScript.SetBattery((int)currentBattery);
        }

        textBattery.text = ((int)currentBattery).ToString() + "%";

        if (Input.GetMouseButtonDown(0) && phone.activeSelf)
        {
            phoneLight.enabled = !phoneLight.enabled;
        }

        if(currentBattery <= 0)
        {
            currentBattery = 0;
            haveBattery = false;
            phoneLight.enabled = false;
            textBattery.enabled = false;
        }
    }
}