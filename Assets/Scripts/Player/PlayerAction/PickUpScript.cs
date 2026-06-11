using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;
using static UnityEngine.UI.Image;

public class PickUpScript : MonoBehaviour
{
    PlayerAction controls;

    [Header("Reference")]
    [SerializeField] private RectTransform chargeBarTransform;
    [SerializeField] private GameObject objectPreview;
    GameObject preview;
    [SerializeField] private LayerMask layerMask;

    private float initialChargeBarWidth;

    [Header("Sockets")]
    [SerializeField] private Transform leftHandSocket;
    [SerializeField] private Transform rightHandSocket;

    [Header("Settings")]
    [SerializeField] private float pickupDistance = 10.0f;
    [SerializeField] public Camera playerCamera;

    private GameObject rightHandItem = null;
    private GameObject leftHandItem = null;

    [Header("Launch Settings")]
    [SerializeField] private float chargeSpeed = 15.0f;
    [SerializeField] private float minThrowForce = 5.0f;
    [SerializeField] private float maxThrowForce = 25.0f;
    [SerializeField] private float currentThrowCharge = 0f;


    [SerializeField] private bool editModeRight = false;
    [SerializeField] private bool editModeLeft = false;
    private bool wasInteractingLastFrame = false;
    private bool isChargingRight = false;
    private bool isChargingLeft = false;
    private float percentLeft = 0f;


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
    }
    private void Awake()
    {
        controls = InputManager.controls;
    }
    void Update()
    {
        HandleInput();
        UpdateUI();
       
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
        float interactValue = controls.GamePlay.Interact.ReadValue<float>();
        bool isInteractingThisFrame = Mathf.Abs(interactValue) > 0.5f;
        if (isInteractingThisFrame && !wasInteractingLastFrame)
        {
            if (interactValue > 0.5f)
            {
                if (rightHandItem != null)
                {
                    //DropRightHandItem();
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
                    //DropLeftHandItem();
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
        if(isChargingRight || isChargingLeft)
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
            }
            else if (isChargingLeft)
            {
                LaunchItem(leftHandItem, currentThrowCharge);
                isChargingLeft = false;
                leftHandItem = null;
            }
            currentThrowCharge = 0f;
            percentLeft = 0f;
        }
        wasInteractingLastFrame = isInteractingThisFrame;

        //EditMode
        if (rightHandItem != null)
        {
            CheckPlaceable(rightHandItem, true);
        }
        else if (leftHandItem != null)
        {
            CheckPlaceable(leftHandItem, false);
        }
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
        }
    }

    void PickUp(GameObject targetItem, bool isRightHand)
    {
        if (rightHandSocket != null && isRightHand == true)
        {
            rightHandItem = targetItem;
            AttachItem(targetItem, rightHandSocket);
        }
        else if(rightHandSocket != null && isRightHand == false)
        {
            leftHandItem = targetItem;
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
    }
    void CheckPlaceable(GameObject handItem, bool isRightHand)
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
                    ReplaceMesh(handItem, preview);
                }
                else if (preview != null) 
                {
                    preview.transform.position = new Vector3(hit.point.x, hit.point.y + (preview.transform.localScale.y / 2), hit.point.z);
                    if (Input.GetMouseButtonDown(0))
                    {
                        
                        PlaceItem(handItem);
                        Destroy(preview);
                        preview = null;
                        if(isRightHand == true)
                        {
                            rightHandItem = null;
                        }
                        else
                        {
                            leftHandItem = null;
                        }
                    }
                }
                
            }
        }
        else if(preview != null)
        {
            Destroy(preview);
        }
    }
    void PlaceItem(GameObject handItem)
    {
        Vector3 currentWorldScale = handItem.transform.lossyScale;
        handItem.transform.SetParent(null);
        handItem.transform.localScale = currentWorldScale;

        handItem.transform.position = preview.transform.position;
        Rigidbody rb = handItem.GetComponent<Rigidbody>();
        Collider collider = handItem.GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        if (collider != null)
        {
            collider.enabled = true;
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

}
