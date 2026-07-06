using Unity.VisualScripting;
using UnityEngine;

public class FootSteps : MonoBehaviour
{

    [SerializeField] private GameObject footprintPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 2f;

    [SerializeField] private float timeBetweenSteps = 2f;
    [SerializeField] private float currentTimeSteps = 0f;

    void Awake()
    {
        currentTimeSteps = 0f;
    }
    void Update()
    {
        CheckCanPlaceSteps();
    }

    void CheckCanPlaceSteps()
    {
        currentTimeSteps += Time.deltaTime;
        if (currentTimeSteps > timeBetweenSteps)
        {
            currentTimeSteps = 0f;
            Vector3 rayStart = playerTransform.position + Vector3.up * 0.5f;
            RaycastHit hit;
            if (Physics.Raycast(rayStart, Vector3.down, out hit, raycastDistance, groundLayer))
            {
                PlaceSteps(rayStart, hit);
            }
            else
            {
                Debug.DrawRay(rayStart, Vector3.down + Vector3.up * 0.5f, Color.green);
            }
        }
    }
    void PlaceSteps(Vector3 rayStart, RaycastHit hit)
    {

        Debug.DrawLine(rayStart, hit.point, Color.red);

        Vector3 spawnPosition = hit.point + (hit.normal * 0.005f);
        Vector3 forwardDirection = Vector3.ProjectOnPlane(playerTransform.forward, hit.normal);

        Quaternion spawnRotation = Quaternion.LookRotation(-forwardDirection, hit.normal);

        GameObject newDecal = Instantiate(footprintPrefab, spawnPosition, spawnRotation);

        Destroy(newDecal, 10f);
       
    }
}
