using UnityEngine;

public class DroneBattery : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DroneScript droneScript;
    [SerializeField] private BatteryScript batteryUI;

    [Header("Battery")]
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float maxTime = 60f;

    private float currentBattery;
    private float currentTimer;
    private bool isUsingDrone;

    public bool HasBattery => currentBattery > 0;

    private void Start()
    {
        currentBattery = maxBattery;
        currentTimer = maxTime;

        batteryUI.SetMaxBattery((int)maxBattery);

        batteryUI.SetUIOff();
    }

    private void Update()
    {
        if (!isUsingDrone)
            return;

        batteryUI.SetUIOn();

        HandleBatteryDrain();
    }

    private void HandleBatteryDrain()
    {
        currentTimer -= Time.deltaTime;
        currentTimer = Mathf.Clamp(currentTimer, 0f, maxTime);

        currentBattery = (currentTimer / maxTime) * maxBattery;

        batteryUI.SetBattery((int)currentBattery);

        if (currentTimer <= 0f)
        {
            isUsingDrone = false;
            droneScript.StopControl();
        }
    }

    public void StartUsing()
    {
        if (!HasBattery)
            return;

        batteryUI.SetUIOn();
        isUsingDrone = true;
    }

    public void StopUsing()
    {
        batteryUI.SetUIOff();
        isUsingDrone = false;
    }
}