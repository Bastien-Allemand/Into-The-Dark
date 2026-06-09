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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            Debug.DrawRay(ray.origin, ray.direction * pickupDistance, Color.red, 2f);

            if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
            {
                Debug.Log("Le Raycast a touché : " + hit.collider.name);

                if (hit.collider.CompareTag("Item"))
                {
                    Pickup(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropLeftHandItem();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            DropRightHandItem();
        }
    }

    private void Pickup(GameObject targetItem)
    {
        if (rightHandSocket != null && rightHandItem == null)
        {
            rightHandItem = targetItem;
            AttachItem(targetItem, rightHandSocket);
        }
        else if (leftHandSocket != null && leftHandItem == null)
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
        item.transform.SetParent(socket);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
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
        item.transform.SetParent(null);
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
