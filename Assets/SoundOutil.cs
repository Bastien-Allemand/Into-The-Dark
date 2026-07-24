using UnityEngine;
using UnityEngine.UIElements;

public class SoundDebug : MonoBehaviour
{
    private static float alpha = 0.35f;


    private AudioSource audioSource;
    private GameObject debugVisualObject;
    private bool lastState = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            CreateTransparentSphere();
            lastState = audioSource.isPlaying;
        }
        else
        {
            Debug.Log($"No Audio Source sur ce GameObject : {transform.gameObject.name}");
            Destroy(this);
        }
    }
    private void Update()
    {
        if (lastState != audioSource.isPlaying)
        {
            lastState = audioSource.isPlaying;
            debugVisualObject.SetActive(lastState);
        }
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }
    private void CreateTransparentSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = "OutilsAudioSphereVisual";

        sphere.transform.SetParent(transform, false);

        //   adapt le scale de base par rapport au parent pour obtenir une sphere
        Vector3 parentScale = transform.lossyScale;
        sphere.transform.localScale = new Vector3(
            1f / parentScale.x,
            1f / parentScale.y,
            1f / parentScale.z
        );
        //  augment la scale par rapport a la force du son
        float diameter = audioSource.maxDistance * 2f;
        sphere.transform.localScale *= diameter;

        Collider col = sphere.GetComponent<Collider>();
        if (col != null)
            Destroy(col);

        {   // le mat utiliser (randomisation de la couleur et transparence, transparence pas encore terminer)
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));

            Color randomColor = new Color(
                Random.value,
                Random.value,
                Random.value,
                alpha
            );
            mat.color = randomColor;

            // Passe le matériau en Transparent
            mat.SetFloat("_Surface", 1);

            mat.SetOverrideTag("RenderType", "Transparent");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            sphere.GetComponent<Renderer>().material = mat;
        }
        debugVisualObject = sphere;
    }
}
