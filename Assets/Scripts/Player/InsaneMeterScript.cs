using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float insaneMeter = 0f;
    [SerializeField] private float maxInsaneMeter = 100f;
    [SerializeField] private float initalInsaneMeterWidth = 0f;

    [SerializeField] private RectTransform insaneBarTransform;
    public Camera visionCam;
    public string targetTag = "Ghost";
    public float insanityStack = 0f;
    private void Start()
    {
        if (insaneBarTransform != null)
        {
            initalInsaneMeterWidth = insaneBarTransform.rect.width;
        }
    }
    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag(targetTag);

        if (target == null)
        {
            Debug.Log("Target not found");
            return;
        }

        UpdateInsanity(target);

        UpdateBarUI();
    }
    void UpdateInsanity(GameObject _target)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(visionCam);
        bool isTargetVisible = false;
        if (GeometryUtility.TestPlanesAABB(planes, _target.GetComponent<Collider>().bounds))
        {
            Vector3 dir = _target.transform.position - visionCam.transform.position;
            if (Physics.Raycast(visionCam.transform.position, dir, out RaycastHit hit))
            {
                if (hit.transform.CompareTag(targetTag))
                {
                    isTargetVisible = true;
                    Debug.Log("ghost in vision");
                }
            }
        }
        if (isTargetVisible)
        {
            insanityStack += Time.deltaTime * 0.1f; // Increase the insanity stack over time while the target is visible
            if (insanityStack > 6)
            {
                insanityStack = 6;
            }
            insaneMeter += insanityStack * 0.5f; // Increase the insane meter over time while the target is visible
        }
        else if (insaneMeter > 0)
        {
            insanityStack = 0f; // Reset the insanity stack when the target is not visible
            insaneMeter -= Time.deltaTime * 0.5f; // Decrease the insane meter over time when the target is not visible
        }

        float distanceToTarget = Vector3.Distance(visionCam.transform.position, _target.transform.position);
        if (distanceToTarget < 5f)
        {
            insaneMeter += (distanceToTarget/ 5f * Time.deltaTime) * 2; // Increase the insane meter more rapidly as the target gets closer
        }

        if (insaneMeter > 100)
        {
            insaneMeter = 100;
        }
    }
    void UpdateBarUI()
    {
        if (insaneBarTransform == null) return;
        float percentLeft = insaneMeter / 100;
        insaneBarTransform.sizeDelta = new Vector2(initalInsaneMeterWidth * percentLeft, insaneBarTransform.rect.height);
    }
}
