using UnityEngine;
using static UnityEngine.UI.Image;

public class PickUpScript : MonoBehaviour
{
    PlayerAction controls;

    [Header("Reference")]
    [SerializeField] private RectTransform chargeBarTransform;
    [SerializeField] private Camera _camera;

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
        if (rightHandItem != null ||leftHandItem != null)
        {
            CheckPlaceable();
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 start = _camera.transform.position;
        Vector3 end = new Vector3(start.x  + 2f, start.y + 2f, start.z + 2f);
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
                    CheckInsight(isRightHand: true);
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
                    CheckInsight(isRightHand: false);
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
                if(isRightHand == true)
                {
                    PickupRightHand(hit.collider.gameObject);
                }
                else
                {
                    PickupLeftHand(hit.collider.gameObject);
                }
            }
        }
    }
    private void PickupRightHand(GameObject targetItem)
    {
        if (rightHandSocket != null)
        {
            rightHandItem = targetItem;
            AttachItem(targetItem, rightHandSocket);
        }
        else
        {
            Debug.LogWarning("Impossible de ramasser : Les deux mains gèrent déjà un Item, ou un socket est manquant.");
        }
    }
    private void PickupLeftHand(GameObject targetItem)
    {
        if (leftHandSocket != null)
        {
            leftHandItem = targetItem;
            AttachItem(targetItem, leftHandSocket);
        }
        else
        {
            Debug.LogWarning("Impossible de ramasser : Les deux mains gèrent déjà un Item, ou un socket est manquant.");
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
    void CheckPlaceable()
    {
        RaycastHit hit;

        if(Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit))
        {
            
        }
    }
}
