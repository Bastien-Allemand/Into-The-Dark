using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using static ItemScript;

public class ConsumeScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Camera playerCamera; 
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    [Header("Effects Configurations")]
    [SerializeField] private EffectBattery batteryEffect; 
    [SerializeField] private GameEffect pillEffect;
    [SerializeField] private GameEffect ventolineEffect;
    [SerializeField] ConsumableType targetType = ConsumableType.NONE;

    private bool reloadCamera = true;

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

    private void OnEnable() => controls.PlayerInteraction.Inventory.performed += OnInteract;
    private void OnDisable() => controls.PlayerInteraction.Inventory.performed -= OnInteract;

    private void Update()
    {
        CheckConsume();
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInventory == null) return;

        targetType = ConsumableType.NONE;
        
        if (context.v == "1")
        {
            targetType = ConsumableType.BATTERY;
            reloadCamera = true;
        }
        else if (context.control.name == "2")
        {
            targetType = ConsumableType.PILL;
            takingPills = true;

        }
        //else if (context.control.name == "2")
        //{
        //    targetedType = ConsumableType.VENTOLINE;
        //    takingVentolin = true;

        //}
    }

    void CheckConsume()
    {
        bool availableCount = playerInventory.GetConsumableCount(targetType) > 0;
        switch (targetType)
        {
            case ConsumableType.BATTERY:
                {
                    if (rightHand != null && leftHand != null
                        && rightHand.TryGetComponent(out HandContent rightHD)
                        && leftHand.TryGetComponent(out HandContent leftHD)
                        && availableCount)
                    {
                        ItemScript itemToReload = null;
                        GameObject targetObj = null;

                        if (rightHD.inHand != null && rightHD.inHand.TryGetComponent(out ItemScript itemRight))
                        {
                            if (itemRight.type == ItemType.ITEM_CAMERA)
                            {
                                itemToReload = itemRight;
                                targetObj = rightHD.inHand;
                            }
                        }
                        else if (leftHD.inHand != null && leftHD.inHand.TryGetComponent(out ItemScript itemLeft))
                        {
                            if (itemLeft.type == ItemType.ITEM_CAMERA)
                            {
                                itemToReload = itemLeft;
                                targetObj = leftHD.inHand;
                            }
                        }

                        if (itemToReload != null && itemToReload.rechargable && itemToReload.energy < itemToReload.maxEnergy)
                        {
                            if (TryConsumeBattery(targetObj))
                            {
     
                                targetType = ConsumableType.NONE;
                            }
                        }
                        else
                        {
                            targetType = ConsumableType.NONE;
                        }
                    }
                    break;

                }
            case ConsumableType.PILL:
                {
                    if (takingPills)
                    {
                        takingPillsCurrentTime += Time.deltaTime;
                        if (takingPillsCurrentTime >= takingPillsDuration && availableCount)
                        {
                            if(TryConsume(pillEffect))
                            { 
                                targetType = ConsumableType.NONE;
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
                        if (takingVentolinCurrentTime >= takingVentolinDuration && availableCount)
                        {
                            if(TryConsume(ventolineEffect))
                            {
                                targetType = ConsumableType.NONE;
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

    bool TryConsumeBattery(GameObject targetObject)
    {
        if (batteryEffect == null) 
            return false;

      
        batteryEffect.ApplyBatteryEffect(targetObject);


        playerInventory.Remove(1, ConsumableType.BATTERY);

        return true;
    }

    bool TryConsume(GameEffect effectToApply)
    {
       
        if (targetType == ConsumableType.NONE || effectToApply == null) return false;

        effectToApply.ApplyEffect();

        playerInventory.Remove(1, targetType);

        return true;
      
    }
}

//-----------------------RAYCAST VERSION-----------------------------------//
//Camera cam = playerCamera != null ? playerCamera : gameObject.GetComponentInChildren<Camera>();
//if (cam == null) return;
//var result = Raycast.CheckRaycast(cam, 10f);

//if(result != null && result.hitObject != null)
//{
//   if(result.hitObject.TryGetComponent(out ItemScript item))
//   {
//        if(item.rechargable  && item.energy < 100f && availableCount)
//        {
//            if (TryConsumeBattery(result.hitObject))
//            {

//                targetType = ConsumableType.NONE;
//            }
//        }
//   }
//}