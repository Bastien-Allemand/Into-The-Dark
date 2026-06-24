using UnityEngine;

public class FootSteps : MonoBehaviour
{

    [SerializeField] private GameObject footprintPrefab;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 2f;
    void Start()
    {
        
    }
    void Update()
    {
        Vector3 rayStart = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(rayStart, Vector3.down, out hit, raycastDistance, groundLayer))
        {

            Debug.DrawLine(rayStart, hit.point, Color.red);

            Vector3 spawnPosition = hit.point;
            Quaternion spawnRotation = Quaternion.LookRotation(-hit.normal, transform.forward);

            GameObject newDecal = Instantiate(footprintPrefab, spawnPosition, spawnRotation);

            Destroy(newDecal, 10f);
        }
        else
        {
            Debug.DrawRay(rayStart, Vector3.back, Color.green);
        }

    }
}
