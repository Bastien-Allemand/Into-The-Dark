using UnityEngine;

public class MonkeyVisionScript : MonoBehaviour
{
    public Camera VisionCam;
    public string targetTag = "Ghost";
    void Start()
    {

    }
    void Action()
    {
        Debug.Log("Action performed on target");
    }
    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag(targetTag);

        if (target == null)
        {
            Debug.Log("Target not found");
            return;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(VisionCam);

        if (GeometryUtility.TestPlanesAABB(planes, target.GetComponent<Collider>().bounds))
        {
            Vector3 dir = target.transform.position - VisionCam.transform.position;
            if (Physics.Raycast(VisionCam.transform.position, dir, out RaycastHit hit))
            {
                if (hit.transform.CompareTag(targetTag))
                {
                    Debug.Log("Target is visible and not occluded");
                    Action();
                }
                else
                {
                    Debug.Log("Target is occluded by " + hit.transform.name);
                }
            }
        }
    }
}