using UnityEngine;
using UnityEngine.UI;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    PlayerAction controls;

    public List<ItemType> Slots { get; private set; } = new List<ItemType> { ItemType.Battery, ItemType.Ventolin, ItemType.Pill };

    public static event Action<int> OnPillsCountChanged;
    public static event Action<int> OnBatteryCountChanged;
    public static event Action<int> OnVentolinCountChanged;
    public static event Action OnInventoryRearranged;
    [SerializeField] private int pillsCount = 0;
    [SerializeField] private int batteryCount = 0;
    [SerializeField] private int ventolinCount = 0;
    public ItemType ActiveItem => Slots[0];

    private bool wasInventoryPressedLastFrame = false;

    private void Awake()
    {
        controls = InputManager.controls;
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
        HandleInput();
    }

    void HandleInput()
    {
        float inventoryValue = controls.GamePlay.Inventory.ReadValue<float>();
        bool isPressedThisFrame = Mathf.Abs(inventoryValue) > 0.5f;

        if (isPressedThisFrame && !wasInventoryPressedLastFrame)
        {
            if (inventoryValue < -0.5f)
            {
                SwapObject(0, 1);
            }
            else if (inventoryValue > 0.5f)
            {
                SwapObject(0, 2);
            }
        }
        wasInventoryPressedLastFrame = isPressedThisFrame;
    }

    void SwapObject(int indexA, int indexB)
    {
        if (indexA >= Slots.Count || indexB >= Slots.Count) return;
        ItemType temp = Slots[indexA];
        Slots[indexA] = Slots[indexB];
        Slots[indexB] = temp;

        OnInventoryRearranged?.Invoke();
    }

    public void Add(ItemType type)
    {
        switch(type)
        {
            case ItemType.Pill:
            {
                    pillsCount++;
                    OnPillsCountChanged?.Invoke(pillsCount);
                    break;
            }
            case ItemType.Battery:
            {
                    batteryCount++;
                    OnBatteryCountChanged?.Invoke(batteryCount);
                    break;
            }
            case ItemType.Ventolin:
            {
                    ventolinCount++;
                    OnVentolinCountChanged?.Invoke(ventolinCount);
                    break;
            }
            default:
                break;
        }
    }

    public void Remove(ItemType type)
    {
        switch (type)
        {
            case ItemType.Pill:
            {
                pillsCount--;
                OnPillsCountChanged?.Invoke(pillsCount);
                break;
            }
            case ItemType.Battery:
            {
                batteryCount--;
                OnBatteryCountChanged?.Invoke(batteryCount);
                break;
            }
            case ItemType.Ventolin:
            {
                ventolinCount--;
                OnVentolinCountChanged?.Invoke(ventolinCount);
                break;
            }
            default:
                break;
        
        }
    }

    public ItemType GetActiveSlot()
    {
        return ActiveItem;
    }
}
