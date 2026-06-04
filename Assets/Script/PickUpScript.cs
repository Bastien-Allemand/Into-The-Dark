using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    private Transform handSocket;

    void Start()
    {
        GameObject socketObj = GameObject.Find("RightHandSocket");
        if (socketObj != null)
        {
            handSocket = socketObj.transform;
        }
        else
        {
            Debug.LogError("RightHandSocket introuvable dans la scène !");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (handSocket != null)
            {
                transform.SetParent(handSocket);
                
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }
    }
}
