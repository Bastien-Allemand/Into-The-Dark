using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{

    [SerializeField] public Camera playerCamera;
    [SerializeField] public float pickupRange = 6f;
    [SerializeField] public Inventory inventoryscript;
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
    private itemType CheckHit(out GameObject _hitObject)
    {
        _hitObject = null;

        Raycast.RaycastResult hit = Raycast.CheckRaycast(playerCamera, pickupRange);

        if (hit == null)
            return itemType.NONE;

        _hitObject = hit.hitObject;

        if (_hitObject == null)
            return itemType.NONE;

        if (_hitObject.CompareTag("Item"))
            return itemType.GADGET;

        if (_hitObject.CompareTag("Consumable"))
            return itemType.CONSUMABLE;

        return itemType.NONE;
    }
    private void OnInteract()
    {
        float side = controls.PlayerInteraction.TakeUseObject.ReadValue<float>();
        HandContent hand;

        if (side < 0) hand = leftHandContent;
        else if (side > 0) hand = rightHandContent;
        else
        {
            Debug.Log($"hand = {side}");
            return;
        }

        GameObject hitObject;
        itemType item = CheckHit(out hitObject);

        if (item == itemType.NONE)
            return;
        if (hitObject == null)
            return;

        switch (item)
        {
            case itemType.GADGET:

                if (hand.filled)
                {
                    ItemScript itemScript = hitObject.GetComponent<ItemScript>();
                    if (itemScript.deployed)
                    {
                        if (itemScript.canBeRepickUp)
                            itemScript.deployed = false;
                        else
                            return;
                    }
                    hand.GiveObject(hitObject);
                }
                break;

            case itemType.CONSUMABLE:

                inventoryscript.AddConsumable(hitObject);

                Destroy(hitObject);

                break;
        }
    }

    private void OnEnable()
    {
        controls.PlayerInteraction.TakeUseObject.performed += _ => OnInteract();
    }
    private void OnDisable()
    {
        controls.PlayerInteraction.TakeUseObject.performed -= _ => OnInteract();
    }
    private void Awake()
    {
        controls = InputManager.controls;
        leftHandContent = leftHand.GetComponent<HandContent>();
        rightHandContent = rightHand.GetComponent<HandContent>();
    }
}
