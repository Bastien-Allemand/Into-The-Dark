using UnityEngine;

public class UseScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float pickupDistance = 10.0f;
    [SerializeField] private Transform phoneTransform;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
            {
                Debug.Log("Le Raycast a touché : " + hit.collider.name);

                if (hit.collider.CompareTag("Gadget"))
                {
                    //use item
                }
            }
        }
    }
}
