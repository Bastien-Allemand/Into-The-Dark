using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{

    [SerializeField] public Camera playerCamera;
    [SerializeField] public float pickupRange = 6f;
    [SerializeField] public Inventory inventoryscript;
    [SerializeField] public Raycast raycastUtil;
    [Header("Hands Position")]
    [SerializeField] public GameObject leftHand;
    [SerializeField] public GameObject rightHand;

    private HandContent leftHandContent;
    private HandContent rightHandContent;

    private PlayerAction controls;

    private enum itemType
    {
        NONE,
        GADGET,
        CONSUMABLE
    }
    private class Content
    {
        public itemType type = itemType.NONE;
        public GameObject GameObject = null;
    }
    private Content CheckHit()
    {
        Content result = new Content();
        result.GameObject = Raycast.CheckRaycast(playerCamera, pickupRange);

        if (result.GameObject != null)
        {
            if (result.GameObject.CompareTag("Item"))
                result.type = itemType.GADGET;
            else if (result.GameObject.CompareTag("Consumable"))
                result.type = itemType.CONSUMABLE;
        }

        return result;
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        Content item = CheckHit();

        if (item.type == itemType.NONE)
            return;

        switch (item.type)
        {
            case itemType.GADGET:

                if (context.control.name == "leftButton" && !leftHandContent.filled)
                {
                    leftHandContent.GiveObject(item.GameObject);
                }
                else if (context.control.name == "rightButton" && !rightHandContent.filled)
                {
                    rightHandContent.GiveObject(item.GameObject);
                }

                break;

            case itemType.CONSUMABLE:

                inventoryscript.AddConsumable(item.GameObject);
                Destroy(item.GameObject);
                break;
        }
    }

    private void OnEnable()
    {
        controls.GamePlay.TakePlaceobject.performed += OnInteract;
    }
    private void OnDisable()
    {
        controls.GamePlay.TakePlaceobject.performed -= OnInteract;
    }
    private void Awake()
    {
        controls = InputManager.controls;
        leftHandContent = leftHand.GetComponent<HandContent>();
        rightHandContent = rightHand.GetComponent<HandContent>();
    }
}
