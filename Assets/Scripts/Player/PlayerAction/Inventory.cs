using UnityEngine;
using UnityEngine.UI;
using System;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public static event Action<int> OnPillsCountChanged;
    public static event Action<int> OnBatteryCountChanged;
    public static event Action<int> OnVentolinCountChanged;
    [SerializeField] private int pillsCount = 0;
    [SerializeField] private int batteryCount = 0;
    [SerializeField] private int ventolinCount = 0;

    private void Awake()
    {

        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
    }
    
    void Update()
    {
    }

    public void AddPill()
    {
        pillsCount++;
        OnPillsCountChanged?.Invoke(pillsCount);
    }

    public void RemovePill()
    {
        pillsCount--;
        OnPillsCountChanged?.Invoke(pillsCount);
    }
    public void AddBattery()
    {
        batteryCount++;
        OnBatteryCountChanged?.Invoke(batteryCount);
    }

    public void RemoveBattery()
    {
        batteryCount--;
        OnBatteryCountChanged?.Invoke(batteryCount);
    }
    public void AddVentolin()
    {
        ventolinCount++;
        OnVentolinCountChanged?.Invoke(ventolinCount);
    }

    public void RemoveVentolin()
    {
        ventolinCount--;
        OnVentolinCountChanged?.Invoke(ventolinCount);
    }
}
