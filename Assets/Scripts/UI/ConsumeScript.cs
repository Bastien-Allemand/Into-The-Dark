using UnityEngine;
using UnityEngine.InputSystem;

public class ConsumeScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory playerInventory;

    [Header("Effects Configurations")]
    [SerializeField] private GameEffect batteryEffect;
    [SerializeField] private GameEffect pillEffect;
    [SerializeField] private GameEffect ventolineEffect;

    [SerializeField] ConsumableType targetedType = ConsumableType.NONE;

    private bool takingPills;
    private float takingPillsDuration = 2f;
    [SerializeField] private float takingPillsCurrentTime = 0f;

    private bool takingVentolin;
    private float takingVentolinDuration = 0.1f;
    [SerializeField] private float takingVentolinCurrentTime = 0f;


    private PlayerAction controls;

    private void Awake()
    {
        controls = InputManager.controls;
    }

    private void OnEnable() => controls.GamePlay.Inventory.performed += OnInteract;
    private void OnDisable() => controls.GamePlay.Inventory.performed -= OnInteract;

    private void Update()
    {
        CheckConsume(targetedType);

    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInventory == null) return;

        targetedType = ConsumableType.NONE;
        
        if (context.control.name == "1")
        {
            targetedType = ConsumableType.BATTERY;
        }
        //else if (context.control.name == "2")
        //{
        //    targetedType = ConsumableType.PILL;
        //    takingPills = true;

        //}
        else if (context.control.name == "2")
        {
            targetedType = ConsumableType.VENTOLINE;
            takingVentolin = true;

        }
    }

    void CheckConsume(ConsumableType targetType)
    {
        int availableCount = playerInventory.GetConsumableCount(targetType);
        switch (targetType)
        {
            case ConsumableType.BATTERY:
                {
                    break;
                }
            case ConsumableType.PILL:
                {
                    if (takingPills)
                    {
                        takingPillsCurrentTime += Time.deltaTime;
                        if (takingPillsCurrentTime >= takingPillsDuration && availableCount > 0)
                        {
                            if(TryConsume(targetType))
                            { 
                                targetedType = ConsumableType.NONE;
                            }
                            takingPillsCurrentTime = 0f;
                            takingPills = false;
                        }
                    }
                    break;
                }
            case ConsumableType.VENTOLINE:
                {
                    if (takingVentolin)
                    {
                        takingVentolinCurrentTime += Time.deltaTime;
                        if (takingVentolinCurrentTime >= takingVentolinDuration && availableCount > 0)
                        {
                            if(TryConsume(targetType))
                            {
                                targetedType = ConsumableType.NONE;
                            }
                            takingVentolinCurrentTime = 0f;
                            takingVentolin = false;
                        }
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        
    }
    
    bool TryConsume(ConsumableType targetType)
    {
        GameEffect effectToApply = pillEffect;

        if (targetedType == ConsumableType.NONE || effectToApply == null) return false;

        effectToApply.ApplyEffect();

        playerInventory.Remove(1, targetedType);

        return true;
      
    }
}