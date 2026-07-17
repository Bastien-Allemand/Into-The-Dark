using UnityEngine;

[ExecuteInEditMode]
public class ProximityLightController : MonoBehaviour
{
    [Header("Paramètres de la Lumière")]
    public Light lampeTorche;
    public float rayonDeLumiere = 5f;

    [Header("Cibles à éclairer")]
    public Material[] materiauxCibles;

    void Update()
    {
        if (lampeTorche == null || materiauxCibles.Length == 0) return;

        if (lampeTorche.enabled && lampeTorche.gameObject.activeInHierarchy)
        {
            foreach (Material mat in materiauxCibles)
            {
                if (mat != null)
                {
                    mat.SetVector("_LightSourcePos", lampeTorche.transform.position);
                    mat.SetVector("_LightDir", lampeTorche.transform.forward);
                    mat.SetFloat("_LightRadius", rayonDeLumiere);
                }
            }
        }
        else
        {
            foreach (Material mat in materiauxCibles)
            {
                if (mat != null)
                {
                    mat.SetFloat("_LightRadius", 0f);
                }
            }
        }
    }
}