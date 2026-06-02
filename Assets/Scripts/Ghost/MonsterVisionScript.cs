using UnityEngine;

public class MonsterVisionScript : MonoBehaviour
{
    [SerializeField] public float rayonDetection = 5f;

    public float angleVision = 180f;
    public float hauteurYeux = 1.0f;
    [SerializeField] public LayerMask cibleLayer;
    [SerializeField] public LayerMask obstacleLayer;

    private bool PlayerFound = false;

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //RaycastHit hit;
        //Debug.DrawRay(transform.position, transform.up, Color.red);
        //if (Physics.Raycast(transform.position, transform.right, out hit, 10))
        //{
        //    Debug.Log("Raycast connect");
        //}

    }

    void Update()
    {

        Vector3 origineVision = transform.position + Vector3.up * hauteurYeux;
        Collider[] ciblesDansLeRayon = Physics.OverlapSphere(transform.position, rayonDetection, cibleLayer);

        foreach (Collider cible in ciblesDansLeRayon)
        {
            Transform cibleTransform = cible.transform;

            Vector3 directionVersCible = (cibleTransform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionVersCible) < angleVision / 2)
            {
                float distanceVersCible = Vector3.Distance(origineVision, cibleTransform.position);

                if (!Physics.Raycast(origineVision, directionVersCible, distanceVersCible, obstacleLayer))
                {
                    Debug.DrawRay(origineVision, cibleTransform.position, Color.green);
                }
                else
                {
                    Debug.DrawRay(origineVision, cibleTransform.position, Color.red);
                    Debug.Log($"Cible détectée : {cible.name} est vue depuis les yeux !");
                }
            }
        }
    }
}
