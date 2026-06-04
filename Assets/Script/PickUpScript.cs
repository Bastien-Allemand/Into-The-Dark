using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Transform handSocket;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.SetParent(handSocket);
            
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
