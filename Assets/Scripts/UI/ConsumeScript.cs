using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using static ItemScript;

public class ConsumeScript : MonoBehaviour
{
    [Header("References")]

    GameManager.NightData? Night = null;

    private Inventory CharacterInventory;
    private Camera CharacterCamera;
    private GameObject rightHand;
    private GameObject leftHand;

    [Header("Effects Configurations")]
    [SerializeField] private EffectBattery batteryEffect; 
    [SerializeField] private GameEffect pillEffect;
    [SerializeField] private GameEffect ventolineEffect;
    [SerializeField] ConsumableType targetType = ConsumableType.NONE;

    private bool reloadCamera = true;
    private bool InGame = false;

    private bool takingPills;
    private float takingPillsDuration = 2f;
    [SerializeField] private float takingPillsCurrentTime = 0f;

    private bool takingVentolin;
    private float takingVentolinDuration = 0.1f;
    [SerializeField] private float takingVentolinCurrentTime = 0f;

    private PlayerAction controls;

    private void Awake()
    {
        if (!Night.HasValue)
        {
            InGame = false;
            return;
        }
        InGame = true; 
        CharacterInventory = Night.Value.nightGO.GetComponent<Inventory>();
        CharacterCamera = Night.Value.characterOfTheNight.cam.GetComponent<Camera>();
        rightHand = Night.Value.characterOfTheNight.rightHand;
        leftHand = Night.Value.characterOfTheNight.leftHand;

        controls = InputManager.controls;

    }

    private void OnEnable()
    {
        controls.PlayerMoves.ConsumeItem1.performed += _ => OnInteract(ConsumableType.BATTERY);
        controls.PlayerMoves.ConsumeItem2.performed += _ => OnInteract(ConsumableType.PILL);
        controls.PlayerMoves.ConsumeItem3.performed += _ => OnInteract(ConsumableType.VENTOLINE);

    }
    private void OnDisable()
    {
        controls.PlayerMoves.ConsumeItem1.performed -= _ => OnInteract(ConsumableType.BATTERY);
        controls.PlayerMoves.ConsumeItem2.performed -= _ => OnInteract(ConsumableType.PILL);
        controls.PlayerMoves.ConsumeItem3.performed -= _ => OnInteract(ConsumableType.VENTOLINE);
    }

    private void Update()
    {
        if (!InGame)
            return;
        CheckConsume();
    }

    void OnInteract(ConsumableType item)
    {
        if (CharacterInventory == null || item == ConsumableType.NONE) return;

        targetType = item;
        
        switch(targetType)
        {
            case ConsumableType.BATTERY:
                reloadCamera = true;
                break;
            case ConsumableType.PILL:
                takingPills = true;
                break;
            case ConsumableType.VENTOLINE:
                takingVentolin = true;
                break;
        }
    }

    void CheckConsume()
    {
        bool availableCount = CharacterInventory.GetConsumableCount(targetType) > 0;
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


        CharacterInventory.Remove(1, ConsumableType.BATTERY);

        return true;
    }

    bool TryConsume(GameEffect effectToApply)
    {
       
        if (targetType == ConsumableType.NONE || effectToApply == null) return false;

        effectToApply.ApplyEffect();

        CharacterInventory.Remove(1, targetType);

        return true;
      
    }

    public void ActivateConsumeScriptUI(GameManager.NightData nightSelected)
    {
        Night = nightSelected;
        Awake();
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