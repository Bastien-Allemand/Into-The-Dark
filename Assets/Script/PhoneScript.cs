using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class PhoneScript : MonoBehaviour
{
    public GameObject phone;
    public Light phoneLight;
    public TextMeshProUGUI textBattery;

    public float maxBattery = 100; // Max Battery (percentage)
    public float currentBattery = 0; // Current Battery (percentage)
    public float maxTime = 60; // Time in seconds
    public float currentTimer = 0; // Current Timer  

    private void Start()
    {
        currentBattery = maxBattery;
        currentTimer = maxTime;
        textBattery.text = ((int)currentBattery).ToString() + "%";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            phone.SetActive(!phone.activeSelf);
            textBattery.enabled = !textBattery.enabled;

            if (!phone.activeSelf)
            {
                phoneLight.enabled = false;
            }
        }

        if (phoneLight.enabled == true)
        {
            currentTimer -= 8 * Time.deltaTime;
        }
        else if (phone.activeSelf)
        {
            currentTimer -= Time.deltaTime;
        }

        currentBattery = currentTimer * maxBattery / maxTime;
        textBattery.text = ((int)currentBattery).ToString() + "%";

        if (Input.GetMouseButtonDown(0) && phone.activeSelf)
        {
            phoneLight.enabled = !phoneLight.enabled;
        }

        if(currentBattery <= 0)
        {
            currentBattery = 0;
            phone.SetActive(false);
            textBattery.enabled = false;
        }
    }
}