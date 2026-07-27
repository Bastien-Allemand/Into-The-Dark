using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class SoundDebug : MonoBehaviour
{
    private struct visualDebug
    {
        public GameObject _gameObject;
        public float rayon;
    }
    private enum size
    {
        Min, Max
    }

    public static float alpha = 0.18f;

    private AudioSource audioSource;

    private visualDebug visualMinSound;
    private visualDebug visualMaxSound;

    private bool lastState = false;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            CreateTransparentSphere(visualMaxSound, size.Max);
            CreateTransparentSphere(visualMinSound, size.Min);
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
            visualMaxSound._gameObject.SetActive(lastState);
            visualMinSound._gameObject.SetActive(lastState);
        }
        if (lastState)
        {
            sizeUpdate(visualMaxSound, size.Max);
            sizeUpdate(visualMinSound, size.Min);
        }
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    private void sizeUpdate(visualDebug sphere, size size)
    {
        float distance = size switch
        {
            size.Min => audioSource.minDistance,
            size.Max => audioSource.maxDistance,
            _ => -1
        };
        if (distance == -1) return;
        if (sphere.rayon != distance)
        {
            sphere.rayon = distance;

            //   adapt le scale de base par rapport au parent pour obtenir une sphere
            Vector3 parentScale = transform.lossyScale;
            sphere._gameObject.transform.localScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            );

            //  augment la scale par rapport a la force du son
            float diameter = sphere.rayon * 2f;
            sphere._gameObject.transform.localScale *= diameter;
        }
    }
    private void CreateTransparentSphere(visualDebug gameObject, size size)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = "OutilsAudioSphereVisual";

        sphere.transform.SetParent(transform, false);

        gameObject._gameObject = sphere;
        gameObject.rayon = 0;
        sizeUpdate(gameObject, size);


        Collider col = sphere.GetComponent<Collider>();
        if (col != null)
            Destroy(col);

        {   // le mat utiliser (randomisation de la couleur et transparence, transparence pas encore terminer)

            Material mat = Resources.Load<Material>("Materials/transparentTemplate");
            Debug.Log($"test {mat.name}");


            Color randomColor = new Color(
                Random.value,
                Random.value,
                Random.value,
                alpha
            );
            //mat.color = randomColor;
            mat.SetColor("_BaseColor", randomColor);

            //mat.SetFloat("_Surface", 1);
            //mat.SetFloat("_Blend", 0);
            //mat.SetFloat("_ZWrite", 0);
            //mat.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);

            //mat.SetFloat("_Cull", (float)UnityEngine.Rendering.CullMode.Off);


            //mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            //mat.EnableKeyword("_ALPHABLEND_ON");
            mat.renderQueue = (int)RenderQueue.Transparent;

            sphere.GetComponent<Renderer>().material = mat;
        }

    }
}
