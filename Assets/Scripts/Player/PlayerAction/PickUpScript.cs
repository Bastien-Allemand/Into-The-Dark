using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [Header("Sockets (À glisser dans l'inspecteur si non trouvés)")]
    [SerializeField] private Transform leftHandSocket;
    [SerializeField] private Transform rightHandSocket;

    [Header("Settings")]
    [SerializeField] private float pickupDistance = 10.0f;
    [SerializeField] public Camera playerCamera;

    private GameObject rightHandItem = null;
    private GameObject leftHandItem = null;

    PlayerAction controls;

    private bool wasInteractingLastFrame = false;
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
    }
    private void Awake()
    {
        controls = InputManager.controls;
    }
    void Update()
    {
        float interactValue = controls.GamePlay.Interact.ReadValue<float>();
        bool isInteractingThisFrame = Mathf.Abs(interactValue) > 0.5f;

        if (isInteractingThisFrame && !wasInteractingLastFrame)
        {
            if (interactValue > 0.5f) 
            {
                if (rightHandItem != null)
                {
                    DropRightHandItem();
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
                    DropLeftHandItem();
                }
                else
                {
                    CheckInsight(isRightHand: false);
                }
            }
        }
        wasInteractingLastFrame = isInteractingThisFrame;
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
    private void DropRightHandItem()
    {
        if (rightHandItem != null)
        {
            DetachItem(rightHandItem);
            rightHandItem = null;
        }
    }
    private void DropLeftHandItem()
    {
        if (leftHandItem != null)
        {
            DetachItem(leftHandItem);
            leftHandItem = null;
        }
    }
    private void DetachItem(GameObject item)
    {
        Vector3 currentWorldScale = item.transform.lossyScale;
        item.transform.SetParent(null);
        item.transform.localScale = currentWorldScale;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        Collider collider = item.GetComponent<Collider>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
}
