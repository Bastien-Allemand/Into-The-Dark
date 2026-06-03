using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MonsterVisionScript : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Camera enemyCamera;

    private GameObject targetObj;
    private bool PlayerFound = false;
    [SerializeField] private float rayonDetection;
    [SerializeField] private float chaseCD;
    private float chaseActualCD;

    void Start()
    {
        Collider[] allColliders = Object.FindObjectsByType<Collider>(FindObjectsSortMode.None);

        foreach (Collider col in allColliders)
        {
            if ((targetLayer.value & (1 << col.gameObject.layer)) > 0)
            {
                targetObj = col.gameObject;
                Debug.Log("targetObj " + targetObj.layer);
            }
        }

    }
    void Update()
    {
        EnnemyChase();

        if (!PlayerInFrustum())
            return;

        if (!RayConnectToPlayer())
            return;

        PlayerFound = true;
        chaseActualCD = chaseCD;
    }
    void EnnemyChase()
    {
        if (!PlayerFound)
            return;

        chaseActualCD -= 2 * Time.deltaTime;

        if (chaseActualCD < 0)
            PlayerFound = false;
    }
    bool PlayerInFrustum()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(enemyCamera);

        Collider targetCollider = targetObj.GetComponent<Collider>();

        return GeometryUtility.TestPlanesAABB(planes, targetCollider.bounds);
    }
    bool RayConnectToPlayer()
    {
        RaycastHit hit;
        bool result = false;

        if (targetObj == null) 
            return result;
        Vector3 direction = targetObj.transform.position - transform.position;

        Vector3 directionNormalisee = direction.normalized;

        Debug.DrawRay(transform.position, directionNormalisee, Color.blue);
        if (Physics.Raycast(transform.position, directionNormalisee, out hit, 1000, -1))
        {
            if (!hit.collider.gameObject.layer.Equals(targetObj.layer))
            {
                result = false;
                Debug.Log("obstacle touché " + hit.collider.gameObject.layer + " target layer :" );
            }
            else
            {
                result = true;
                Debug.Log("player trouvé");
            }
        }
        return result;
    }

}
