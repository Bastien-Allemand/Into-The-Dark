using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    PlayerAction controls;
    
    [Header("UI Visual References")]
    [SerializeField] private RawImage[] slotsImages = new RawImage[3];
    [SerializeField] private TextMeshProUGUI[] slotsTexts = new TextMeshProUGUI[3];

    [Header("Item Textures")]

    [SerializeField] private Texture batteryTexture;
    [SerializeField] private Texture ventolinTexture;
    [SerializeField] private Texture pillTexture;

    private int currentPillsCount = 0;
    private int currentBatteryCount = 0;
    private int currentVentolinCount = 0;


    private void Awake()
    {
        controls = InputManager.controls;
    }

    void Start()
    {
        RefreshSlotsUI();
    }

    private void OnEnable()
    {
        Inventory.OnPillsCountChanged += UpdatePillsUI;
        Inventory.OnBatteryCountChanged += UpdateBatteryUI;
        Inventory.OnVentolinCountChanged += UpdateVentolinUI;
        Inventory.OnInventoryRearranged += RefreshSlotsUI;
    }

    private void OnDisable()
    {
        Inventory.OnPillsCountChanged -= UpdatePillsUI;
        Inventory.OnBatteryCountChanged -= UpdateBatteryUI;
        Inventory.OnVentolinCountChanged -= UpdateVentolinUI;
        Inventory.OnInventoryRearranged -= RefreshSlotsUI;
    }

    void Update()
    {
    }

    void RefreshSlotsUI()
    {
        for (int i = 0; i < Inventory.instance.Slots.Count; i++)
        {
            ItemType typeInSlot = Inventory.instance.Slots[i];

            switch (typeInSlot)
            {
                case ItemType.Battery:
                    slotsImages[i].texture = batteryTexture;
                    slotsTexts[i].text = currentBatteryCount.ToString();
                    break;

                case ItemType.Ventolin:
                    slotsImages[i].texture = ventolinTexture;
                    slotsTexts[i].text = currentVentolinCount.ToString();
                    break;

                case ItemType.Pill:
                    slotsImages[i].texture = pillTexture;
                    slotsTexts[i].text = currentPillsCount.ToString();
                    break;
            }
        }
    }
    void UpdatePillsUI(int count)
    {
        currentPillsCount = count;
        RefreshSlotsUI();
    }

    void UpdateBatteryUI(int count)
    {
        currentBatteryCount = count;
        RefreshSlotsUI();
    }

    void UpdateVentolinUI(int count)
    {
        currentVentolinCount = count;
        RefreshSlotsUI();
    }



}
