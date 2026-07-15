using UnityEngine;

[RequireComponent(typeof(Fracture))]
public class OpenFractureSoundController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip fragmentImpactSound;

    [Header("Fragments Destruction")]
    [SerializeField] private bool autoDestroyFragments = true;
    [SerializeField] private float timeBeforeDisappear = 5f;
    [SerializeField] private float shrinkDuration = 1f;

    private Fracture fractureComponent;

    void Start()
    {
        fractureComponent = GetComponent<Fracture>();
    }

    public void ShatterWithSound()
    {
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position, 1.0f);
        }

        string fracturedGroupName = name + " (Fractured)";

        fractureComponent.CauseFracture();

        GameObject fracturedGroup = GameObject.Find(fracturedGroupName);

        if (fracturedGroup != null)
        {
            foreach (Transform fragment in fracturedGroup.transform)
            {
                FractureSound impactScript = fragment.gameObject.AddComponent<FractureSound>();
                impactScript.breakSound = fragmentImpactSound;

                if (autoDestroyFragments)
                {
                    FragmentLifetime lifetimeScript = fragment.gameObject.AddComponent<FragmentLifetime>();
                    lifetimeScript.lifetime = timeBeforeDisappear;
                    lifetimeScript.fadeDuration = shrinkDuration;
                }
            }

            if (autoDestroyFragments)
            {
                Destroy(fracturedGroup, timeBeforeDisappear + shrinkDuration + 0.5f);
            }
        }
    }
}