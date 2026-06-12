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

    private string[] currentSlotContents = new string[3] { "Battery", "Ventolin", "Pill" };

    private bool wasInventoryPressedLastFrame = false;

    private void Awake()
    {
        controls = InputManager.controls;
    }
    void Start()
    {
    }

    private void OnEnable()
    {
        Inventory.OnPillsCountChanged += UpdatePillsUI;
        Inventory.OnBatteryCountChanged += UpdateBatteryUI;
        Inventory.OnVentolinCountChanged += UpdateVentolinUI;
    }
    private void OnDisable()
    {
        Inventory.OnPillsCountChanged -= UpdatePillsUI;
        Inventory.OnBatteryCountChanged -= UpdateBatteryUI;
        Inventory.OnVentolinCountChanged -= UpdateVentolinUI;
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
                Debug.Log("1 Pressed");
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

        if (slotsImages[indexA] == null || slotsImages[indexB] == null) return;
        Texture tempTex = slotsImages[indexA].texture;
        slotsImages[indexA].texture = slotsImages[indexB].texture;
        slotsImages[indexB].texture = tempTex;

        string tempText = slotsTexts[indexA].text;
        slotsTexts[indexA].text = slotsTexts[indexB].text;
        slotsTexts[indexB].text = tempText;

        string tempContent = currentSlotContents[indexA];
        currentSlotContents[indexA] = currentSlotContents[indexB];
        currentSlotContents[indexB] = tempContent;
    }

    void UpdatePillsUI(int currentPills)
    {
        
        for (int i = 0; i < currentSlotContents.Length; i++)
        {
            if (currentSlotContents[i] == "Pill")
            {
                if (slotsTexts[i] != null)
                {
                    slotsTexts[i].text = currentPills.ToString();
                }
                break;
            }
        }

    }

    void UpdateBatteryUI(int currentBattery)
    {

        for (int i = 0; i < currentSlotContents.Length; i++)
        {
            if (currentSlotContents[i] == "Battery")
            {
                if (slotsTexts[i] != null)
                { 
                    slotsTexts[i].text = currentBattery.ToString();
                }
                break;
            }
        }

    }

    void UpdateVentolinUI(int currentVentolin)
    {

        for (int i = 0; i < currentSlotContents.Length; i++)
        {
            if (currentSlotContents[i] == "Ventolin")
            {
                if (slotsTexts[i] != null)
                {
                    slotsTexts[i].text = currentVentolin.ToString();
                }
                break;
            }
        }

    }

}
