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

    public HandContent leftHandContent;
    public HandContent rightHandContent;

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
    private void OnInteract(InputAction.CallbackContext context)
    {
        GameObject hitObject;
        itemType item = CheckHit(out hitObject);

        if (item == itemType.NONE || hitObject == null)
            return;

        switch (item)
        {
            case itemType.GADGET:

                ItemScript itemScript = hitObject.GetComponent<ItemScript>();
                if (itemScript == null)
                    return;

                if (itemScript.deployed && !itemScript.canBeRepickUp)
                    return;

                if (itemScript.deployed && itemScript.canBeRepickUp)
                    itemScript.deployed = false;

                WorldCameraItem cameraItem = hitObject.GetComponent<WorldCameraItem>();

                if (context.control.name == "leftButton")
                {
                    if (!leftHandContent.filled)
                    {
                        leftHandContent.GiveObject(hitObject);

                        if (cameraItem != null)
                            cameraItem.PickUp();
                    }
                }
                else if (context.control.name == "rightButton")
                {
                    if (!rightHandContent.filled)
                    {
                        rightHandContent.GiveObject(hitObject);

                        if (cameraItem != null)
                            cameraItem.PickUp();
                    }
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

    public void DropLeftHand(bool restoreRb)
    {
        if (leftHandContent != null)
        {
            GameObject obj = leftHandContent.TakeOutObject(restoreRb);
        }
    }
}
