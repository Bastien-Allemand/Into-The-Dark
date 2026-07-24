using UnityEngine;

public class MonkeyVisionScript : MonoBehaviour
{
    [Header("Vision")]
    public Camera visionCam;
    public string targetTag = "Ghost";

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip monkeySound;

    private bool canSeeGhost = false;

    void Start()
    {
        if (visionCam == null)
        {
            visionCam = GetComponentInChildren<Camera>();
        }

        if (audioSource != null)
        {
            audioSource.clip = monkeySound;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag(targetTag);

        if (target == null)
        {
            StopSound();
            return;
        }

        bool visible = false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(visionCam);

        if (GeometryUtility.TestPlanesAABB(planes, target.GetComponent<Collider>().bounds))
        {
            Vector3 dir = target.transform.position - visionCam.transform.position;

            if (Physics.Raycast(visionCam.transform.position, dir, out RaycastHit hit))
            {
                if (hit.transform.CompareTag(targetTag))
                {
                    visible = true;
                }
            }
        }

        if (visible && !canSeeGhost)
        {
            canSeeGhost = true;

            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (!visible && canSeeGhost)
        {
            canSeeGhost = false;

            StopSound();
        }
    }

    private void StopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}