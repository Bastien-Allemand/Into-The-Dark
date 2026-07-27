using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class SoundOutil : MonoBehaviour
{
    public class visualDebug
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

    public visualDebug visualMinSound;
    public visualDebug visualMaxSound;

    private Color mColor;

    private bool lastState = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            visualMinSound = new visualDebug();
            visualMaxSound = new visualDebug();
            mColor = new Color(
                Random.value,
                Random.value,
                Random.value,
                alpha
            );
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
        if (!audioSource)
        {
            Destroy(this);
            return;
        }
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
        visualMinSound?._gameObject?.SetActive(true);
        visualMaxSound?._gameObject?.SetActive(true);
    }
    private void OnDisable()
    {
        visualMinSound?._gameObject?.SetActive(false);
        visualMaxSound?._gameObject?.SetActive(false);
    }
    private void OnDestroy()
    {
        if (visualMinSound != null) DestroyVisual(visualMinSound._gameObject);
        if (visualMaxSound != null) DestroyVisual(visualMaxSound._gameObject);
    }

    private void DestroyVisual(GameObject obj)  //  devrai être inutile car uniquement utiliser en Editor
    {
        if (obj == null)
            return;
#if UNITY_EDITOR
        DestroyImmediate(obj);
#else
    Destroy(obj);
#endif
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

            Vector3 parentScale = transform.lossyScale;
            sphere._gameObject.transform.localScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            );

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

        {
            Material mat = new Material(Resources.Load<Material>("Materials/transparentTemplate"));
            Debug.Log($"test {mat.name}");

            mat.SetColor("_BaseColor", mColor);
            mat.renderQueue = (int)RenderQueue.Transparent;

            sphere.GetComponent<Renderer>().material = mat;
        }
    }
}
