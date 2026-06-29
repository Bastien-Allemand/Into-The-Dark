using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;
using static UnityEngine.UI.Image;
using Debug = UnityEngine.Debug;

public class PickUpScript : MonoBehaviour
{
    PlayerAction controls => JsonManager.controls;

    [Header("Reference")]
    [SerializeField] private RectTransform chargeBarTransform;
    [SerializeField] private GameObject objectPreview;
    GameObject preview;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask batteryLayerMask;

    private float initialChargeBarWidth;

    [Header("Sockets")]
    [SerializeField] private Transform leftHandSocket;
    [SerializeField] private Transform rightHandSocket;

    [Header("Settings")]
    [SerializeField] private float pickupDistance = 10.0f;
    [SerializeField] public Camera playerCamera;

    private GameObject rightHandItem = null;
    private GameObject leftHandItem = null;

    public bool isLeftHandEmpty = true;
    public bool isRightHandEmpty = true;

    [Header("Launch Settings")]
    [SerializeField] private float chargeSpeed = 15.0f;
    [SerializeField] private float minThrowForce = 5.0f;
    [SerializeField] private float maxThrowForce = 25.0f;
    [SerializeField] private float currentThrowCharge = 0f;

    [Header("Edit Settings")]
    [SerializeField] private bool editModeRight = false;
    [SerializeField] private bool editModeLeft = false;
    [SerializeField] private bool isRotating = false;
    public bool IsRotating => isRotating;
    private bool wasEditingLastFrame = false;
    [SerializeField] private float rotationSpeed = 3f;
    private bool canPlaceThisFrame = true;

    private bool wasInteractingLastFrame = false;
    private bool wasConsumeLastFrame = false;
    private bool isChargingRight = false;
    private bool isChargingLeft = false;
    private float percentLeft = 0f;
    private float editHoldTimer = 0f;
    private bool wasSwitchEditLastFrame = false;

    public float pillsStack;
    public float batteryStack;
    public float ventolinStack;
    private int batteryCount;
    public bool isConsume;

    void Start()
    {
        if (rightHandSocket == null)
        {
            GameObject rightSocketObj = GameObject.Find("RightHandSocket");
            if (rightSocketObj != null) rightHandSocket = rightSocketObj.transform;
        }

        if (leftHandSocket == null)
        {
            GameObject leftSocketObj = GameObject.Find("LeftHandSocket");
            if (leftSocketObj != null) leftHandSocket = leftSocketObj.transform;
        }

        initialChargeBarWidth = chargeBarTransform.rect.width;
        chargeBarTransform.sizeDelta = new Vector2(chargeBarTransform.rect.width * 0f, chargeBarTransform.rect.height);

        layerMask = LayerMask.GetMask("Placeable");
        batteryLayerMask = LayerMask.GetMask("ItemCamera");
    }
    void Update()
    {
        HandleInput();
        HandleConsumeInput();
        UpdateUI();

    }
    private void OnEnable()
    {
        Inventory.OnBatteryCountChanged += UpdateBatteryCount;
    }
    private void OnDisable()
    {
        Inventory.OnBatteryCountChanged -= UpdateBatteryCount;
    }
    private void OnDrawGizmos()
    {
        Vector3 start = playerCamera.transform.position;
        Vector3 end = start + (playerCamera.transform.forward * 2.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(start, end);
    }
    void HandleInput()
    {
        float interactValue = controls.GamePlay.Interact.ReadValue<float>() ;
        float editValue = controls.GamePlay.Edit.ReadValue<float>();
        float switchEdit = controls.GamePlay.EditSwitch.ReadValue<float>();
        bool isInteractingThisFrame = Mathf.Abs(interactValue) > 0.5f;
        bool isEditingThisFrame = Mathf.Abs(editValue) > 0.5f;

        if (editModeLeft == false && editModeRight == false)
        {
            if (isInteractingThisFrame && !wasInteractingLastFrame)
            {
                if (interactValue > 0.5f)
                {
                    if (rightHandItem != null)
                    {
                        isChargingRight = true;
                        currentThrowCharge = minThrowForce;
                    }
                    else
                    {
                        CheckInsight(true);
                    }
                }
                else if (interactValue < -0.5f)
                {
                    if (leftHandItem != null)
                    {
                        isChargingLeft = true;
                        currentThrowCharge = minThrowForce;
                    }
                    else
                    {
                        CheckInsight(false);
                    }
                }
            }

            //Charge
            if (isChargingRight || isChargingLeft)
            {
                currentThrowCharge += chargeSpeed * Time.deltaTime;
                currentThrowCharge = Mathf.Clamp(currentThrowCharge, minThrowForce, maxThrowForce);
            }
            if (!isInteractingThisFrame && wasInteractingLastFrame)
            {
                if (isChargingRight)
                {
                    LaunchItem(rightHandItem, currentThrowCharge);
                    isChargingRight = false;
                    rightHandItem = null;
                    isLeftHandEmpty = true;
                }
                else if (isChargingLeft)
                {
                    LaunchItem(leftHandItem, currentThrowCharge);
                    isChargingLeft = false;
                    leftHandItem = null;
                    isLeftHandEmpty = true;
                }
                currentThrowCharge = 0f;
                percentLeft = 0f;
            }
            wasInteractingLastFrame = isInteractingThisFrame;
        }

        //EditMode
        bool isEditStartedThisFrame = isEditingThisFrame && !wasEditingLastFrame;
        bool isEditReleasedThisFrame = !isEditingThisFrame && wasEditingLastFrame;
        bool isSwitchEditStartedThisFrame = (switchEdit > 0.5f) && !wasSwitchEditLastFrame;

        if (isSwitchEditStartedThisFrame)
        {
            if (editModeLeft || editModeRight)
            {
                QuitEditMode();
            }

            else if (rightHandItem != null && editModeRight == false)
            {
                StartEditModeRight();
                canPlaceThisFrame = true;
            }
            else if (leftHandItem != null && editModeLeft == false)
            {
                StartEditModeLeft();
                canPlaceThisFrame = true;
            }
        }
        wasSwitchEditLastFrame = (switchEdit > 0.5f);

        if (editModeRight == true)
        {
            if (editValue < -0.5f && leftHandItem != null)
            {
                StartEditModeLeft();
            }
            else
            {
                CheckPlaceable(rightHandItem, true, isEditReleasedThisFrame);
            }
                
        }

        else if (editModeLeft == true)
        {
            if (editValue > 0.5f && rightHandItem != null)
            {
                StartEditModeRight();
            }
            else
            {
                CheckPlaceable(leftHandItem, false, isEditReleasedThisFrame);
            }
        }

        if (isEditingThisFrame == true && canPlaceThisFrame == true)
        {
            if(editModeRight == true || editModeLeft == true)
            {
                editHoldTimer += Time.deltaTime;
                if(editHoldTimer > 0.15f)
                {
                    isRotating = true;

                    Vector2 mouse = controls.GamePlay.Look.ReadValue<Vector2>() * 0.25f;
                    RotateItem(mouse.x);
                }
            }
        }
        else
        {
            editHoldTimer = 0f;
        }

        if (isEditReleasedThisFrame && !canPlaceThisFrame)
        {
            canPlaceThisFrame = true;
            isEditReleasedThisFrame = false;
        }

        wasEditingLastFrame = isEditingThisFrame;
    }
    private void UpdateUI()
    {
        if (isChargingRight || isChargingLeft)
        {
            percentLeft = (currentThrowCharge - minThrowForce) / (maxThrowForce - minThrowForce);
        }
        chargeBarTransform.sizeDelta = new Vector2(initialChargeBarWidth * percentLeft, chargeBarTransform.rect.height);
    }
    void CheckInsight(bool isRightHand)
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Debug.DrawRay(ray.origin, ray.direction * pickupDistance, Color.red, 2f);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            Debug.Log("Le Raycast a touché : " + hit.collider.name);

            if (hit.collider.CompareTag("Item"))
            {
                PickUp(hit.collider.gameObject, isRightHand);
            }

            if (hit.collider.CompareTag("Lootable"))
            {
                Animator animator = hit.collider.GetComponent<Animator>();
                if (animator != null)
                {
                    bool isOpen = animator.GetBool("IsOpen");
                    animator.SetBool("IsOpen", !isOpen);
                }
            }

            if (hit.collider.CompareTag("Consumable"))
            {
                CollectibleScript item = hit.collider.GetComponent<CollectibleScript>();

                if (item != null)
                {
                    ItemType type = item.GetItemType();

                    switch(type)
                    {
                        case ItemType.Pill:
                        {
                            Inventory.instance.Add(ItemType.Pill);
                            break;
                        }
                        case ItemType.Battery:
                        {
                            Inventory.instance.Add(ItemType.Battery);
                            break;
                        }
                        case ItemType.Ventolin:
                        {
                            Inventory.instance.Add(ItemType.Ventolin);
                            Debug.Log("Ventolin Take");
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                    Destroy(hit.collider.gameObject);
                }

            }
        }
    }
    void PickUp(GameObject targetItem, bool isRightHand)
    {
        if (rightHandSocket != null && isRightHand == true)
        {
            rightHandItem = targetItem;
            isRightHandEmpty = false;
            AttachItem(targetItem, rightHandSocket);
        }
        else if(rightHandSocket != null && isRightHand == false)
        {
            leftHandItem = targetItem;
            isLeftHandEmpty = false;
            AttachItem(targetItem, leftHandSocket);
        }
    }
    private void AttachItem(GameObject item, Transform socket)
    {
        Vector3 originalWorldScale = item.transform.lossyScale;
        item.transform.SetParent(socket);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = new Vector3(
            originalWorldScale.x / socket.lossyScale.x,
            originalWorldScale.y / socket.lossyScale.y,
            originalWorldScale.z / socket.lossyScale.z
        );

        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider collider = item.GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        if(collider != null)
        {
            collider.enabled = false;
        }
    }
    private void LaunchItem(GameObject item, float currentThrowCharge)
    {
        Vector3 currentWorldScale = item.transform.lossyScale;
        item.transform.SetParent(null);
        item.transform.localScale = currentWorldScale;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider collider = item.GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = false;

            Vector3 throwDirection = playerCamera.transform.forward;

            rb.AddForce(throwDirection * currentThrowCharge, ForceMode.VelocityChange);
        }
        if (collider != null)
        {
            collider.enabled = true;
        }
        if(preview != null )
        {
            Destroy(preview);
        }
    }
    void CheckPlaceable(GameObject handItem, bool isRightHand, bool shouldPlace)
    {
        RaycastHit hit;
        Vector3 start = playerCamera.transform.position;
        Vector3 dir = playerCamera.transform.forward;
        float maxDistance = 5f;
        if (Physics.Raycast(start, dir, out hit, maxDistance, layerMask) && handItem != null)
        {
            if(hit.normal == new Vector3(0, 1, 0))
            {
                
                if (preview == null)
                {
                    preview = Instantiate(objectPreview, hit.point, Quaternion.identity);
                    Vector3 newPos = new Vector3(hit.point.x, hit.point.y + (preview.transform.localScale.y / 2), hit.point.z);
                    ReplaceMesh(handItem, preview);
                }
                else if (preview != null) 
                {
                    Vector3 newPos = new Vector3(hit.point.x, hit.point.y + (preview.transform.localScale.y / 2), hit.point.z);
                    preview.transform.position = newPos;
                    float editValue = controls.GamePlay.Edit.ReadValue<float>();
                    if (shouldPlace && canPlaceThisFrame == true)
                    {
                        PlaceItem(handItem);
                        Destroy(preview);
                        preview = null;

                        if (isRightHand) 
                        { 
                            rightHandItem = null;
                            isRightHandEmpty = true;
                            editModeRight = false; 
                        }
                        else 
                        { 
                            leftHandItem = null;
                            isLeftHandEmpty = true;
                            editModeLeft = false; 
                        }

                        isRotating = false;
                    }
                }
            }
        }
        else if (preview != null)
        {
            Destroy(preview);
            preview = null;
            if (shouldPlace == true)
            {
                if (isRightHand)
                {
                    editModeRight = false;
                }
                else
                {  
                    editModeLeft = false; 
                }
                isRotating = false;
            }
        }
    }
    void PlaceItem(GameObject handItem)
    {
        Vector3 currentWorldScale = handItem.transform.lossyScale;
        handItem.transform.SetParent(null);
        handItem.transform.localScale = currentWorldScale;
        handItem.transform.position = preview.transform.position;
        //handItem.transform.position = new Vector3(preview.transform.position.x, preview.transform.position.y + (handItem.transform.localScale.y / 2) - 0.1f, preview.transform.position.z);
        handItem.transform.rotation = preview.transform.rotation;
        Rigidbody rb = handItem.GetComponent<Rigidbody>();
        Collider collider = handItem.GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = true;
        } 
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
    void RotateItem(float mouseX)
    {
        if (preview != null)
        {
            preview.transform.Rotate(Vector3.up, -mouseX * rotationSpeed);
        }
    }
    void ReplaceMesh(GameObject handItem, GameObject preview) 
    {
        MeshFilter filterItem = handItem.GetComponent<MeshFilter>();
        MeshFilter filterPreview = preview.GetComponent<MeshFilter>();

        if (filterItem != null && filterPreview != null && filterItem.sharedMesh != filterPreview.sharedMesh)
        {
            filterPreview.sharedMesh = filterItem.sharedMesh;
        }
    }
    void StartEditModeRight()
    {
        Destroy(preview);
        editModeLeft = false;
        editModeRight = true;
        isRotating = false;
        canPlaceThisFrame = false;
    }
    void StartEditModeLeft()
    {
        Destroy(preview);
        editModeRight = false;
        editModeLeft = true;
        isRotating = false;
        canPlaceThisFrame = false;
    }
    void QuitEditMode()
    {
        if (preview != null) Destroy(preview);
        preview = null;
        editModeRight = false;
        editModeLeft = false;
        isRotating = false;
        canPlaceThisFrame = false;
    }
    void HandleConsumeInput()
    {
        float consumeValue = controls.GamePlay.Consume.ReadValue<float>();
        bool isConsumePressed = consumeValue > 0.5f;
        if (isConsumePressed && !wasConsumeLastFrame)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Debug.Log("R Pressed");
            if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, batteryLayerMask))
            {
                CameraBattery camBat = hit.collider.GetComponent<CameraBattery>();

                if (camBat != null)
                {
                    if (batteryCount > 0 && Inventory.instance.GetActiveSlot() == ItemType.Battery && camBat.batteryRemaining < camBat.maxBattery)
                    {
                        if (isLeftHandEmpty || isRightHandEmpty)
                        {
                            Inventory.instance.Remove(ItemType.Battery);
                            if(camBat.canReload == false)
                            {
                                camBat.canReload = true;
                            }
                        }
                    }
                }
            }
        }
        wasConsumeLastFrame = isConsumePressed;
    }
    void UpdateBatteryCount(int total)
    {
        batteryCount = total;
    }
}
